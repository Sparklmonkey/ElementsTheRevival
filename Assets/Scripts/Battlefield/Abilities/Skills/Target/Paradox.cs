using System.Collections.Generic;
using System.Linq;

public class Paradox : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable() && x.Item2.AtkNow > x.Item2.DefNow);
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        return possibleTargets.Count == 0 ? default : possibleTargets.Aggregate((i1, i2) => i1.Item2.AtkNow >= i2.Item2.AtkNow ? i1 : i2);
    }
}