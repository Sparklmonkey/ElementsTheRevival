using System.Collections.Generic;
using UnityEngine;

public class Nightmare : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.HighestCost;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var opponent = DuelManager.Instance.GetNotIDOwner(Owner.playerID);
        var creature = CardDatabase.Instance.GetCardFromId(targetCard.iD);

        var damage = 7 - opponent.playerHand.GetHandCount();
        opponent.FillHandWith(creature);
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damage * 2, true, true, opponent.Owner));
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damage * 2, false, true, Owner.Owner));
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

        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}