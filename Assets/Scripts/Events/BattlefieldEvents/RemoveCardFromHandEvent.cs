public struct RemoveCardFromHandEvent : IEvent
{
    public bool IsPlayer;
    public ID CardToRemove;
    
    public RemoveCardFromHandEvent(bool isPlayer, ID cardToRemove)
    {
        IsPlayer = isPlayer;
        CardToRemove = cardToRemove;
    }
}