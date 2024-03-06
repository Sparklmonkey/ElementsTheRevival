public struct UpdatePassiveDisplayEvent : IEvent
{
    public ID Id;
    public Card Card;
    public bool IsUpdate;
    
    public UpdatePassiveDisplayEvent(ID id, Card card, bool isUpdate)
    {
        Id = id;
        Card = card;
        IsUpdate = isUpdate;
    }
}

public struct UpdatePrecogEvent : IEvent
{
    public ID Id;
    
    public UpdatePrecogEvent(ID id)
    {
        Id = id;
    }
}