public class HandBehaviour : CardTypeBehaviour
{
    protected override void OnCardPlay(OnCardPlayEvent onCardPlayEvent)
    {
        if (!onCardPlayEvent.IdPlayed.Equals(cardPair.id))
        {
            return;
        }
        cardPair.card = onCardPlayEvent.CardPlayed;
        cardPair.stackCount = 1;
        StackCount = cardPair.stackCount;
        EventBus<UpdateCardDisplayEvent>.Raise(new UpdateCardDisplayEvent(cardPair.id, cardPair.card, cardPair.stackCount, cardPair.isHidden));
    }

    protected override void OnCardRemove(OnCardRemovedEvent cardRemovedEvent)
    {
        if (!cardRemovedEvent.IdRemoved.Equals(cardPair.id))
        {
            return;
        }
        cardPair.isHidden = true;
        cardPair.stackCount = 0;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(cardPair.id, 0, cardPair.card));
        cardPair.card = null;
    }

    public override void OnTurnStart()
    {
        return;
    }

    protected override void DeathTrigger(OnDeathDTriggerEvent onDeathDTriggerEvent)
    {
        return;
    }

    protected override void OnTurnEnd(OnTurnEndEvent onTurnEndEvent)
    {
        return;
    }
}