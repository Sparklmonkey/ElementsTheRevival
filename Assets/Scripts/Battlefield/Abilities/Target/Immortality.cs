using System.Collections.Generic;
using System.Linq;

public class Immortality : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.card.passive.Add("immaterial");
        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        return possibleTargets.FindAll(x => x.card.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets.Aggregate((i1, i2) => i1.card.AtkNow > i2.card.AtkNow ? i1 : i2);
    }
}
