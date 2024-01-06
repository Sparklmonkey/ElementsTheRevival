public class Shieldwings : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        return cardPair.card.innateSkills is { Ranged: false, Airborne: false } ? 0 : atkNow;
    }
}
