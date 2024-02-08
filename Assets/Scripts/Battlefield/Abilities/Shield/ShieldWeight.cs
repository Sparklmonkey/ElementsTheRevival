public class ShieldWeight : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        if (cardPair.card.cardType == CardType.Creature && cardPair.card.DefNow > 5)
        {
            return 0;
        }

        return atkNow;
    }
}
