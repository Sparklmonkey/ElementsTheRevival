using System.Collections.Generic;
using System.Linq;

public class Paralleluniverse : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.AnyHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "ParallelUniverse", Element.Other));
        Card dupe = new(targetCard);
        dupe.DefDamage = targetCard.DefDamage;
        dupe.DefModify = targetCard.DefModify;
        dupe.AtkModify = targetCard.AtkModify;

        if (dupe.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(targetCard.DefDamage, true, false, targetId.owner.Not()));
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, targetId.owner.Not(), targetCard.Poison));
        }

        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(dupe, Owner.Owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(Owner.Owner, dupe));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        return possibleTargets.Aggregate((i1, i2) => i1.Item2.AtkNow >= i2.Item2.AtkNow ? i1 : i2);
    }
}