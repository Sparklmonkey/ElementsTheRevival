public struct UpdatePlayerCountersVisualEvent : IEvent
{
    public Counters Counters;
    public ID PlayerId;
    
    public UpdatePlayerCountersVisualEvent(ID playerId, Counters counters)
    {
        Counters = counters;
        PlayerId = playerId;
    }
}