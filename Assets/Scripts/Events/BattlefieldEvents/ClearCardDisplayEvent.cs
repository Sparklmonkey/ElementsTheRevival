﻿public struct ClearCardDisplayEvent : IEvent
{
    public ID Id;
    public int Stack;
    public Card Card;
    
    public ClearCardDisplayEvent(ID id, int stack = 0, Card card = null)
    {
        Stack = stack;
        Id = id;
        Card = card;
    }
}