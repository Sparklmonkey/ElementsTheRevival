using System.Collections.Generic;
using UnityEngine;

public class Shockwave : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard.Counters.Freeze > 0)
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
            return;
        }

        targetCard.DefDamage += 4;
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(4, true, false, targetId.owner.Not()));
        }

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }
    
                
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.CreatureAndPlayer, -5, 0, 0);
    }
}