﻿using System.Collections.Generic;
using Battlefield.Abilities;

public class Acceleration : ActivatedAbility
{
    public override bool NeedsTarget() => true;
    
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Desc = "Acceleration: \n Gain +2 /-1 per turn";
        targetCard.TurnEndAbility = new AccelerateEndTurn();
        targetCard.Skill = null;
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