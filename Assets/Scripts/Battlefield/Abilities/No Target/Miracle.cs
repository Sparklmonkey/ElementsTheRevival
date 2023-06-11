using System.Collections.Generic;
using System.Linq;

public class Miracle : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        int maxHp = Owner.healthManager.GetMaxHealth();
        int currentHP = Owner.healthManager.GetCurrentHealth();

        int hpToHeal = maxHp - currentHP - 1;

        Owner.ModifyHealthLogic(hpToHeal, false, true);
        Owner.SpendQuantaLogic(Element.Light, 75);
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}
