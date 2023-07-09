using System.Collections.Generic;
using UnityEngine;

public class Butterfly : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.card.skill = "destroy";
        target.card.skillCost = 3;
        target.card.skillElement = Element.Entropy;
        target.card.desc = "<sprite=6><sprite=6><sprite=6>: Destroy: \n Destroy the targeted permanent";
        target.UpdateCard();
        return;
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable() && x.card.AtkNow < 4);
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