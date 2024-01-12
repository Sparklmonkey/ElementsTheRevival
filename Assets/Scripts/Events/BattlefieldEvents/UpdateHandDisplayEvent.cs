public struct UpdateHandDisplayEvent : IEvent
{
    public OwnerEnum Owner;

    public UpdateHandDisplayEvent(OwnerEnum owner)
    {
        Owner = owner;
    }
}
public struct RemoveCardFromManagerEvent : IEvent
{
    public ID Id;

    public RemoveCardFromManagerEvent(ID id)
    {
        Id = id;
    }
}