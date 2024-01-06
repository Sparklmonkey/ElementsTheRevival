using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mutation : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        // AnimationManager.Instance.StartAnimation("Mutation", target.transform);
        switch (GetMutationResult())
        {
            case MutationEnum.Kill:
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
                return;
            case MutationEnum.Mutate:
                EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, CardDatabase.Instance.GetMutant(targetCard.iD.IsUpgraded())));
                break;
            default:
                EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, CardDatabase.Instance.GetCardFromId(targetCard.iD.IsUpgraded() ? "6tu" : "4ve")));
                break;
        }
    }

    private MutationEnum GetMutationResult()
    {
        var num = Random.Range(0, 100);
        return num switch
        {
            >= 90 => MutationEnum.Kill,
            >= 50 => MutationEnum.Mutate,
            _ => MutationEnum.Abomination
        };
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
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

        var opCreatures = possibleTargets.FindAll(x => x.Item1.owner == OwnerEnum.Player && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return possibleTargets.Find(x => x.Item1.owner == OwnerEnum.Player);
        }
        else
        {
            return opCreatures.Aggregate((i1, i2) => i1.Item2.AtkNow >= i2.Item2.AtkNow ? i1 : i2);
        }
    }
}