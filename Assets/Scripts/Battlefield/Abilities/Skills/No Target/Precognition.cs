using System.Collections.Generic;

public class Precognition : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card) => false;
    public override void Activate(ID targetId, Card targetCard)
    {
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner.Owner));
        DuelManager.Instance.GetNotIDOwner(Owner.playerID).DisplayHand();
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
