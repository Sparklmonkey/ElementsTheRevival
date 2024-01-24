using System.Collections.Generic;

public class Evolve : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var card = targetCard.iD.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("77h")
            : CardDatabase.Instance.GetCardFromId("591");
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, card, false));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
