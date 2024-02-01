using System.Collections.Generic;

public class Deadalive : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "DeadAndAlive", Element.Other));
        EventBus<OnDeathTriggerEvent>.Raise(new OnDeathTriggerEvent());
    }
}
