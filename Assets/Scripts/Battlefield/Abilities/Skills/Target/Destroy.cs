using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Destroy : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        if (id.IsPermanentField() && card.IsTargetable())
        {
            return true;
        }
        
        if (card.iD is "4t1" or "4t2") return false;
        if (card.cardType.Equals(CardType.Mark)) return false;
        return id.field.Equals(FieldEnum.Passive) && card.cardType is CardType.Shield or CardType.Weapon && card.IsTargetable();
    }
}