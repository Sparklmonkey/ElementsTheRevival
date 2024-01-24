using System.Collections.Generic;
using UnityEngine;

public class Wisdom : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.SelfHighAtk;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.AtkModify += 4;
        targetCard.passiveSkills.Psion = true;
        targetCard.desc = $"{targetCard.cardName}'s attacks deal spell damage.";
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.Item2.innateSkills.Immaterial);
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.innateSkills.Immaterial;
    }

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets)
    {
        return possibleTargets.Count == 0 ? default : possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}