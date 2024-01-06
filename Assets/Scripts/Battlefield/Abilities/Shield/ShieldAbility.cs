public abstract class ShieldAbility
{
    public PlayerManager Owner;
    public PlayerManager Enemy;

    public virtual int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        return atkNow;
    }
}
