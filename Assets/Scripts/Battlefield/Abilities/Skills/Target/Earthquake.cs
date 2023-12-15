using System.Collections.Generic;
using UnityEngine;

public class Earthquake : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Pillar;

    public override void Activate(IDCardPair target)
    {
        EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(target.id));

        if (!target.HasCard()) return;
        EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(target.id));

        if (!target.HasCard()) return;
        EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(target.id));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerPermanentManager.GetAllValidCardIds()
            .FindAll(x => x.card.cardType == CardType.Pillar));
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
            return null;
        }
        else
        {
            return opCreatures[Random.Range(0, possibleTargets.Count)];
        }
    }
}