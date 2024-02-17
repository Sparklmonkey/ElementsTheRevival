public class ShieldIceTwo : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        atkNow = atkNow > 2 ? atkNow - 2 : 0;
        cardPair.card.Counters.Freeze = UnityEngine.Random.Range(0f, 1f) <= 0.3f ? 3 : 0;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
        return atkNow;
    }
}