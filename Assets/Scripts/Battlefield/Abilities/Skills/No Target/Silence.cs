using System.Collections.Generic;
using System.Linq;

public class Silence : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.AddPlayerCounter(PlayerCounters.Silence, 1);
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}