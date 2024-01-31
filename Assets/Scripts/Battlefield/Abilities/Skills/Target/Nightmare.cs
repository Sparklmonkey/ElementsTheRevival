using System.Collections.Generic;
using UnityEngine;

public class Nightmare : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var opponent = DuelManager.Instance.GetNotIDOwner(BattleVars.Shared.AbilityIDOrigin);
        var creature = CardDatabase.Instance.GetCardFromId(targetCard.iD);

        var damage = 7 - opponent.playerHand.GetHandCount();
        opponent.FillHandWith(creature);
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damage * 2, true, true, opponent.Owner));
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damage * 2, false, true, BattleVars.Shared.AbilityIDOrigin.owner));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}