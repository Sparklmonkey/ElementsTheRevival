public struct PlayCardOnFieldEvent : IEvent
{
    public Card CardToPlay;
    public bool IsPlayer;
    
    public PlayCardOnFieldEvent(Card cardToPlay, bool isPlayer)
    {
        CardToPlay = cardToPlay;
        IsPlayer = isPlayer;
    }
}