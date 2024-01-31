using System.Collections.Generic;
using UnityEngine;

public class Mitosiss : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.skill = "mitosis";
        targetCard.desc = "Mitosis: \n Generate a daughter creature";
        targetCard.skillCost = targetCard.cost;
        targetCard.skillElement = targetCard.costElement;
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}