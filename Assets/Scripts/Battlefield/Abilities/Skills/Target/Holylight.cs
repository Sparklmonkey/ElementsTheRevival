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

        var damage = targetCard.costElement.Equals(Element.Death) || targetCard.costElement.Equals(Element.Darkness)
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
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}