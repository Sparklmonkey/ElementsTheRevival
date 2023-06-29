public class ShieldshieldTwo : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        atkNow -= 2;
        if (atkNow < 0) { atkNow = 0; }
    }
}
