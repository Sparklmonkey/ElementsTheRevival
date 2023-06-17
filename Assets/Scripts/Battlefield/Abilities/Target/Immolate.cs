using System.Collections.Generic;
using UnityEngine;

public class Immolate : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.RemoveCard();
        for (int i = 0; i < 12; i++)
        {
            Owner.GenerateQuantaLogic((Element)i, 1);
        }

        Owner.GenerateQuantaLogic(Element.Fire, 5);
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();

        return possibleTargets.FindAll(x => x.card.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}