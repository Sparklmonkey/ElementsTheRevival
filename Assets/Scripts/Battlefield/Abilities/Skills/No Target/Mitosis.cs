using System.Collections.Generic;

public class Mitosis : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.PlayCardOnField(CardDatabase.Instance.GetCardFromId(target.card.iD));
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
