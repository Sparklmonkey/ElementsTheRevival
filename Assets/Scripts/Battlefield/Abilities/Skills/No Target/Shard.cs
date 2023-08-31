using System.Collections.Generic;
using System.Linq;

public class Shard : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        int maxHPBuff = Owner.playerPassiveManager.GetMark().card.costElement.Equals(Element.Light) ? 24 : 16;
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
