public struct DisplayCreatureAttackEvent : IEvent
{
    public ID AttackingId;
    public int AttackValue;

    public DisplayCreatureAttackEvent(ID attackingId, int attackValue)
    {
        AttackingId = attackingId;
        AttackValue = attackValue;
    }
}