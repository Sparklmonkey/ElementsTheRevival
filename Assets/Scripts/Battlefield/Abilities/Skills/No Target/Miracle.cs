using System.Collections.Generic;

public class Miracle : ActivatedAbility
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
        var maxHp = DuelManager.Instance.GetIDOwner(targetId).HealthManager.GetMaxHealth();
        var currentHp = DuelManager.Instance.GetIDOwner(targetId).HealthManager.GetCurrentHealth();

        var hpToHeal = maxHp - currentHp - 1;

        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(hpToHeal, false, true, targetId.owner));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Light, targetId.owner, false));
    }
}
