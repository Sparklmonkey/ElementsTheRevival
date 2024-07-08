namespace Battlefield.Abilities
{
    public class UnholySkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (!cardPair.card.Type.Equals(CardType.Creature)) return atkNow;
            var activation = UnityEngine.Random.Range(0, 100);
            
            var creatureChance = 50 / cardPair.card.DefNow;
            if (activation > creatureChance) return atkNow;
            
            var isUpgraded = cardPair.card.Id.IsUpgraded();
            EventBus<OnDeathTriggerEvent>.Raise(new OnDeathTriggerEvent());
            var card = CardDatabase.Instance.GetCardFromId(isUpgraded ? "716" : "52m");
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, card, false));

            return atkNow;
        }
    }
}