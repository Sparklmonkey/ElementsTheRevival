using System.Collections.Generic;

public class Nova : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        for (int i = 0; i < 12; i++)
        {
            Owner.GenerateQuantaLogic((Element)i, 1);
        }
        if (BattleVars.Shared.IsSingularity > 1)
        {
            Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4vr"));
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
