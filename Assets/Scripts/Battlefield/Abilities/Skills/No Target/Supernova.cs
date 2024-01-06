using System.Collections.Generic;

public class Supernova : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        for (var i = 0; i < 12; i++)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, (Element)i, Owner.Owner, true));
        }

        if (BattleVars.Shared.IsSingularity > 0)
        {
            var card = CardDatabase.Instance.GetCardFromId("6ub");
            
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.owner.Equals(OwnerEnum.Player)));
            EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(card, targetId.owner));
        }

        BattleVars.Shared.IsSingularity++;
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
