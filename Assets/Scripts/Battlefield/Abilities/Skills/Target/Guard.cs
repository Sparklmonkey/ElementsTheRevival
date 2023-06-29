using System.Collections.Generic;
using UnityEngine;

public class Guard : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.card.innate.Add("delay");
        BattleVars.shared.abilityOrigin.card.innate.Add("delay");
        if (!target.card.innate.Contains("airborne"))
        {
            target.card.DefDamage += BattleVars.shared.abilityOrigin.card.AtkNow;
        }
        BattleVars.shared.abilityOrigin.UpdateCard();
        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}