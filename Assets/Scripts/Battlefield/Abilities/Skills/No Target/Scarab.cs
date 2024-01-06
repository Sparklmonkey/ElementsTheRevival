using System.Collections.Generic;

public class Scarab : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var card = targetCard.iD.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("7qa")
            : CardDatabase.Instance.GetCardFromId("5rq");
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(card, targetId.owner));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
