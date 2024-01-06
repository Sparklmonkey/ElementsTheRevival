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

public struct UpdateHandDisplayEvent : IEvent
{
    public OwnerEnum Owner;

    public UpdateHandDisplayEvent(OwnerEnum owner)
    {
        Owner = owner;
    }
}
