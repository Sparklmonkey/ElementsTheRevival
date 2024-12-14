using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Catapult : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var damage = 100 * targetCard.DefNow / (100 + targetCard.DefNow);
        damage += targetCard.Counters.Freeze > 0 ? Mathf.FloorToInt(damage * 0.5f) : 0;

        if (targetCard.Counters.Poison > 0)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, targetId.owner.Not(), targetCard.Counters.Poison));
        }
        
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damage, true, false, targetId.owner.Not()));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner) && card.IsTargetable(id);
    }

    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Trebuchet, 20, 0, 0);
    }
}