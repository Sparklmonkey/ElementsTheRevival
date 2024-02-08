public class Shieldiceone : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        atkNow = atkNow > 1 ? atkNow - 1 : 0;
        cardPair.card.Freeze = UnityEngine.Random.Range(0f, 1f) <= 0.3f ? 3 : 0;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
        return atkNow;
    }
}
