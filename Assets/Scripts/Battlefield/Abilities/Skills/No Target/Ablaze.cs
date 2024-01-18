using System.Collections.Generic;

public class Ablaze : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card) => false;
    public override void Activate(ID targetId, Card targetCard)
    {
        targetCard.AtkModify += 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
