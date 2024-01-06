public class Shieldtwo : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        return atkNow <= 2 ? 0 : atkNow - 2;
    }
}
