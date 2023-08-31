using System.Collections.Generic;
using UnityEngine;

public class Holylight : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        if (!target.HasCard())
        {
            DuelManager.GetIDOwner(target.id).ModifyHealthLogic(10, false, false);
            return;
        }
        int damage = (target.card.costElement.Equals(Element.Death) || target.card.costElement.Equals(Element.Darkness)) ? -10 : 10;
        target.card.DefDamage -= damage;
        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add(enemy.playerID);
        possibleTargets.Add(Owner.playerID);
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}