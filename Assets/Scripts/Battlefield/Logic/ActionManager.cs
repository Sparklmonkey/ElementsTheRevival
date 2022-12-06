using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager
{
    public static List<ElementAction> actionList = new List<ElementAction>();

    public static void AddCardDrawAction(bool isPlayer, Card ownerCard)
    {
        ElementAction action;

        if (isPlayer)
        {
            action = new ElementAction($"{ApiManager.shared.GetEmailAndUsername().Item2}", "Draw", ownerCard.imageID, "", false);
        }
        else
        {
            action = new ElementAction($"{BattleVars.shared.enemyAiData.opponentName}", "Draw", "", "", false);
        }
        actionList.Add(action);
    }

    public static void AddCardPlayedOnFieldAction(bool isPlayer, Card ownerCard)
    {
        ElementAction action = new ElementAction($"{(isPlayer ? ApiManager.shared.GetEmailAndUsername().Item2 : BattleVars.shared.enemyAiData.opponentName)}", "Played", ownerCard.imageID, "", false);
        
        actionList.Add(action);
    }
    public static void AddSpellPlayedAction(bool isPlayer, Card origin, Card target)
    {
        string owner = isPlayer ? ApiManager.shared.GetEmailAndUsername().Item2 : BattleVars.shared.enemyAiData.opponentName;
        bool shouldShowArrow = target != null;
        string targetId = target == null ? "" : target.imageID;
        ElementAction action = new ElementAction(owner, "Played Spell", origin.imageID, targetId, shouldShowArrow);
        
        actionList.Add(action);
    }

    public static void AddAbilityActivatedAction(bool isPlayer, Card origin, Card target)
    {
        string owner = isPlayer ? ApiManager.shared.GetEmailAndUsername().Item2 : BattleVars.shared.enemyAiData.opponentName;
        bool shouldShowArrow = target != null;
        string targetId = target == null ? "" : target.imageID;
        ElementAction action = new ElementAction(owner, "Activated Ability", origin.imageID, targetId, shouldShowArrow);

        actionList.Add(action);
    }
}


public class ElementAction
{
    public string owner;
    public string action;

    public string originImage;
    public string targetImage;
    public bool shouldShowArrow;

    public ElementAction(string owner, string action, string originImage, string targetImage, bool shouldShowArrow)
    {
        this.owner = owner;
        this.action = action;
        this.originImage = originImage;
        this.targetImage = targetImage;
        this.shouldShowArrow = shouldShowArrow;
    }

    public ElementAction()
    {

    }
}