public abstract class ActivatedAbility
{
    public abstract bool NeedsTarget();
    public virtual bool IsCardValid(ID id, Card card)
    {
        return id.Equals(BattleVars.Shared.AbilityIDOrigin);
    }
    public abstract void Activate(ID targetId, Card targetCard);

    public virtual AiTargetType GetTargetType()
    {
        return null;
    }
    
    public ActivatedAbility Clone()
    {
        var clone = (ActivatedAbility)MemberwiseClone();
        return clone;
    }
}