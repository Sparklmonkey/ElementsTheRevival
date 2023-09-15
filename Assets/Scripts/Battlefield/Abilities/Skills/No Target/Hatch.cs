using System.Collections.Generic;

public class Hatch : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var cardToPlay = CardDatabase.Instance.GetRandomCard(CardType.Creature, target.card.iD.IsUpgraded(), true);
        target.PlayCard(cardToPlay);
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
