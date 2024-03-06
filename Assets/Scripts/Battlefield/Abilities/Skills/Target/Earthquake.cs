using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Earthquake : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        for (var i = 0; i < 3; i++)
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        }
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Pillar) && id.IsPermanentField() && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Pillar, -1, 0, 0);
    }
}