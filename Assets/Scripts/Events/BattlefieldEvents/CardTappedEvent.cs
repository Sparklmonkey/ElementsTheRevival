public struct CardTappedEvent : IEvent
{
    public IDCardPair TappedPair;
    
    public CardTappedEvent(IDCardPair tappedPair)
    {
        TappedPair = tappedPair;
    }
}