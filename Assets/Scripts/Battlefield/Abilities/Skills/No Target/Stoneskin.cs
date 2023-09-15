using System.Collections.Generic;

public class Stoneskin : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        int maxHpBuff = Owner.GetAllQuantaOfElement(Element.Earth);
        Owner.ModifyMaxHealthLogic(maxHpBuff, true);
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
