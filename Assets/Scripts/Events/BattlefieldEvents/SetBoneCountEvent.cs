public struct SetBoneCountEvent : IEvent
{
    public OwnerEnum Owner;
    public int Amount;
    public SetBoneCountEvent(OwnerEnum owner, int amount)
    {
        Owner = owner;
        Amount = amount;
    }
}