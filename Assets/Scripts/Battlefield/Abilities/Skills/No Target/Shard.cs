using System.Collections.Generic;

public class Shard : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        int maxHpBuff = Owner.playerPassiveManager.GetMark().card.costElement.Equals(Element.Light) ? 24 : 16;
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
