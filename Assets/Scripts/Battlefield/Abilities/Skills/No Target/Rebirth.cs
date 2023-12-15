using System.Collections.Generic;

public class Rebirth : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var card = target.card.iD.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("7ds")
            : CardDatabase.Instance.GetCardFromId("5fc");
        
        EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(target.id, card));
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
