using System.Collections.Generic;
using UnityEngine;

public class Tsunami : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Pillar;

    public override void Activate(ID targetId, Card targetCard)
    {
        for (int i = 0; i < 3; i++)
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerPermanentManager.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}