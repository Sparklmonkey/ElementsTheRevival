using System.Collections.Generic;
using UnityEngine;

public class Purify : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.IsPoisoned;

    public override void Activate(IDCardPair target)
    {
        if (!target.HasCard())
        {
            DuelManager.Instance.GetIDOwner(target.id).sacrificeCount = 0;
            DuelManager.Instance.GetIDOwner(target.id).AddPlayerCounter(PlayerCounters.Purify, 2);
            return;
        }

        target.card.IsAflatoxin = false;
        target.card.Poison = target.card.Poison > 0 ? 0 : target.card.Poison;

        target.card.Poison -= 2;
        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add(enemy.playerID);
        possibleTargets.Add(Owner.playerID);
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return null;
        }

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}