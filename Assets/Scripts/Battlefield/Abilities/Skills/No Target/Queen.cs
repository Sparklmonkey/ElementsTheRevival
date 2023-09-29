using System.Collections.Generic;

public class Queen : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.PlayCardOnField(target.card.iD.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("7n4")
            : CardDatabase.Instance.GetCardFromId("5ok"));
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
