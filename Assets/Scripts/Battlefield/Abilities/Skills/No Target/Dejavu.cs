using System.Collections.Generic;

public class Dejavu : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.skill = "";
        targetCard.desc = "";
        Card dupe = new(targetCard);
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(dupe, targetId.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, dupe));
        
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
