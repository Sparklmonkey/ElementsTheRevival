using System.Collections.Generic;

public abstract class AbilityEffect
{
    public PlayerManager Owner { get; set; }
    public IDCardPair Origin { get; set; }
    public abstract bool NeedsTarget();
    public abstract IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets);
    public abstract List<IDCardPair> GetPossibleTargets(PlayerManager enemy);
    public abstract void Activate(IDCardPair target);
    public abstract TargetPriority GetPriority();
}