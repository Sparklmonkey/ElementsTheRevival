public struct OnPermanentTurnEndEvent : IEvent
{
    public CardType CardType;
    public OwnerEnum Owner;
    
    public OnPermanentTurnEndEvent(OwnerEnum owner, CardType cardType)
    {
        Owner = owner;
        CardType = cardType;
    }
}