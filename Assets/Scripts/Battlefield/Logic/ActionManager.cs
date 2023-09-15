using System.Collections.Generic;

public class ActionManager
{
    public static List<ElementAction> ActionList = new();

    public static void AddCardDrawAction(bool isPlayer, Card ownerCard)
    {
        ElementAction action;

        if (isPlayer)
        {
            action = new($"{PlayerData.Shared.userName}", "Draw", ownerCard.imageID, "", false);
        }
        else
        {
            action = new($"{BattleVars.Shared.EnemyAiData.opponentName}", "Draw", "", "", false);
        }
        ActionList.Add(action);
    }

    public static void AddCardPlayedOnFieldAction(bool isPlayer, Card ownerCard)
    {
        ElementAction action = new($"{(isPlayer ? PlayerData.Shared.userName : BattleVars.Shared.EnemyAiData.opponentName)}", "Played", ownerCard.imageID, "", false);

        ActionList.Add(action);
    }
    public static void AddSpellPlayedAction(bool isPlayer, IDCardPair origin, IDCardPair target)
    {
        string owner = isPlayer ? PlayerData.Shared.userName : BattleVars.Shared.EnemyAiData.opponentName;
        bool shouldShowArrow = target.HasCard();
        string targetId = target.HasCard() ? target.card.imageID : "";
        ElementAction action = new(owner, "Played Spell", origin.card.imageID, targetId, shouldShowArrow);

        ActionList.Add(action);
    }

    public static void AddAbilityActivatedAction(bool isPlayer, IDCardPair origin, IDCardPair target)
    {
        string owner = isPlayer ? PlayerData.Shared.userName : BattleVars.Shared.EnemyAiData.opponentName;
        bool shouldShowArrow = target.HasCard();
        string targetId = target.HasCard() ? target.card.imageID : "";
        ElementAction action = new(owner, "Activated Ability", origin.card.imageID, targetId, shouldShowArrow);

        ActionList.Add(action);
    }
}


public class ElementAction
{
    public string Owner;
    public string Action;

    public string OriginImage;
    public string TargetImage;
    public bool ShouldShowArrow;

    public ElementAction(string owner, string action, string originImage, string targetImage, bool shouldShowArrow)
    {
        this.Owner = owner;
        this.Action = action;
        this.OriginImage = originImage;
        this.TargetImage = targetImage;
        this.ShouldShowArrow = shouldShowArrow;
    }

    public ElementAction()
    {

    }
}