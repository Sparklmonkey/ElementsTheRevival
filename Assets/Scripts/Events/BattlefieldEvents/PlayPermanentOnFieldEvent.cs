public struct PlayPermanentOnFieldEvent : IEvent
{
    public OwnerEnum Owner;
    public readonly Card CardToPlay;
    
    public PlayPermanentOnFieldEvent(OwnerEnum owner, Card cardToPlay)
    {
        Owner = owner;
        CardToPlay = cardToPlay;
    }
}

public struct PlayAnimationEvent : IEvent
{
    public ID Id;
    public string AnimName;
    public Element Element;
    
    public PlayAnimationEvent(ID id, string animName, Element element)
    {
        Id = id;
        AnimName = animName;
        Element = element;
    }
}