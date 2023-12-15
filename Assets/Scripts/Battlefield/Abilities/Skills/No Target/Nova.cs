using System.Collections.Generic;

public class Nova : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        for (var i = 0; i < 12; i++)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, Owner.isPlayer, true));
        }
        if (BattleVars.Shared.IsSingularity > 1)
        {
            Owner.PlayCardOnField(CardDatabase.Instance.GetCardFromId("4vr"));
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
