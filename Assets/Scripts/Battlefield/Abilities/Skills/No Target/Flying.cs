using System.Collections.Generic;

public class Flying : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        Card weapon = new(Owner.playerPassiveManager.GetWeapon().Item2);
        if (weapon.iD == "4t2") return;
        weapon.cardType = CardType.Creature;
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(weapon, targetId.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, weapon));
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(new ID(targetId.owner, FieldEnum.Passive, 1)));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
