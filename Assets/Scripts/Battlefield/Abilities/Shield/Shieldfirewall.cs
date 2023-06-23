public class Shieldfirewall : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        cardPair.card.DefDamage++;
    }
}
