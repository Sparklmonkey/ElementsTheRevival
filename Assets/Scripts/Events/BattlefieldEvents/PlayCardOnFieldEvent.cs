public struct PlayCardOnFieldEvent : IEvent
{
    public Card CardToPlay;
    public OwnerEnum Owner;
    
    public PlayCardOnFieldEvent(Card cardToPlay, OwnerEnum owner)
    {
        CardToPlay = cardToPlay;
        Owner = owner;
    }
}