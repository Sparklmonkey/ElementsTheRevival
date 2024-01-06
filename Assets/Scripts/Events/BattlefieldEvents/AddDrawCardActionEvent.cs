public struct AddDrawCardActionEvent : IEvent
{
    public Card CardDrawn;
    public OwnerEnum Owner;
    
    public AddDrawCardActionEvent(Card cardDrawn, OwnerEnum owner)
    {
        CardDrawn = cardDrawn;
        Owner = owner;
    }
}

public struct OnTurnStartEvent : IEvent
{
    public OwnerEnum Owner;
    public OnTurnStartEvent(OwnerEnum owner)
    {
        Owner = owner;
    }
}