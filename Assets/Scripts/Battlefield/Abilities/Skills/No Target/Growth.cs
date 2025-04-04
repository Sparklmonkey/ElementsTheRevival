using System.Collections.Generic;

public class Growth : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.DefModify += 2;
        targetCard.AtkModify += 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
