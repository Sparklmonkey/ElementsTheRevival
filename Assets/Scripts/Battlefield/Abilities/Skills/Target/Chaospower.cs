using System.Collections.Generic;
using UnityEngine;

public class Chaospower : ActivatedAbility
{
    public override bool NeedsTarget() => true;
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.AtkModify += Random.Range(1, 6);
        targetCard.DefModify += Random.Range(1, 6);
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}