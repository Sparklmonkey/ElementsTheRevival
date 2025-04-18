using System.Collections.Generic;

public class Stoneform : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.DefModify += 20;
        targetCard.Skill = null;
        targetCard.Desc = "";
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
