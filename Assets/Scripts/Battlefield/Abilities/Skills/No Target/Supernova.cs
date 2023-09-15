using System.Collections.Generic;

public class Supernova : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        for (int i = 0; i < 12; i++)
        {
            Owner.GenerateQuantaLogic((Element)i, 2);
        }

        if (BattleVars.Shared.IsSingularity > 0)
        {
            Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("6ub"));
        }

        BattleVars.Shared.IsSingularity++;
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
