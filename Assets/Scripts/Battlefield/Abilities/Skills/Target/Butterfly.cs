using System.Collections.Generic;
using UnityEngine;

public class Butterfly : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Skill = new Destroy();
        targetCard.SkillCost = 3;
        targetCard.SkillElement = Element.Entropy;
        targetCard.Desc = "<sprite=6><sprite=6><sprite=6>: Destroy: \n Destroy the targeted permanent";
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.AtkNow < 3 && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.CreatureLowAtk, 1, 3, 0);
    }
}