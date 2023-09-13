using System.Collections.Generic;

public class Stoneskin : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        int maxHPBuff = Owner.GetAllQuantaOfElement(Element.Earth);
        Owner.ModifyMaxHealthLogic(maxHPBuff, true);
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
