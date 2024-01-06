using System.Collections.Generic;
using UnityEngine;

public class Nymph : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.OwnPillar;

    public override void Activate(ID targetId, Card targetCard)
    {
        var element = targetCard.costElement;
        var card = targetCard.iD.IsUpgraded()
            ? CardDatabase.Instance.GetRandomEliteNymph(element)
            : CardDatabase.Instance.GetRandomRegularNymph(element);
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, card));
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerPermanentManager.GetAllValidCardIds();
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.Item2.cardType.Equals(CardType.Pillar));
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0)
        {
            return default;
        }

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}