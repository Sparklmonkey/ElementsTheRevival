using System.Collections.Generic;

public class Miracle : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var maxHp = Owner.HealthManager.GetMaxHealth();
        var currentHp = Owner.HealthManager.GetCurrentHealth();

        var hpToHeal = maxHp - currentHp - 1;

        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(hpToHeal, false, true, Owner.Owner));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Light, Owner.Owner, false));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
