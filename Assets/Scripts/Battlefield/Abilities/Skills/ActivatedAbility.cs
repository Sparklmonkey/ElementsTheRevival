public abstract class ActivatedAbility
{
    public abstract bool NeedsTarget();
    public abstract bool IsCardValid(ID id, Card card);
    public abstract void Activate(ID targetId, Card targetCard);
    public abstract AiTargetType GetAiTargetType(string skill);
}