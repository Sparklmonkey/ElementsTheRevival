using Elements.Duel.Manager;

public struct ShouldShowUsableEvent : IEvent
{
    public QuantaCheck QuantaCheck;
    public OwnerEnum Owner;
    
    public ShouldShowUsableEvent(QuantaCheck quantaCheck, OwnerEnum owner)
    {
        QuantaCheck = quantaCheck;
        Owner = owner;
    }
}

public struct HideUsableDisplayEvent : IEvent
{
}