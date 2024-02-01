using System.Collections.Generic;

public class Dive : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.passiveSkills.Dive = true;
        targetCard.AtkModify *= 2;
        targetCard.atk *= 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
