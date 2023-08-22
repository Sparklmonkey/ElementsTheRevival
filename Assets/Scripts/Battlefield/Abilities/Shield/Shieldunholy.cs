public class Shieldunholy : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        if (!cardPair.card.IsAflatoxin && UnityEngine.Random.Range(0f, 1f) <= (0.5f / cardPair.card.DefNow) && atkNow > 0 &&
            cardPair.card.cardType == CardType.Creature)
        {
            bool isUpgraded = cardPair.card.iD.IsUpgraded();
            cardPair.RemoveCard();
            cardPair.PlayCard(CardDatabase.Instance.GetCardFromId(isUpgraded ? "716" : "52m"));
        }
    }
}
