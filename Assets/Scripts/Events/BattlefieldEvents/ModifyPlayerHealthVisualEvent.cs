public struct ModifyPlayerHealthVisualEvent : IEvent
{
    public int CurrentHp;
    public int MaxHp;
    public bool IsPlayer;
    
    public ModifyPlayerHealthVisualEvent(int currentHp, bool isPlayer, int maxHp)
    {
        CurrentHp = currentHp;
        IsPlayer = isPlayer;
        MaxHp = maxHp;
    }
}