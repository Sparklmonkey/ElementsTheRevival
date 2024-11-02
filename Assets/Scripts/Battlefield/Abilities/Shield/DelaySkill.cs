namespace Battlefield.Abilities
{
    public class DelaySkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (cardPair.card.Type is CardType.Weapon) return atkNow;
            cardPair.card.Counters.Delay += 2;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
            return atkNow;
        }
    }
}