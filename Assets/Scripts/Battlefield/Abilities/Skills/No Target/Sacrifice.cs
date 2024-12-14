using System.Collections.Generic;

public class Sacrifice : ActivatedAbility
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
        var amount = BattleVars.Shared.AbilityCardOrigin.Id.IsUpgraded() ? 40 : 48;
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(amount, true, true, BattleVars.Shared.AbilityIDOrigin.owner));
        for (var i = 0; i < 12; i++)
        {
            if ((Element)i != Element.Death) { continue; }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, (Element)i, BattleVars.Shared.AbilityIDOrigin.owner, false));
        }
        EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Sacrifice, BattleVars.Shared.AbilityIDOrigin.owner, 2));
    }
}
