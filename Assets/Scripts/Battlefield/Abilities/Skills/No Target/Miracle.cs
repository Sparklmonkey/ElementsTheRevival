using System.Collections.Generic;

public class Miracle : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        int maxHp = Owner.HealthManager.GetMaxHealth();
        int currentHp = Owner.HealthManager.GetCurrentHealth();

        int hpToHeal = maxHp - currentHp - 1;

        Owner.ModifyHealthLogic(hpToHeal, false, true);
        Owner.SpendQuantaLogic(Element.Light, 75);
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
