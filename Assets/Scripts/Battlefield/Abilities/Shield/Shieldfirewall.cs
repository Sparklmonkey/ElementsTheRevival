public class Shieldfirewall : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        cardPair.card.DefDamage++;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card));
        return atkNow;
    }
}
