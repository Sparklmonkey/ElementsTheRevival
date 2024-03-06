namespace Battlefield.Abilities
{
    public class WingsSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            return cardPair.card.innateSkills is { Ranged: false, Airborne: false } ? 0 : atkNow;
        }
    }
}