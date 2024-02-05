using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Tsunami : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        for (int i = 0; i < 3; i++)
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        }
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return id.IsPermanentField() && card.IsTargetable();
    }
}