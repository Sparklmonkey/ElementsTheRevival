public struct UpdatePermanentCardEvent : IEvent
{
    public ID Id;
    public Card Card;
    
    public UpdatePermanentCardEvent(ID id, Card card)
    {
        Id = id;
        Card = card;
    }
}