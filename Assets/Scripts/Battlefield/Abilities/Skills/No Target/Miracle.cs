using System.Collections.Generic;

public class Miracle : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var maxHp = Owner.HealthManager.GetMaxHealth();
        var currentHp = Owner.HealthManager.GetCurrentHealth();

        var hpToHeal = maxHp - currentHp - 1;

        Owner.ModifyHealthLogic(hpToHeal, false, true);
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Light, Owner.isPlayer, false));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
