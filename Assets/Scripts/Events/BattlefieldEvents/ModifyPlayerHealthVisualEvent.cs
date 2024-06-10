public struct ModifyPlayerHealthVisualEvent : IEvent
{
    public int CurrentHp;
    public int MaxHp;
    public OwnerEnum Owner;
    
    public ModifyPlayerHealthVisualEvent(int currentHp, OwnerEnum owner, int maxHp)
    {
        CurrentHp = currentHp;
        Owner = owner;
        MaxHp = maxHp;
    }
}

public struct UpdatePossibleDamageEvent : IEvent
{
    
}