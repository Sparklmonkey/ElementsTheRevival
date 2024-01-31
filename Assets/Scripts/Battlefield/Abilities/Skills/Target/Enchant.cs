using System.Collections.Generic;
using UnityEngine;

public class Enchant : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.innateSkills.Immaterial = true;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        if (id.field.Equals(FieldEnum.Permanent))
        {
            return true;
        }
        
        return id.field.Equals(FieldEnum.Passive) && card.cardType is CardType.Shield or CardType.Weapon && card.IsTargetable();
    }
}
