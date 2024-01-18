public struct CardTappedEvent : IEvent
{
    public ID TappedId;
    public Card TappedCard;
    
    public CardTappedEvent(ID tappedId, Card tappedCard)
    {
        TappedId = tappedId;
        TappedCard = tappedCard;
    }
}public struct AddTargetToListEvent : IEvent
{
    public ID TargetId;
    public Card TargetCard;
    
    public AddTargetToListEvent(ID tappedId, Card tappedCard)
    {
        TargetId = tappedId;
        TargetCard = tappedCard;
    }
}