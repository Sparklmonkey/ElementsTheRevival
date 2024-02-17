public class ShieldUnholy : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        if (cardPair.card.Counters.Aflatoxin == 0 && UnityEngine.Random.Range(0f, 1f) <= 0.5f / cardPair.card.DefNow && atkNow > 0 &&
            cardPair.card.Type == CardType.Creature)
        {
            var isUpgraded = cardPair.card.Id.IsUpgraded();
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(cardPair.id));
            var card = CardDatabase.Instance.GetCardFromId(isUpgraded ? "716" : "52m");
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, card, false));
        }

        return atkNow;
    }
}
