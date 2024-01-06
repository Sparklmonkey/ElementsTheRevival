public struct DrawCardFromDeckEvent : IEvent
{
    public OwnerEnum Owner;
    
    public DrawCardFromDeckEvent(OwnerEnum owner)
    {
        Owner = owner;
    }
}