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

        return possibleTargets.FindAll(x => x.card.IsTargetable() && x.card.AtkNow < 4);
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}