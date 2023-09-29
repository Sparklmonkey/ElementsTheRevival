using System.Collections.Generic;

public class Dejavu : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        target.card.skill = "";
        target.card.desc = "";
        Card dupe = new(target.card);
        Owner.PlayCardOnField(dupe);
        target.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
