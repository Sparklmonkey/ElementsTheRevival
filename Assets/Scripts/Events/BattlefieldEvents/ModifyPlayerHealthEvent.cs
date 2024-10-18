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

public struct SetupCardDisplayEvent : IEvent
{
    public ID Id;
    public Card Card;
    public bool IsPlayable;
    public bool IsAbilityUsable;
    
    public SetupCardDisplayEvent(ID id, Card card, bool isPlayable, bool isAbilityUsable)
    {
        Id = id;
        Card = card;
        IsPlayable = isPlayable;
        IsAbilityUsable = isAbilityUsable;
    }
}


public struct DisplayCardToolTipEvent : IEvent
{
    public ID Id;
    public Card Card;
    public bool IsHide;
    
    public DisplayCardToolTipEvent(ID id, Card card, bool isHide)
    {
        Id = id;
        Card = card;
        IsHide = isHide;
    }
}