using System.Collections.Generic;

public class Fractal : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        DuelManager.Instance.GetIDOwner(BattleVars.Shared.AbilityIDOrigin).FillHandWith(CardDatabase.Instance.GetCardFromId(targetCard.iD));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Aether, BattleVars.Shared.AbilityIDOrigin.owner, false));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}