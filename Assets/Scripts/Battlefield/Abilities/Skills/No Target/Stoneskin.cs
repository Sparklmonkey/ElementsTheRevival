using System.Collections.Generic;

public class Stoneskin : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var maxHpBuff = Owner.GetAllQuantaOfElement(Element.Earth);
        EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(maxHpBuff, Owner.isPlayer, true));
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
