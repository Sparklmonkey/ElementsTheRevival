using System.Collections.Generic;
using UnityEngine;

public class Web : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "Web", Element.Other));
        targetCard.innateSkills.Airborne = false;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable() && card.innateSkills.Airborne;
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Creature, -1, 0, 0);
    }
}