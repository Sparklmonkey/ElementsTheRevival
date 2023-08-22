using System.Collections.Generic;
using System.Linq;

public class Scarab : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        Owner.PlayCardOnFieldLogic(target.card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7qa") : CardDatabase.Instance.GetCardFromId("5rq"));
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
