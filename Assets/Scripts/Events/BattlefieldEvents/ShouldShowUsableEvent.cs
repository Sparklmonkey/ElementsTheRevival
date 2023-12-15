public struct ShouldShowUsableEvent : IEvent
{
    public bool ShouldShow;
    public ID DisplayerId;
    
    public ShouldShowUsableEvent(bool shouldShow, ID displayerId)
    {
        ShouldShow = shouldShow;
        DisplayerId = displayerId;
    }
}