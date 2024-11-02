public struct SetBoneCountEvent : IEvent
{
    public OwnerEnum Owner;
    public int Amount;
    public bool IsFromDestroy;
    public SetBoneCountEvent(OwnerEnum owner, int amount, bool isFromDestroy)
    {
        Owner = owner;
        Amount = amount;
        IsFromDestroy = isFromDestroy;
    }
}