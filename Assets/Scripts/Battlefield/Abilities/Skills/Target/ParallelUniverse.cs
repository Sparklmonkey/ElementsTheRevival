using System.Collections.Generic;
using System.Linq;

public class Paralleluniverse : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.AnyHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        // AnimationManager.Instance.StartAnimation("ParallelUniverse", target.transform);
        Card dupe = new(targetCard);
        dupe.DefDamage = targetCard.DefDamage;
        dupe.DefModify = targetCard.DefModify;
        dupe.AtkModify = targetCard.AtkModify;

        if (dupe.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(targetCard.DefDamage, true, false, targetId.owner.Not()));
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, targetId.owner.Not(), targetCard.Poison));
        }

        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(dupe, targetId.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(dupe, targetId.owner));
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

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        return possibleTargets.Aggregate((i1, i2) => i1.Item2.AtkNow >= i2.Item2.AtkNow ? i1 : i2);
    }
}