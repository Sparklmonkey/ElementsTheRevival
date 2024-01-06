public class Shieldthree : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        return atkNow <= 3 ? 0 : atkNow - 3;
    }
}