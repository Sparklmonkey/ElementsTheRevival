using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lightning : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        if (!target.HasCard())
        {
            DuelManager.GetIDOwner(target.id).ModifyHealthLogic(5, true, true);
            return;
        }
        target.card.DefDamage += 5;
        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add(enemy.playerID);
        possibleTargets.Add(Owner.playerID);

        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        if (posibleTargets.Find(x => x.card != null) == null)
        {
            return posibleTargets.Find(x => x.id.Owner != Owner.playerID.id.Owner);
        }
        return posibleTargets.Aggregate((i1, i2) => i1.card.AtkNow > i2.card.AtkNow ? i1 : i2);
    }
}