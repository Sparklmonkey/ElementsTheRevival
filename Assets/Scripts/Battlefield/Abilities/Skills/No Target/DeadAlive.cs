using System.Collections.Generic;

public class Deadalive : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        AnimationManager.Instance.StartAnimation("DeadAndAlive", target.transform);
        DuelManager.Instance.ActivateDeathTriggers();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}
