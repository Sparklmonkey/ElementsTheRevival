public abstract class ShieldAbility 
{
    public PlayerManager Owner;
    public PlayerManager Enemy;
    public abstract void ActivateShield(ref int atkNow, ref IDCardPair cardPair);
}
