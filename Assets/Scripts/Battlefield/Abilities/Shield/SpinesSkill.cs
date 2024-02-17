namespace Battlefield.Abilities
{
    public class SpinesSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (!(UnityEngine.Random.Range(0f, 1f) <= 0.75f)) return atkNow;
            cardPair.card.Counters.Poison++;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
            return atkNow;
        }
    }
}