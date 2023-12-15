using System.Collections.Generic;
using UnityEngine;

public class Petrify : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.SelfHighAtk;

    public override void Activate(IDCardPair target)
    {
        for (var i = 0; i < 6; i++)
        {
            target.card.innateSkills.Delay++;
        }

        target.card.DefModify += 20;
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