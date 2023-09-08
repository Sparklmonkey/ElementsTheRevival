using System.Collections.Generic;
using System.Linq;

public class Photosynthesis : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        AnimationManager.Instance.StartAnimation("QuantaGenerate", target.transform, Element.Life);
        Owner.GenerateQuantaLogic(Element.Life, 2);
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
