public class Shielddelay : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        cardPair.card.innate.Add("delay");
    }
}
