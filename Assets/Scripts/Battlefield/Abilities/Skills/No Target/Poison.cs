using System.Collections.Generic;

public class Poison : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        DuelManager.GetNotIDOwner(target.id).AddPlayerCounter(PlayerCounters.Poison, target.card.cardType.Equals(CardType.Spell) ? 2 : 1);
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
