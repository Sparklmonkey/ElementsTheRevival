using System.Collections.Generic;

public class Photosynthesis : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        AnimationManager.Instance.StartAnimation("QuantaGenerate", target.transform, Element.Life);
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, Element.Life, Owner.isPlayer, true));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
