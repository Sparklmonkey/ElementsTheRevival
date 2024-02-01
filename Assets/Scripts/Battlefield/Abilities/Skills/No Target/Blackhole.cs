using System.Collections.Generic;

public class Blackhole : ActivatedAbility
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
        var victim = DuelManager.Instance.GetNotIDOwner(targetId);
        var hpToRestore = 0;
        if (victim.playerCounters.sanctuary == 0)
        {
            for (var i = 0; i < 12; i++)
            {
                if (victim.HasSufficientQuanta((Element)i, 3))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, (Element)i, victim.Owner, false));
                    hpToRestore += 3;
                }
                else if (victim.HasSufficientQuanta((Element)i, 2))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, (Element)i, victim.Owner, false));
                    hpToRestore += 2;
                }
                else if (victim.HasSufficientQuanta((Element)i, 1))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, victim.Owner, false));
                    hpToRestore++;
                }
            }
        }
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(hpToRestore, false, false, targetId.owner));
    }
}
