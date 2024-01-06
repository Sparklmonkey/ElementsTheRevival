using System.Collections.Generic;
using UnityEngine;

public class Purify : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.IsPoisoned;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (targetCard is null)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Purify, targetId.owner, 2));
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Sacrifice, targetId.owner, -9999));
            return;
        }

        targetCard.IsAflatoxin = false;
        targetCard.Poison = targetCard.Poison > 0 ? 0 : targetCard.Poison;

        targetCard.Poison -= 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        possibleTargets.Add((enemy.playerID, null));
        possibleTargets.Add((Owner.playerID, null));
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