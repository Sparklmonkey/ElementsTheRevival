public struct OnTurnEndEvent : IEvent
{
    public CardType CardType;
    public OwnerEnum Owner;
    public OnTurnEndEvent(CardType cardType, OwnerEnum owner)
    {
        CardType = cardType;
        Owner = owner;
    }
}