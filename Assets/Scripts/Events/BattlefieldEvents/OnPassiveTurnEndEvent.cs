public struct OnPassiveTurnEndEvent : IEvent
{
    public CardType CardType;
    public OwnerEnum Owner;
    
    public OnPassiveTurnEndEvent(OwnerEnum owner, CardType cardType)
    {
        CardType = cardType;
        Owner = owner;
    }
}