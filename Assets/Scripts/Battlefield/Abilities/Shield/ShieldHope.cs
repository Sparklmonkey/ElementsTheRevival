public class ShieldHope : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        var damageReduction = DuelManager.Instance.GetNotIDOwner(cardPair.id).GetLightEmittingCreatures();

        atkNow -= damageReduction;
        if (atkNow < 0) { atkNow = 0; }

        return atkNow;
    }
}
