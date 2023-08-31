using System.Collections.Generic;
using UnityEngine;

public class Blessing : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.card.AtkModify += 3;
        target.card.DefModify += 3;
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

        var opCreatures = posibleTargets.FindAll(x => x.id.Owner == OwnerEnum.Opponent && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return null;
        }
        else
        {
            return opCreatures[Random.Range(0, posibleTargets.Count)];
        }
    }
}