using System.Collections.Generic;

public class Divineshield : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.passiveSkills.DivineShield = true;
        targetCard.innateSkills.Immaterial = true;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
