public struct AddSpellActivatedActionEvent : IEvent
{
    public Card Spell;
    public bool IsPlayer;
    public ID TargetId;
    public Card TargetCard;
    
    public AddSpellActivatedActionEvent(bool isPlayer, Card spell,  ID targetId, Card targetCard)
    {
        Spell = spell;
        IsPlayer = isPlayer;
        TargetId = targetId;
        TargetCard = targetCard;
    }
}