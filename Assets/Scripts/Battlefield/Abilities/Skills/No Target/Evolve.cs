using System.Collections.Generic;

public class Evolve : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        target.PlayCard(target.card.iD.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("77h")
            : CardDatabase.Instance.GetCardFromId("591"));
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
