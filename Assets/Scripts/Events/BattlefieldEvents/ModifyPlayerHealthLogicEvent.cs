public struct ModifyPlayerHealthLogicEvent : IEvent
{
    public int Amount;
    public bool IsPlayer;
    public bool IsMaxChange;
    
    public ModifyPlayerHealthLogicEvent(int amount, bool isPlayer, bool isMaxChange)
    {
        Amount = amount;
        IsPlayer = isPlayer;
        IsMaxChange = isMaxChange;
    }
}