public struct AddSpellActivatedActionEvent : IEvent
{
    public Card Spell;
    public bool IsPlayer;
    public IDCardPair Target;
    
    public AddSpellActivatedActionEvent(bool isPlayer, Card spell, IDCardPair target)
    {
        Spell = spell;
        IsPlayer = isPlayer;
        Target = target;
    }
}