using System.Collections.Generic;

public class Blackhole : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var victim = Owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
        var hpToRestore = 0;
        if (victim.playerCounters.sanctuary == 0)
        {
            for (var i = 0; i < 12; i++)
            {
                if (victim.HasSufficientQuanta((Element)i, 3))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, (Element)i, Owner.isPlayer, false));
                    hpToRestore += 3;
                }
                else if (victim.HasSufficientQuanta((Element)i, 2))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, (Element)i, Owner.isPlayer, false));
                    hpToRestore += 2;
                }
                else if (victim.HasSufficientQuanta((Element)i, 1))
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, Owner.isPlayer, false));
                    hpToRestore++;
                }
            }
        }

        Owner.ModifyHealthLogic(hpToRestore, false, false);
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
