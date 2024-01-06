public struct ModifyPlayerHealthLogicEvent : IEvent
{
    public int Amount;
    public OwnerEnum Owner;
    public bool IsMaxChange;
    
    public ModifyPlayerHealthLogicEvent(int amount, OwnerEnum owner, bool isMaxChange)
    {
        Amount = amount;
        Owner = owner;
        IsMaxChange = isMaxChange;
    }
}