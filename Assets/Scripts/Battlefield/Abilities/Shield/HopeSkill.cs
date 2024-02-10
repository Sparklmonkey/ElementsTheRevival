namespace Battlefield.Abilities
{
    public class HopeSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            var damageReduction = DuelManager.Instance.GetNotIDOwner(cardPair.id).GetLightEmittingCreatures();

            atkNow -= damageReduction;
            if (atkNow < 0) { atkNow = 0; }

            return atkNow;
        }
    }
}