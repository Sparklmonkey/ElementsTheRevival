using System.Collections.Generic;
using UnityEngine;

public class Overdrive : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.desc = "Overdrive: \n Gain +3 /-1 per turn";
        targetCard.skill = "";
        targetCard.passiveSkills.Overdrive = true;
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}