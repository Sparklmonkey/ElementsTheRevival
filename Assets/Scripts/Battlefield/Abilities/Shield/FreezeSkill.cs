﻿namespace Battlefield.Abilities
{
    public class FreezeSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (cardPair.card.Type is CardType.Weapon) return atkNow;
            if (UnityEngine.Random.Range(0f, 1f) > 0.3f) return atkNow;
            cardPair.card.Counters.Freeze += 3;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
            return atkNow;
        }
    }
}