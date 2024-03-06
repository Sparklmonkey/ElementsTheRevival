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
            var card = CardDatabase.Instance.GetCardFromId(targetCard.Id.IsUpgraded() ? "7qc" : "5rs");
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, card, true));
        }
        else if (targetCard.innateSkills.Undead)
        {
            var card = CardDatabase.Instance.GetRandomCard(CardType.Creature, targetCard.Id.IsUpgraded(), true);
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, card, true));
        }
        else
        {
            var baseCreature = CardDatabase.Instance.GetCardFromId(targetCard.Id);
            DuelManager.Instance.GetIDOwner(targetId).AddCardToDeck(baseCreature);
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        }
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }

    public override AiTargetType GetTargetType()
    {
        if (DuelManager.Instance.player.DeckManager.GetDeckCount() < 5)
        {
            return new AiTargetType(true, false, false, TargetType.BetaCreature, 0, 0, 0);
        }
        return new AiTargetType(false, false, false, TargetType.Creature, -1, 0, 0);
    }
}