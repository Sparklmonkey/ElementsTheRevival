using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Purify : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard is null)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Purify, targetId.owner, 2));
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Sacrifice, targetId.owner, -9999));
            return;
        }

        targetCard.IsAflatoxin = false;
        targetCard.Poison = targetCard.Poison > 0 ? 0 : targetCard.Poison;

        targetCard.Poison -= 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null)
        {
            return id.IsPlayerField();
        }
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}