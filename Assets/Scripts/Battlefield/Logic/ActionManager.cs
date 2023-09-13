using System.Collections.Generic;

public class ActionManager
{
    public static List<ElementAction> actionList = new();

    public static void AddCardDrawAction(bool isPlayer, Card ownerCard)
    {
        ElementAction action;

        if (isPlayer)
        {
            action = new($"{PlayerData.shared.userName}", "Draw", ownerCard.imageID, "", false);
        }
        else
        {
            action = new($"{BattleVars.shared.enemyAiData.opponentName}", "Draw", "", "", false);
        }
        actionList.Add(action);
    }

    public static void AddCardPlayedOnFieldAction(bool isPlayer, Card ownerCard)
    {
        ElementAction action = new($"{(isPlayer ? PlayerData.shared.userName : BattleVars.shared.enemyAiData.opponentName)}", "Played", ownerCard.imageID, "", false);

        actionList.Add(action);
    }
    public static void AddSpellPlayedAction(bool isPlayer, IDCardPair origin, IDCardPair target)
    {
        string owner = isPlayer ? PlayerData.shared.userName : BattleVars.shared.enemyAiData.opponentName;
        bool shouldShowArrow = target.HasCard();
        string targetId = target.HasCard() ? target.card.imageID : "";
        ElementAction action = new(owner, "Played Spell", origin.card.imageID, targetId, shouldShowArrow);

        actionList.Add(action);
    }

    public static void AddAbilityActivatedAction(bool isPlayer, IDCardPair origin, IDCardPair target)
    {
        string owner = isPlayer ? PlayerData.shared.userName : BattleVars.shared.enemyAiData.opponentName;
        bool shouldShowArrow = target.HasCard();
        string targetId = target.HasCard() ? target.card.imageID : "";
        ElementAction action = new(owner, "Activated Ability", origin.card.imageID, targetId, shouldShowArrow);

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