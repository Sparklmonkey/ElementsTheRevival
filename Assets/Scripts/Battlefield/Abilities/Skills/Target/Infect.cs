﻿using System.Collections.Generic;
using System.Linq;

public class Infect : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Counters.Poison += 1;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        if (targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, targetId.owner.Not(), 1));
        }

        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(BattleVars.Shared.AbilityIDOrigin));
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable(id);
    }

    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Creature, -1, 0, 0);
    }
}