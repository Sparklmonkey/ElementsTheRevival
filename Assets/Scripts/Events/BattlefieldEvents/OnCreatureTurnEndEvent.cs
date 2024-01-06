public struct OnCreatureTurnEndEvent : IEvent
{
    public OwnerEnum Owner;
    public OnCreatureTurnEndEvent(OwnerEnum owner)
    {
        Owner = owner;
    }
}