public struct OnTurnEndEvent : IEvent
{
    public CardType CardType;
    public bool IsPlayer;
    public OnTurnEndEvent(CardType cardType, bool isPlayer)
    {
        CardType = cardType;
        IsPlayer = isPlayer;
    }
}