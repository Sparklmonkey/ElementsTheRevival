public struct AddCardPlayedOnFieldActionEvent : IEvent
{
    public Card CardToPlay;
    public bool IsPlayer;
    
    public AddCardPlayedOnFieldActionEvent(Card cardToPlay, bool isPlayer)
    {
        CardToPlay = cardToPlay;
        IsPlayer = isPlayer;
    }
}