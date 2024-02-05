using System.Collections.Generic;

public class Steam : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Charge += 5;
        targetCard.AtkModify += 5;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
