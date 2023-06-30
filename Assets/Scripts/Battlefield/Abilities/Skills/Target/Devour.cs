using System.Collections.Generic;
using System.Linq;

public class Devour : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        BattleVars.shared.abilityOrigin.card.AtkModify++;
        BattleVars.shared.abilityOrigin.card.DefModify++;
        if (target.card.innate.Contains("poisonous"))
        {
            BattleVars.shared.abilityOrigin.card.Poison++;
        }
        BattleVars.shared.abilityOrigin.UpdateCard();
        target.RemoveCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        if (!possibleTargets.Exists(x => x.IsTargetable() && x.card.DefNow <= Origin.card.DefNow)) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable() && x.card.DefNow < Origin.card.DefNow);
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets.Aggregate((i1, i2) => i1.card.AtkNow > i2.card.AtkNow ? i1 : i2);
    }
}
