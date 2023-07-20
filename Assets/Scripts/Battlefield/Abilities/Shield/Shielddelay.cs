public class Shielddelay : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        cardPair.card.innateSkills.Delay += 1;
    }
}
