using System.Collections.Generic;

public class Hatch : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        target.PlayCard(target.card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteHatchCreature() : CardDatabase.Instance.GetRandomHatchCreature());
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
