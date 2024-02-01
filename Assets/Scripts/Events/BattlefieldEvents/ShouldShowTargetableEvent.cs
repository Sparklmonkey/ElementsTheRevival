using JetBrains.Annotations;

public struct ShouldShowTargetableEvent : IEvent
{
    [CanBeNull] public IsCardValidTarget IsCardValidTarget;
    public bool ShouldHideGraphic;
    
    public ShouldShowTargetableEvent(IsCardValidTarget isCardValidTarget, bool shouldHideGraphic = false)
    {
        IsCardValidTarget = isCardValidTarget;
        ShouldHideGraphic = shouldHideGraphic;
    }
}