using System.Collections.Generic;
using System.Linq;

public class Evolve : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        BattleVars.shared.abilityOrigin.PlayCard(BattleVars.shared.abilityOrigin.card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("77h") : CardDatabase.Instance.GetCardFromId("591"));
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
