public class ShieldSpines : ShieldAbility
{
    public override int ActivateShield(int atkNow, (ID id, Card card) cardPair)
    {
        if (UnityEngine.Random.Range(0f, 1f) <= 0.75f)
        {
            cardPair.card.Counters.Poison++;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(cardPair.id, cardPair.card, true));
        }
        
        return atkNow;
    }
}
