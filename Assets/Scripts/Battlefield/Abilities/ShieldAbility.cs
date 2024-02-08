public abstract class ShieldAbility
{
    public virtual int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        return atkNow;
    }
}
