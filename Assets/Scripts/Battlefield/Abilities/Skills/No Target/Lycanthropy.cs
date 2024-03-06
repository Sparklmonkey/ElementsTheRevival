using System.Collections.Generic;

public class Lycanthropy : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.AtkModify += 5;
        targetCard.DefModify += 5;
        targetCard.Skill = null;
        targetCard.Desc = "";
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
