public struct CardTappedEvent : IEvent
{
    public ID TappedId;
    public Card TappedCard;
    
    public CardTappedEvent(ID tappedId, Card tappedCard)
    {
        TappedId = tappedId;
        TappedCard = tappedCard;
    }
}