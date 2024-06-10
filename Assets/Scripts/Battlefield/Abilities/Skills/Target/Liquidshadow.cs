using System.Collections.Generic;
using UnityEngine;

public class Liquidshadow : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Skill = null;
        targetCard.Desc = "";
        targetCard.passiveSkills = new();
        targetCard.passiveSkills.Vampire = true;
        targetCard.Counters.Poison++;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.DefineDef, 1, 3, 0);
    }
}