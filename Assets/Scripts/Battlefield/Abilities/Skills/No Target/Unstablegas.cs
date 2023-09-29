using System.Collections.Generic;

public class Unstablegas : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.PlayCardOnField(target.card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n6") : CardDatabase.Instance.GetCardFromId("5om"));
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
