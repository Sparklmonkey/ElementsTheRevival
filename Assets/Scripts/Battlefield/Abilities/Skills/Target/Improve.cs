using System.Collections.Generic;
using UnityEngine;

public class Improve : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(IDCardPair target)
    {
        AnimationManager.Instance.StartAnimation("Mutation", target.transform);
        EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(target.id, CardDatabase.Instance.GetMutant(target.card.iD.IsUpgraded())));
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

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}