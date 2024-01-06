using System.Collections.Generic;

public class Photosynthesis : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        // AnimationManager.Instance.StartAnimation("QuantaGenerate", target.transform, Element.Life);
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, Element.Life, Owner.Owner, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
