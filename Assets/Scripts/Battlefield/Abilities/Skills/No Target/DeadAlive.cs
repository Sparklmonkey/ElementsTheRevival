using System.Collections.Generic;

public class Deadalive : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        AnimationManager.Instance.StartAnimation("DeadAndAlive", target.transform);
        EventBus<OnDeathDTriggerEvent>.Raise(new OnDeathDTriggerEvent());
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
