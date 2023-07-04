public class Shieldhope : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        int damageReduction = Owner.GetLightEmittingCreatures();

        atkNow -= damageReduction;
        if (atkNow < 0) { atkNow = 0; }
    }
}
