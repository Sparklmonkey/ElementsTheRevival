public struct AddCardToHandEvent : IEvent
{
    public bool IsPlayer;
    public Card CardToAdd;
    
    public AddCardToHandEvent(bool isPlayer, Card cardToAdd)
    {
        IsPlayer = isPlayer;
        CardToAdd = cardToAdd;
    }
}