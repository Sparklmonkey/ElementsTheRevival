public class Shieldwings : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (!cardPair.card.innateSkills.Ranged && !cardPair.card.innateSkills.Airborne)
        {
            atkNow = 0;
        }
    }
}
