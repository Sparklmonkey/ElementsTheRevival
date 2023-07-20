using System.Collections.Generic;
using System.Linq;
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
        if (target.card.DefNow > 0 && target.card.innateSkills.Voodoo)
        {
            Owner.ModifyHealthLogic(target.card.DefNow < damageToDeal ? target.card.DefNow : damageToDeal, true, false);
            Owner.AddPlayerCounter(PlayerCounters.Freeze, target.card.Freeze += willFreeze ? 3 : 0);
        }
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

        var opCreatures = posibleTargets.FindAll(x => x.id.Owner == OwnerEnum.Player && x.HasCard());
        if (opCreatures.Count == 0)
        {
            return posibleTargets.Find(x => x.id.Owner == OwnerEnum.Player);
        }
        else
        {
            return opCreatures.Aggregate((i1, i2) => i1.card.AtkNow >= i2.card.AtkNow ? i1 : i2);
        }
    }
}