using System.Collections.Generic;

public abstract class AbilityEffect
{
    public PlayerManager Owner { get; set; }
    public Card Origin { get; set; }
    public abstract bool NeedsTarget();
    public abstract (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets);
    public abstract List<(ID, Card)> GetPossibleTargets(PlayerManager enemy);
    public abstract bool IsCardValid(ID id, Card card);
    public abstract void Activate(ID targetId, Card targetCard);
    public abstract TargetPriority GetPriority();
}