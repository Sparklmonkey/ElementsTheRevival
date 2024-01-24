using System.Collections.Generic;

public class Shard : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var maxHpBuff = Owner.playerPassiveManager.GetMark().Item2.costElement.Equals(Element.Light) ? 24 : 16;
        EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(maxHpBuff, Owner.Owner, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
