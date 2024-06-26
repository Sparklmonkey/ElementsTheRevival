﻿using System.Collections.Generic;
using UnityEngine;

public class Devour : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        BattleVars.Shared.AbilityCardOrigin.AtkModify++;
        BattleVars.Shared.AbilityCardOrigin.DefModify++;
        if (targetCard.innateSkills.Poisonous)
        {
            BattleVars.Shared.AbilityCardOrigin.Counters.Poison++;
        }
        EventBus<UpdateCreatureCardEvent>.Raise( new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.DefNow < BattleVars.Shared.AbilityCardOrigin.DefNow && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Smaller, -1, 0, 0);
    }
}
