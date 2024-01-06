using System.Collections.Generic;

public class Sacrifice : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(40, true, false, Owner.Owner));
        for (var i = 0; i < 12; i++)
        {
            if ((Element)i == Element.Death) { continue; }
            
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, (Element)i, Owner.Owner, false));
        }
        EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Sacrifice, Owner.Owner, 2));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
