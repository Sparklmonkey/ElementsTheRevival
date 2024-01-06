using System.Collections.Generic;
using UnityEngine;

public class Immolate : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.SelfLowAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        for (var i = 0; i < 12; i++)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, Owner.Owner, true));
        }

        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(5, Element.Fire, Owner.Owner, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        return possibleTargets.Count == 0 ? new() : possibleTargets.FindAll(x => x.IsTargetable());
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