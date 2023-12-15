using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mutation : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(IDCardPair target)
    {
        AnimationManager.Instance.StartAnimation("Mutation", target.transform);
        switch (GetMutationResult())
        {
            case MutationEnum.Kill:
                EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(target.id));
                return;
            case MutationEnum.Mutate:
                EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(target.id, CardDatabase.Instance.GetMutant(target.card.iD.IsUpgraded())));
                break;
            default:
                EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(target.id, CardDatabase.Instance.GetCardFromId(target.card.iD.IsUpgraded() ? "6tu" : "4ve")));
                break;
        }
    }

    private MutationEnum GetMutationResult()
    {
        var num = Random.Range(0, 100);
        if (num >= 90)
        {
            return MutationEnum.Kill;
        }
        else if (num >= 50)
        {
            return MutationEnum.Mutate;
        }
        else
        {
            return MutationEnum.Abomination;
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return null;
        }

        var opCreatures = possibleTargets.FindAll(x => x.id.owner == OwnerEnum.Player && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return possibleTargets.Find(x => x.id.owner == OwnerEnum.Player);
        }
        else
        {
            return opCreatures.Aggregate((i1, i2) => i1.card.AtkNow >= i2.card.AtkNow ? i1 : i2);
        }
    }
}