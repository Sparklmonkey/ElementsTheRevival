using System.Collections.Generic;

public class Rebirth : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        target.PlayCard(target.card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7ds") : CardDatabase.Instance.GetCardFromId("5fc"));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}
