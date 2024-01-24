using System.Collections.Generic;

public class Nova : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        for (var i = 0; i < 12; i++)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)i, Owner.Owner, true));
        }
        if (BattleVars.Shared.IsSingularity > 1)
        {
            var card = CardDatabase.Instance.GetCardFromId("4vr");
            
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.owner.Equals(OwnerEnum.Player)));
            EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, card));
        }
        BattleVars.Shared.IsSingularity++;
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
