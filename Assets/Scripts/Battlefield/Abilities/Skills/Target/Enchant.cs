using System.Collections.Generic;
using Core.Helpers;
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
        if (id.IsPermanentField())
        {
            return true;
        }
        
        return id.field.Equals(FieldEnum.Passive) && card.Type is CardType.Shield or CardType.Weapon && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Permanent, -1, 0, 0);
    }
}
