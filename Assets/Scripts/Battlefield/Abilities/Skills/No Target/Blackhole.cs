using System.Collections.Generic;

public class Blackhole : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var victim = DuelManager.Instance.GetNotIDOwner(Owner.playerID);
        var hpToRestore = 0;
        if (victim.playerCounters.sanctuary == 0)
        {
            for (var i = 0; i < 12; i++)
            {
                if (victim.HasSufficientQuanta((Element)i, 3))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, (Element)i, Owner.Owner, false));
                    hpToRestore += 3;
                }
                else if (victim.HasSufficientQuanta((Element)i, 2))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, (Element)i, Owner.Owner, false));
                    hpToRestore += 2;
                }
                else if (victim.HasSufficientQuanta((Element)i, 1))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, Owner.Owner, false));
                    hpToRestore++;
                }
            }
        }
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(hpToRestore, false, false, Owner.Owner));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
