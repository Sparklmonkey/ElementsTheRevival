using System.Collections.Generic;
using UnityEngine;

public class Liquidshadow : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.HighestHp;

    public override void Activate(IDCardPair target)
    {
        target.card.skill = "";
        target.card.desc = "";
        target.card.passiveSkills = new();
        target.card.passiveSkills.Vampire = true;
        target.card.Poison++;
        target.UpdateCard();
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