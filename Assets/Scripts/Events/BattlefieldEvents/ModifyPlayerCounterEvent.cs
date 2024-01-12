public struct ModifyPlayerCounterEvent : IEvent
{
    public PlayerCounters Counter;
    public int Amount;
    public OwnerEnum Owner;
    
    public ModifyPlayerCounterEvent(PlayerCounters counter, OwnerEnum owner, int amount)
    {
        Counter = counter;
        Owner = owner;
        Amount = amount;
    }
}