public class Shieldhope : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        var damageReduction = Owner.GetLightEmittingCreatures();

        atkNow -= damageReduction;
        if (atkNow < 0) { atkNow = 0; }

        return atkNow;
    }
}
