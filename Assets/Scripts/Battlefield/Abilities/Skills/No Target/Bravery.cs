using System.Collections.Generic;

public class Bravery : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var cardToDraw = Owner.playerPassiveManager.GetMark().card.costElement.Equals(Element.Fire) ? 3 : 2;
        for (var i = 0; i < cardToDraw; i++)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner.isPlayer));
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(!Owner.isPlayer));
        }
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
