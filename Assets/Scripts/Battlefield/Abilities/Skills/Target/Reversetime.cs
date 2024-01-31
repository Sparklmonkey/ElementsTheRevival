using System.Collections.Generic;
using UnityEngine;

public class Reversetime : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard.innateSkills.Mummy)
        {
            var card = CardDatabase.Instance.GetCardFromId(targetCard.iD.IsUpgraded() ? "7qc" : "5rs");
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, card, true));
        }
        else if (targetCard.innateSkills.Undead)
        {
            var card = CardDatabase.Instance.GetRandomCard(CardType.Creature, targetCard.iD.IsUpgraded(), true);
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, card, true));
        }
        else
        {
            var baseCreature = CardDatabase.Instance.GetCardFromId(targetCard.iD);
            DuelManager.Instance.GetIDOwner(targetId).AddCardToDeck(baseCreature);
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        }
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}