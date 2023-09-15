using System.Collections.Generic;

public class Sacrifice : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.ModifyHealthLogic(40, true, false);
        for (int i = 0; i < 12; i++)
        {
            if ((Element)i == Element.Death) { continue; }
            Owner.SpendQuantaLogic((Element)i, 75);
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
