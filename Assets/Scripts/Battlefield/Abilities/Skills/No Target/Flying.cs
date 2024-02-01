using System.Collections.Generic;

public class Flying : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID targetId, Card targetCard) => targetCard.cardType == CardType.Weapon && targetId.owner.Equals(BattleVars.Shared.AbilityIDOrigin.owner);

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        Card weapon = new(targetCard);
        if (weapon.iD == "4t2") return;
        weapon.cardType = CardType.Creature;
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(weapon, targetId.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, weapon));
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new ID(targetId.owner, FieldEnum.Passive, 1)));
    }
}
