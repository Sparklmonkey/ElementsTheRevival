public struct QuantaChangeLogicEvent : IEvent
{
    public int Amount;
    public Element Element;
    public OwnerEnum Owner;
    public bool IsAdd;
    
    public QuantaChangeLogicEvent(int amount, Element element, OwnerEnum owner, bool isAdd)
    {
        Amount = amount;
        Element = element;
        Owner = owner;
        IsAdd = isAdd;
    }
}