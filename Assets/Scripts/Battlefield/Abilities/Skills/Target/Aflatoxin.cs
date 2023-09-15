using System.Collections.Generic;
using UnityEngine;

public class Aflatoxin : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override TargetPriority GetPriority() => TargetPriority.OpHighAtk;

    public override void Activate(IDCardPair target)
    {
        target.card.IsAflatoxin = true;
        target.card.Poison += 2;
        target.UpdateCard();
        if (target.card.DefNow > 0 && target.card.innateSkills.Voodoo)
        {
            Owner.AddPlayerCounter(PlayerCounters.Poison, 2);
        }
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