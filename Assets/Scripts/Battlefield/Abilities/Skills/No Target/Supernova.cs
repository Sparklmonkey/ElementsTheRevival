using System.Collections.Generic;
using System.Linq;

public class Supernova : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        for (int i = 0; i < 12; i++)
        {
            Owner.GenerateQuantaLogic((Element)i, 2);
        }
        if (BattleVars.shared.isSingularity > 0)
        {
            Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("6ub"));
        }
        BattleVars.shared.isSingularity++;
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
