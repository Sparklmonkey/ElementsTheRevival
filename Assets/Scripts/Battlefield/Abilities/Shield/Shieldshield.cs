public class ShieldshieldOne : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        atkNow -= 1;
        if(atkNow < 0) { atkNow = 0; }
    }
}
