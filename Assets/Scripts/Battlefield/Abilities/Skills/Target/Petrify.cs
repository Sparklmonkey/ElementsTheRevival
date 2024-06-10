using System.Collections.Generic;
using UnityEngine;

public class Petrify : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        for (var i = 0; i < 6; i++)
        {
            targetCard.Counters.Delay++;
        }

        targetCard.DefModify += 20;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        if (DuelManager.Instance.GetCardCount(new List<string> { "74h", "561" }) > 0)
        {
            return new AiTargetType(true, false, false, TargetType.BetaCreature, 1, 0, 0);
        }
        return new AiTargetType(false, false, false, TargetType.Creature, -1, 0, 0);
    }
}