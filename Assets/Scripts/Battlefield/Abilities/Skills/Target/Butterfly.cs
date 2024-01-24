using System.Collections.Generic;
using UnityEngine;

public class Butterfly : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.SelfLowAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.skill = "destroy";
        targetCard.skillCost = 3;
        targetCard.skillElement = Element.Entropy;
        targetCard.desc = "<sprite=6><sprite=6><sprite=6>: Destroy: \n Destroy the targeted permanent";
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        return;
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.AtkNow < 4 && card.IsTargetable();
    }
    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        return possibleTargets.Count == 0 ? new() : possibleTargets.FindAll(x => x.IsTargetable() && x.Item2.AtkNow < 4);
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return default; }

        var opCreatures = possibleTargets.FindAll(x => x.Item1.owner == OwnerEnum.Opponent && x.HasCard());

        
        return opCreatures.Count == 0 ? default : opCreatures[Random.Range(0, possibleTargets.Count)];
    }
}