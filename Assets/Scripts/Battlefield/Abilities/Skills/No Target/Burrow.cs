using System.Collections.Generic;
using System.Linq;

public class Burrow : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        if (target.card.passiveSkills.Burrow)
        {
            target.card.passiveSkills.Burrow = false;
            target.card.atk *= 2;
            target.card.AtkModify *= 2;
        }
        else
        {
            target.card.passiveSkills.Burrow = true;
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
