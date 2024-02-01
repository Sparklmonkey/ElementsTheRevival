using System.Collections.Generic;

public class Shard : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card)
    {
        return id.Equals(
            new ID(BattleVars.Shared.AbilityIDOrigin.owner, FieldEnum.Player, 0));
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var maxHpBuff = DuelManager.Instance.GetIDOwner(targetId).playerPassiveManager.GetMark().Item2.costElement.Equals(Element.Light) ? 24 : 16;
        EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(maxHpBuff, targetId.owner, true));
    }
}
