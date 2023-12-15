public struct QuantaChangeLogicEvent : IEvent
{
    public int Amount;
    public Element Element;
    public bool IsPlayer;
    public bool IsAdd;
    
    public QuantaChangeLogicEvent(int amount, Element element, bool isPlayer, bool isAdd)
    {
        Amount = amount;
        Element = element;
        IsPlayer = isPlayer;
        IsAdd = isAdd;
    }
}