using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Cremation : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        for (var i = 0; i < 12; i++)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, BattleVars.Shared.AbilityIDOrigin.owner, true));
        }

        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(7, Element.Fire, BattleVars.Shared.AbilityIDOrigin.owner, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner) && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(true, false, false, TargetType.BetaCreature, 1, 0, 0);
    }
}