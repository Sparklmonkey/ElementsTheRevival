public struct GameEndEvent : IEvent
{
    public OwnerEnum Owner;
    
    public GameEndEvent(OwnerEnum owner)
    {
        Owner = owner;
    }
}