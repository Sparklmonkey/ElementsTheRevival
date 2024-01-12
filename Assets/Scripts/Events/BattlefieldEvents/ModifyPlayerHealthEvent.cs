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
    
    public SetupCardDisplayEvent(ID id, Card card, bool isPlayable)
    {
        Id = id;
        Card = card;
        IsPlayable = isPlayable;
    }
}