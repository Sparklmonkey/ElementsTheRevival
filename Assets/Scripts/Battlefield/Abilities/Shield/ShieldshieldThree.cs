public class ShieldshieldThree : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        atkNow -= 3;
        if (atkNow < 0) { atkNow = 0; }
    }
}