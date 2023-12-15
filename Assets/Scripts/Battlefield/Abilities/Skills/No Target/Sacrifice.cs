using System.Collections.Generic;

public class Sacrifice : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.ModifyHealthLogic(40, true, false);
        for (var i = 0; i < 12; i++)
        {
            if ((Element)i == Element.Death) { continue; }
            
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, (Element)i, Owner.isPlayer, false));
        }
        Owner.AddPlayerCounter(PlayerCounters.Sacrifice, 2);
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
