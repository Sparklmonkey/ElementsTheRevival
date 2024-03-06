using System.Collections.Generic;
using UnityEngine;

public class Guard : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Counters.Delay++;
        BattleVars.Shared.AbilityCardOrigin.Counters.Delay++;
        if (!targetCard.innateSkills.Airborne)
        {
            targetCard.DefDamage += BattleVars.Shared.AbilityCardOrigin.AtkNow;
        }
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin, true));
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Creature, -1, 0, 0);
    }
}