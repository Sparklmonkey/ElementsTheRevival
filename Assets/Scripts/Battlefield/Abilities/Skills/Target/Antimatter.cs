using System.Collections.Generic;
using UnityEngine;

public class Antimatter : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.passiveSkills.Antimatter = true;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }

    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.DefineAtk, -1, 25, 25);
    }
}