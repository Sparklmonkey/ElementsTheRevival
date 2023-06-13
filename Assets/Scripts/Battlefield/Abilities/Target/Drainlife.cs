using System.Collections.Generic;
using UnityEngine;

public class Drainlife : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        int quantaElement = Owner.GetAllQuantaOfElement(Element.Darkness);
        int damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);

        if (target.card == null)
        {
            DuelManager.GetIDOwner(target.id).ModifyHealthLogic(damageToDeal, true, true);
            Owner.ModifyHealthLogic(damageToDeal, false, false);
            return;
        }
        target.card.DefDamage += damageToDeal;
        Owner.ModifyHealthLogic(target.card.DefNow < damageToDeal ? target.card.DefNow : damageToDeal, false, false);
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
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}