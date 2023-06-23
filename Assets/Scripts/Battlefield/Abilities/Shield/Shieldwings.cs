public class Shieldwings : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (!cardPair.card.innate.Contains("airborne") && !cardPair.card.innate.Contains("ranged"))
        {
            atkNow = 0;
        }
    }
}
