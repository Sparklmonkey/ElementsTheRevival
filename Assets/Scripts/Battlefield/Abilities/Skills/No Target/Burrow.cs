using System.Collections.Generic;
using System.Linq;

public class Burrow : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        if (target.card.passive.Contains("burrow"))
        {
            target.card.passive.Remove("burrow");
            target.card.atk *= 2;
            target.card.AtkModify *= 2;
        }
        else
        {
            target.card.passive.Add("burrow");
            target.card.atk /= 2;
            target.card.AtkModify /= 2;
        }
        target.UpdateCard();
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
