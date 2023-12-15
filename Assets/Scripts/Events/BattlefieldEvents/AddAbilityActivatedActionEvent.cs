public struct AddAbilityActivatedActionEvent : IEvent
{
    public Card AbilityOwner;
    public bool IsPlayer;
    public IDCardPair Target;
    
    public AddAbilityActivatedActionEvent(bool isPlayer, Card abilityOwner, IDCardPair target)
    {
        AbilityOwner = abilityOwner;
        IsPlayer = isPlayer;
        Target = target;
    }
}