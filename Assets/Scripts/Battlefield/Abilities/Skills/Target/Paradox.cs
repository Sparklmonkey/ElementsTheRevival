using System.Collections.Generic;
using System.Linq;

public class Paradox : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(IDCardPair target)
    {
        EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(target.id));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable() && x.card.AtkNow > x.card.DefNow);
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return null;
        }

        return possibleTargets.Aggregate((i1, i2) => i1.card.AtkNow >= i2.card.AtkNow ? i1 : i2);
    }
}