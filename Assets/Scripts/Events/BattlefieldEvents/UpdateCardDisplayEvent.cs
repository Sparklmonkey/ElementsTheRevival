public struct UpdateCardDisplayEvent : IEvent
{
    public ID Id;
    public int Stack;
    public Card Card;
    public bool IsHidden;
    
    public UpdateCardDisplayEvent(ID id, Card card, int stack = 0, bool isHidden = true)
    {
        Stack = stack;
        Id = id;
        Card = card;
        IsHidden = isHidden;
    }
}

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