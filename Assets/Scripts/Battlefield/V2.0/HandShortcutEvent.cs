public struct HandShortcutEvent : IEvent
{
    public ID Id;

    public HandShortcutEvent(int index)
    {
        Id = new ID(OwnerEnum.Player, FieldEnum.Hand, index);
    }
}