public struct QuantaChangeVisualEvent : IEvent
{
    public int Amount;
    public Element Element;
    public bool IsPlayer;
    
    public QuantaChangeVisualEvent(int amount, Element element, bool isPlayer)
    {
        Amount = amount;
        Element = element;
        IsPlayer = isPlayer;
    }
}