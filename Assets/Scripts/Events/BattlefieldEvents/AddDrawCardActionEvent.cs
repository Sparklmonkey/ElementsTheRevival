public struct AddDrawCardActionEvent : IEvent
{
    public Card CardDrawn;
    public bool IsPlayer;
    
    public AddDrawCardActionEvent(Card cardDrawn, bool isPlayer)
    {
        CardDrawn = cardDrawn;
        IsPlayer = isPlayer;
    }
}

public struct OnTurnStartEvent : IEvent
{
    public bool IsPlayerTurn;
    public OnTurnStartEvent(bool isPlayerTurn)
    {
        IsPlayerTurn = isPlayerTurn;
    }
}
public struct OnCardPlayEvent : IEvent
{
    public ID IdPlayed;
    public Card CardPlayed;
    public OnCardPlayEvent(ID idPlayed, Card cardPlayed)
    {
        IdPlayed = idPlayed;
        CardPlayed = cardPlayed;
    }
}
public struct OnCardRemovedEvent : IEvent
{
    public ID IdRemoved;
    public OnCardRemovedEvent(ID idRemoved)
    {
        IdRemoved = idRemoved;
    }
}