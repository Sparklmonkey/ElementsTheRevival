using System.Collections.Generic;
using UnityEngine;

public class Cremation : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.SelfLowAtk;

    public override void Activate(IDCardPair target)
    {
        EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(target.id));
        for (var i = 0; i < 12; i++)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, Owner.isPlayer, true));
        }

        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(7, Element.Fire, Owner.isPlayer, true));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return null; }
        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}