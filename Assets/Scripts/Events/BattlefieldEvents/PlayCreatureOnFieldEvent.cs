public struct PlayCreatureOnFieldEvent : IEvent
{
    public OwnerEnum Owner;
    public readonly Card CardToPlay;
    
    public PlayCreatureOnFieldEvent(OwnerEnum owner, Card cardToPlay)
    {
        Owner = owner;
        CardToPlay = cardToPlay;
    }
}