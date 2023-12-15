using System.Collections.Generic;

public class Precognition : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner.isPlayer));
        DuelManager.Instance.GetNotIDOwner(Owner.playerID.id).DisplayHand();
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
