public struct AddCardToHandEvent : IEvent
{
    public OwnerEnum Owner;
    public readonly Card CardToAdd;
    
    public AddCardToHandEvent(OwnerEnum owner, Card cardToAdd)
    {
        Owner = owner;
        CardToAdd = cardToAdd;
    }
}