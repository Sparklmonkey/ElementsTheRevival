using System.Collections.Generic;
using UnityEngine;

public class Nymph : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Pillar;

    public override void Activate(IDCardPair target)
    {
        var element = target.card.costElement;
        Owner.PlayCardOnFieldLogic(target.card.iD.IsUpgraded()
            ? CardDatabase.Instance.GetRandomEliteNymph(element)
            : CardDatabase.Instance.GetRandomRegularNymph(element));
        target.RemoveCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerPermanentManager.GetAllValidCardIds();
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.card.cardType.Equals(CardType.Pillar));
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