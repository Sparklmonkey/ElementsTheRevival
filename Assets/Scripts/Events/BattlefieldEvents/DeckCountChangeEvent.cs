public struct DeckCountChangeEvent : IEvent
{
    public int DeckCount;
    public bool IsPlayer;
    
    public DeckCountChangeEvent(int deckCount,bool isPlayer)
    {
        DeckCount = deckCount;
        IsPlayer = isPlayer;
    }
}public struct GameEndEvent : IEvent
{
    public bool IsPlayer;
    
    public GameEndEvent(bool isPlayer)
    {
        IsPlayer = isPlayer;
    }
}