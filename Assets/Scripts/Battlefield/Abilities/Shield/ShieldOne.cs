public class ShieldOne : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        return atkNow <= 1 ? 0 : atkNow - 1;
    }
}
