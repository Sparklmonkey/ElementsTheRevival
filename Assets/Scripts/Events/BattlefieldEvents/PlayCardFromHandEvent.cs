public struct PlayCardFromHandEvent : IEvent
{
    public Card CardToPlay;
    public ID Id;
    
    public PlayCardFromHandEvent(Card cardToPlay, ID id)
    {
        CardToPlay = cardToPlay;
        Id = id;
    }
}