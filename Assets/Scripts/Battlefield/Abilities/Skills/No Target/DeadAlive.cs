using System.Collections.Generic;

public class Deadalive : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        // AnimationManager.Instance.StartAnimation("DeadAndAlive", target.transform);
        EventBus<OnDeathTriggerEvent>.Raise(new OnDeathTriggerEvent());
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
