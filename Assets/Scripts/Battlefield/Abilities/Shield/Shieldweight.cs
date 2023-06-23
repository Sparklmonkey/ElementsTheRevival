public class Shieldweight : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (cardPair.card.cardType == CardType.Creature && cardPair.card.DefNow > 5)
        {
            atkNow = 0;
        }
    }
}
