public struct PlayPermanentOnFieldEvent : IEvent
{
    public OwnerEnum Owner;
    public readonly Card CardToPlay;
    
    public PlayPermanentOnFieldEvent(OwnerEnum owner, Card cardToPlay)
    {
        Owner = owner;
        CardToPlay = cardToPlay;
    }
}