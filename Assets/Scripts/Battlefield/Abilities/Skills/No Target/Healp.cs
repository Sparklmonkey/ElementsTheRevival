using System.Collections.Generic;

public class Healp : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card)
    {
        return id.Equals(
            new ID(BattleVars.Shared.AbilityIDOrigin.owner, FieldEnum.Player, 0));
    }
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(20, false, false, targetId.owner));
    }

}
