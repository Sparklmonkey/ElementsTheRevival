public struct AddAbilityActivatedActionEvent : IEvent
{
    public Card AbilityOwner;
    public bool IsPlayer;
    public ID TargetId;
    public Card TargetCard;
    
    public AddAbilityActivatedActionEvent(bool isPlayer, Card abilityOwner, ID targetId, Card targetCard)
    {
        AbilityOwner = abilityOwner;
        IsPlayer = isPlayer;
        TargetId = targetId;
        TargetCard = targetCard;
    }
}