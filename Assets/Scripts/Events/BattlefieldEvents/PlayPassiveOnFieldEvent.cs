public struct PlayPassiveOnFieldEvent : IEvent
{
    public OwnerEnum Owner;
    public readonly Card CardToPlay;
    
    public PlayPassiveOnFieldEvent(OwnerEnum owner, Card cardToPlay)
    {
        Owner = owner;
        CardToPlay = cardToPlay;
    }
}