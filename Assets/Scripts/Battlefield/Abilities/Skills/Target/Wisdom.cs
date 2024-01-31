using System.Collections.Generic;
using UnityEngine;

public class Wisdom : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.AtkModify += 4;
        targetCard.passiveSkills.Psion = true;
        targetCard.desc = $"{targetCard.cardName}'s attacks deal spell damage.";
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.innateSkills.Immaterial;
    }
}