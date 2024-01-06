public struct UpdateCreatureCardEvent : IEvent
{
    public ID Id;
    public Card Card;
    
    public UpdateCreatureCardEvent(ID id, Card card)
    {
        Id = id;
        Card = card;
    }
}