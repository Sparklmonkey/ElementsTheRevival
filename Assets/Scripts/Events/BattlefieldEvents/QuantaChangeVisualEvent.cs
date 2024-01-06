public struct QuantaChangeVisualEvent : IEvent
{
    public int Amount;
    public Element Element;
    public OwnerEnum Owner;
    
    public QuantaChangeVisualEvent(int amount, Element element, OwnerEnum owner)
    {
        Amount = amount;
        Element = element;
        Owner = owner;
    }
}