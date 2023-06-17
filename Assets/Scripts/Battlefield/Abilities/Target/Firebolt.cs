using System.Collections.Generic;
using UnityEngine;

public class Firebolt : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        int quantaElement = Owner.GetAllQuantaOfElement(Element.Fire);
        int damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);

        if (!target.HasCard())
        {
            DuelManager.GetIDOwner(target.id).ModifyHealthLogic(damageToDeal, true, true);
            return;
        }
        target.card.DefDamage += damageToDeal;
        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add(enemy.playerID);
        possibleTargets.Add(Owner.playerID);
        return possibleTargets.FindAll(x => x.card.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}