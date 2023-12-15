public struct ShouldShowTargetableEvent : IEvent
{
    public bool ShouldShow;
    public ID DisplayerId;
    
    public ShouldShowTargetableEvent(bool shouldShow, ID displayerId)
    {
        ShouldShow = shouldShow;
        DisplayerId = displayerId;
    }
}