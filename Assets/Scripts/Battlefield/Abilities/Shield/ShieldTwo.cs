public class ShieldTwo : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        return atkNow <= 2 ? 0 : atkNow - 2;
    }
}
