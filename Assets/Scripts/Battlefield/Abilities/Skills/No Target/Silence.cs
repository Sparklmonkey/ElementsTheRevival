using System.Collections.Generic;

public class Silence : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) 
    {
        return id.Equals(
            new ID(BattleVars.Shared.AbilityIDOrigin.owner.Not(), FieldEnum.Player, 0));
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Silence, targetId.owner, 1));
    }
}
