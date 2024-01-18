using System.Collections.Generic;

public class Unstablegas : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var card = targetCard.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n6") : CardDatabase.Instance.GetCardFromId("5om");
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(targetId.owner, card));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
