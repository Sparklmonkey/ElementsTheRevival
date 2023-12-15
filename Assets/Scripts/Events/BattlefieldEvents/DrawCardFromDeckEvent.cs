public struct DrawCardFromDeckEvent : IEvent
{
    public bool IsPlayer;
    
    public DrawCardFromDeckEvent(bool isPlayer)
    {
        IsPlayer = isPlayer;
    }
}