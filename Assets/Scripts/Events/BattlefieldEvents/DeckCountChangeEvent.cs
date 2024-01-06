public struct DeckCountChangeEvent : IEvent
{
    public int DeckCount;
    public OwnerEnum Owner;
    
    public DeckCountChangeEvent(int deckCount, OwnerEnum owner)
    {
        DeckCount = deckCount;
        Owner = owner;
    }
}