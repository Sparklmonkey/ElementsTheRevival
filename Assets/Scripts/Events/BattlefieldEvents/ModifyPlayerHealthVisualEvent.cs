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

public struct ModifyPlayerHealthEvent : IEvent
{
    public OwnerEnum Target;
    public bool IsDamage;
    public int Amount;
    public bool FromSpell;
    public ModifyPlayerHealthEvent(int amount, bool isDamage, bool fromSpell, OwnerEnum target)
    {
        IsDamage = isDamage;
        FromSpell = fromSpell;
        Amount = amount;
        Target = target;
    }
}