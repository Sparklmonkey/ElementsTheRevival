using Elements.Duel.Manager;

public struct ShouldShowUsableEvent : IEvent
{
    public QuantaCheck QuantaCheck;
    public OwnerEnum Owner;
    public int HandCount;
    
    public ShouldShowUsableEvent(QuantaCheck quantaCheck, OwnerEnum owner, int handCount)
    {
        QuantaCheck = quantaCheck;
        Owner = owner;
        HandCount = handCount;
    }
}

public struct HideUsableDisplayEvent : IEvent
{
}

public struct ActivateAbilityEffectEvent : IEvent
{
    public ActivateAbilityEffect ActivateAbilityEffect;
    public ID TargetId;
    public ActivateAbilityEffectEvent(ActivateAbilityEffect activateAbilityEffect, ID targetId)
    {
        ActivateAbilityEffect = activateAbilityEffect;
        TargetId = targetId;
    }
}