namespace Battlefield.Abilities
{
    public class WeightSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (cardPair.card.cardType == CardType.Creature && cardPair.card.DefNow > 5)
            {
                return 0;
            }

            return atkNow;
        }
    }
}