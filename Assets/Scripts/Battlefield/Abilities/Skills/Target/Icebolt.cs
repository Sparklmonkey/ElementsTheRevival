using System.Collections.Generic;
using UnityEngine;

public class Icebolt : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        int quantaElement = Owner.GetAllQuantaOfElement(Element.Water);
        int damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);
        bool willFreeze = Random.Range(0, 100) > 30 + (damageToDeal * 5);

        if (!target.HasCard())
        {
            DuelManager.GetIDOwner(target.id).ModifyHealthLogic(damageToDeal, true, true);
            DuelManager.GetIDOwner(target.id).AddPlayerCounter(PlayerCounters.Freeze, willFreeze ? 3 : 0);
            return;
        }
        target.card.DefDamage += damageToDeal;
        target.card.Freeze += willFreeze ? 3 : 0;
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