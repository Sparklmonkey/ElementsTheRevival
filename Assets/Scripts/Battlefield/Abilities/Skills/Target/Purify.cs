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

        targetCard.Counters.Aflatoxin = 0;
        targetCard.Counters.Poison = targetCard.Counters.Poison > 0 ? 0 : targetCard.Counters.Poison;

        targetCard.Counters.Poison -= 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null)
        {
            return id.IsPlayerField();
        }
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.CreatureAndPlayer, 2, 0, 0);
    }
}