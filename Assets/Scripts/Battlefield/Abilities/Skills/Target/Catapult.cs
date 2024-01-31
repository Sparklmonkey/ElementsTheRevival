using System.Collections.Generic;
using UnityEngine;

public class Catapult : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var damage = 100 * targetCard.DefNow / (100 + targetCard.DefNow);
        damage += targetCard.Freeze > 0 ? Mathf.FloorToInt(damage * 0.5f) : 0;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damage, true, false, targetId.owner.Not()));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && id.owner.Equals(BattleVars.Shared.AbilityIDOrigin.owner) && card.IsTargetable();
    }

}