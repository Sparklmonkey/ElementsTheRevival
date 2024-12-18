using System.Collections.Generic;

public class Precognition : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return id.Equals(
            new ID(BattleVars.Shared.AbilityIDOrigin.owner, FieldEnum.Player, 0));
    }
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(targetId.owner));
        DuelManager.Instance.GetNotIDOwner(targetId).DisplayHand();
    }
}
