using System.Collections.Generic;
using System.Linq;

public class Deadlypoison : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        DuelManager.GetNotIDOwner(target.id).AddPlayerCounter(PlayerCounters.Poison, 3);
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
