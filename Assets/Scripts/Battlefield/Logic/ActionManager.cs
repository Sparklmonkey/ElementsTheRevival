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
            action = new ElementAction("You", $"Drew {ownerCard.name}", "", "", ownerCard.imageID, null, false);
        }
        else
        {
            action = new ElementAction(BattleVars.shared.enemyAiData.opponentName, "Drew A Card", "", "", null, null, false);
        }
        actionList.Add(action);
    }

    public static void AddCardPlayedAction(bool isPlayer, Card ownerCard)
    {
        //ElementAction action;

        //if (isPlayer)
        //{
        //    action = new ElementAction("You", $"Played {ownerCard.name}", "", "", ownerCard.imageID, null, false);
        //}
        //else
        //{
        //    action = new ElementAction(BattleVars.shared.isPvp ? "Game_PvpHubConnection.shared.GetOpponentName()" : BattleVars.shared.enemyAiData.opponentName, $"Played {ownerCard.name}", "", "", ownerCard.imageID, null, false);
        //}
        //actionList.Add(action);
    }
    public static void AddCardPlayedWithTargetAction(bool isPlayer, Card ownerCard, Card targetCard, bool isSelfTarget)
    {
        //ElementAction action;

        //if (isPlayer)
        //{
        //    action = new ElementAction("You", $"Played {ownerCard.name}", isSelfTarget ? "You" : BattleVars.shared.isPvp ? "Game_PvpHubConnection.shared.GetOpponentName()" : BattleVars.shared.enemyAiData.opponentName, targetCard != null ? $"Targeting {targetCard.name}" : "", ownerCard.imageID, targetCard != null ? targetCard.imageID : "", true);
        //}
        //else
        //{
        //    action = new ElementAction(BattleVars.shared.isPvp ? "Game_PvpHubConnection.shared.GetOpponentName()" : BattleVars.shared.enemyAiData.opponentName, $"Played {ownerCard.name}", isSelfTarget ? BattleVars.shared.isPvp ? "Game_PvpHubConnection.shared.GetOpponentName()" : BattleVars.shared.enemyAiData.opponentName : "You", targetCard != null ? $"Targeting {targetCard.name}" : "", ownerCard.imageID, targetCard != null ? targetCard.imageID : "", true);
        //}
        //actionList.Add(action);
    }
    public static void AddActivateAbilityWithTargetAction(bool isPlayer, Card ownerCard, Card targetCard, bool isSelfTarget)
    {
        //ElementAction action;

        //if (isPlayer)
        //{
        //    action = new ElementAction("You", $"Activated {ownerCard.name}", isSelfTarget ? "You" : BattleVars.shared.isPvp ? "Game_PvpHubConnection.shared.GetOpponentName()" : BattleVars.shared.enemyAiData.opponentName, targetCard != null ? $"Targeting {targetCard.name}" : "", ownerCard.imageID, targetCard != null ? targetCard.imageID : "", true);
        //}
        //else
        //{
        //    action = new ElementAction(BattleVars.shared.isPvp ? "Game_PvpHubConnection.shared.GetOpponentName()" : BattleVars.shared.enemyAiData.opponentName, $"Activated {ownerCard.name}", isSelfTarget ? BattleVars.shared.isPvp ? "Game_PvpHubConnection.shared.GetOpponentName()" : BattleVars.shared.enemyAiData.opponentName : "You", targetCard != null ? $"Targeting {targetCard.name}" : "", ownerCard.imageID, targetCard != null ? targetCard.imageID : "", true);
        //}
        //actionList.Add(action);
    }
    //public static void AddSpellPlayedAction(bool isPlayer, Card ownerCard, Card targetCard, bool isSelfTarget)
    //{
    //    ElementAction action;

    //    if (isPlayer)
    //    {
    //        action = new ElementAction("You", $"Played {ownerCard.name}", "", "", ownerCard.imageID, null, false);
    //        action = new ElementAction("You", "Played", ownerCard.name, "", "", "", ImageHelper.GetCardImage(ownerCard.imageID), null, null);
    //    }
    //    else
    //    {
    //        action = new ElementAction("", "", "", ownerCard.name, "Played By", BattleVars.shared.isPvp ? Game_PvpHubConnection.shared.GetOpponentName() : BattleVars.shared.enemyAiData.opponentName, null, ImageHelper.GetCardImage(ownerCard.imageID), null);
    //    }
    //    actionList.Add(action);
    //}
}


public class ElementAction
{
    public string playerName;
    public string playerCard;

    public string enemyName;
    public string enemyCard;

    public string playerSprite;
    public string targetSprite;
    public bool shouldShowArrow;

    public ElementAction(string playerName, string playerCard, string enemyName, string enemyCard, string playerSprite, string targetSprite, bool shouldShowArrow)
    {
        this.playerName = playerName;
        this.playerCard = playerCard;
        this.enemyName = enemyName;
        this.enemyCard = enemyCard;
        this.playerSprite = playerSprite;
        this.targetSprite = targetSprite;
        this.shouldShowArrow = shouldShowArrow;
    }

}