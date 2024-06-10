using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Holylight : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(10, false, false, targetId.owner));
            return;
        }

        var damage = targetCard.CostElement.Equals(Element.Death) || targetCard.CostElement.Equals(Element.Darkness)
            ? -10
            : 10;
        targetCard.DefDamage -= damage;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null)
        {
            return id.IsPlayerField();
        }
        return card.Type.Equals(CardType.Creature) && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.CreatureAndPlayer, 10, 0, 0);
    }
}