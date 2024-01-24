using System.Collections.Generic;

public class Duality : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var cardToAdd = DuelManager.Instance.GetNotIDOwner(targetId).DeckManager.GetTopCard();
        if (cardToAdd == null) { return; }
        EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(Owner.Owner, new(cardToAdd)));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
