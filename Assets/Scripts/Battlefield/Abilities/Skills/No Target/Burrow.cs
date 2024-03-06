using System.Collections.Generic;

public class Burrow : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard.passiveSkills.Burrow)
        {
            targetCard.passiveSkills.Burrow = false;
            targetCard.Atk *= 2;
            targetCard.AtkModify *= 2;
        }
        else
        {
            targetCard.passiveSkills.Burrow = true;
            targetCard.Atk /= 2;
            targetCard.AtkModify /= 2;
        }
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
