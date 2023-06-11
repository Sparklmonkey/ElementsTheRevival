using System.Collections.Generic;
using System.Linq;

public class Nova : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        for (int i = 0; i < 12; i++)
        {
            Owner.GenerateQuantaLogic((Element)i, 1);
        }
        if (BattleVars.shared.isSingularity > 1)
        {
            Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4vr"));
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
