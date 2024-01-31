using System.Collections.Generic;
using UnityEngine;

public class Butterfly : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.skill = "destroy";
        targetCard.skillCost = 3;
        targetCard.skillElement = Element.Entropy;
        targetCard.desc = "<sprite=6><sprite=6><sprite=6>: Destroy: \n Destroy the targeted permanent";
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.AtkNow < 4 && card.IsTargetable();
    }
}