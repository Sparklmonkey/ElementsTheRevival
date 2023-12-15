using System.Collections.Generic;

public class Duality : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var cardToAdd = DuelManager.Instance.GetNotIDOwner(target.id).DeckManager.GetTopCard();
        if (cardToAdd == null) { return; }
        EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(Owner.isPlayer, new(cardToAdd)));
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
