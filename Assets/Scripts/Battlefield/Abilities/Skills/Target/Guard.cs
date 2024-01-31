using System.Collections.Generic;
using UnityEngine;

public class Guard : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.innateSkills.Delay++;
        BattleVars.Shared.AbilityCardOrigin.innateSkills.Delay++;
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
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}