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