using System.Collections.Generic;
using UnityEngine;

public class Web : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.HasSkill;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "Web", Element.Other));
        targetCard.innateSkills.Airborne = false;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        return possibleTargets.Count == 0 ? new() : possibleTargets.FindAll(x => x.IsTargetable() && x.Item2.innateSkills.Airborne);
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable() && card.innateSkills.Airborne;
    }
    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        return possibleTargets.Count == 0 ? default : possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}