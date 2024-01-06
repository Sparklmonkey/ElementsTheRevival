public class Shielddelay : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        cardPair.card.innateSkills.Delay += 1;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card));
        return atkNow;
    }
}
