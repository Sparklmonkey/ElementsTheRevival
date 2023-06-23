using System.Collections.Generic;
using System.Linq;

public class Lycanthropy : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        target.card.AtkModify += 5;
        target.card.DefModify += 5;
        target.card.skill = "";
        target.card.desc = "";
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
