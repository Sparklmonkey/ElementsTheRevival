using JetBrains.Annotations;

public struct ShouldShowTargetableEvent : IEvent
{
    [CanBeNull] public IsCardValidTarget IsCardValidTarget;
    
    public ShouldShowTargetableEvent(IsCardValidTarget isCardValidTarget)
    {
        IsCardValidTarget = isCardValidTarget;
    }
}