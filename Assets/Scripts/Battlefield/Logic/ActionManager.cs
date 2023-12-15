using System.Collections.Generic;

public class ActionManager
{
    public static List<ElementAction> ActionList = new();

    private EventBinding<AddDrawCardActionEvent> _addCardDrawActionBinding;
    private EventBinding<AddCardPlayedOnFieldActionEvent> _addCardPlayedOnFieldBinding;
    private EventBinding<AddSpellActivatedActionEvent> _addSpellActivatedBinding;
    private EventBinding<AddAbilityActivatedActionEvent> _addAbilityActivatedBinding;
    
    public ActionManager()
    {
        _addCardDrawActionBinding = new EventBinding<AddDrawCardActionEvent>(AddCardDrawAction);
        EventBus<AddDrawCardActionEvent>.Register(_addCardDrawActionBinding);
        
        _addCardPlayedOnFieldBinding = new EventBinding<AddCardPlayedOnFieldActionEvent>(AddCardPlayedOnFieldAction);
        EventBus<AddCardPlayedOnFieldActionEvent>.Register(_addCardPlayedOnFieldBinding);
        
        _addSpellActivatedBinding = new EventBinding<AddSpellActivatedActionEvent>(AddSpellPlayedAction);
        EventBus<AddSpellActivatedActionEvent>.Register(_addSpellActivatedBinding);
        
        _addAbilityActivatedBinding = new EventBinding<AddAbilityActivatedActionEvent>(AddAbilityActivatedAction);
        EventBus<AddAbilityActivatedActionEvent>.Register(_addAbilityActivatedBinding);
    }
    
    private static void AddCardDrawAction(AddDrawCardActionEvent addDrawCardActionEvent)
    {
        var isPlayer = addDrawCardActionEvent.IsPlayer;
        ElementAction action = new(isPlayer ? PlayerData.Shared.userName : BattleVars.Shared.EnemyAiData.opponentName, 
            "Draw", isPlayer ? addDrawCardActionEvent.CardDrawn.imageID : "", "", false);
        ActionList.Add(action);
    }

    private static void AddCardPlayedOnFieldAction(AddCardPlayedOnFieldActionEvent addCardPlayedOnFieldActionEvent)
    {
        ElementAction action = new($"{(addCardPlayedOnFieldActionEvent.IsPlayer ? PlayerData.Shared.userName : BattleVars.Shared.EnemyAiData.opponentName)}", "Played", addCardPlayedOnFieldActionEvent.CardToPlay.imageID, "", false);

        ActionList.Add(action);
    }
    private static void AddSpellPlayedAction(AddSpellActivatedActionEvent addSpellActivatedActionEvent)
    {
        var owner = addSpellActivatedActionEvent.IsPlayer ? PlayerData.Shared.userName : BattleVars.Shared.EnemyAiData.opponentName;
        var shouldShowArrow = false;
        var targetId = "";
        if (addSpellActivatedActionEvent.Target != null)
        {
            shouldShowArrow = addSpellActivatedActionEvent.Target.HasCard() || addSpellActivatedActionEvent.Target.id.field.Equals(FieldEnum.Player);
            targetId = addSpellActivatedActionEvent.Target.HasCard() ? addSpellActivatedActionEvent.Target.card.imageID : "";
        }
        ElementAction action = new(owner, "Played Spell", addSpellActivatedActionEvent.Spell.imageID, targetId, shouldShowArrow);

        ActionList.Add(action);
    }

    private static void AddAbilityActivatedAction(AddAbilityActivatedActionEvent addAbilityActivatedActionEvent)
    {
        var owner = addAbilityActivatedActionEvent.IsPlayer ? PlayerData.Shared.userName : BattleVars.Shared.EnemyAiData.opponentName;
        var shouldShowArrow = false;
        var targetId = "";
        if (addAbilityActivatedActionEvent.Target != null)
        {
            shouldShowArrow = addAbilityActivatedActionEvent.Target.HasCard() || addAbilityActivatedActionEvent.Target.id.field.Equals(FieldEnum.Player);
            targetId = addAbilityActivatedActionEvent.Target.HasCard() ? addAbilityActivatedActionEvent.Target.card.imageID : "";
        }
        ElementAction action = new(owner, "Activated Ability", addAbilityActivatedActionEvent.AbilityOwner.imageID, targetId, shouldShowArrow);

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
        Owner = owner;
        Action = action;
        OriginImage = originImage;
        TargetImage = targetImage;
        ShouldShowArrow = shouldShowArrow;
    }

    public ElementAction()
    {

    }
}