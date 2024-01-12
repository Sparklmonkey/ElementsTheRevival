public struct UpdateCreatureCardEvent : IEvent
{
    public ID Id;
    public Card Card;
    public bool IsUpdate;
    
    public UpdateCreatureCardEvent(ID id, Card card, bool isUpdate)
    {
        Id = id;
        Card = card;
        IsUpdate = isUpdate;
    }
}