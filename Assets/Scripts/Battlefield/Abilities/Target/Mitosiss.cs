using System.Collections.Generic;
using UnityEngine;

public class Mitosiss : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.card.skill = "mitosis";
        target.card.desc = "Mitosis: \n Generate a daughter creature";
        target.card.skillCost = target.card.cost;
        target.card.skillElement = target.card.costElement;
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        return possibleTargets.FindAll(x => x.card.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}