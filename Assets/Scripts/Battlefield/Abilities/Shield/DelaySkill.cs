namespace Battlefield.Abilities
{
    public class DelaySkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            cardPair.card.innateSkills.Delay += 1;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
            return atkNow;
        }
    }
}