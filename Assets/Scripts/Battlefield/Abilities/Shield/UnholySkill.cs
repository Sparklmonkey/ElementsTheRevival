﻿namespace Battlefield.Abilities
{
    public class UnholySkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (!cardPair.card.Type.Equals(CardType.Creature)) return atkNow;
            if (cardPair.card.Counters.Aflatoxin == 0) return atkNow;
            if (!(UnityEngine.Random.Range(0f, 1f) <= 0.5f / cardPair.card.DefNow)) return atkNow;
            
            var isUpgraded = cardPair.card.Id.IsUpgraded();
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(cardPair.id));
            var card = CardDatabase.Instance.GetCardFromId(isUpgraded ? "716" : "52m");
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, card, false));

            return atkNow;
        }
    }
}