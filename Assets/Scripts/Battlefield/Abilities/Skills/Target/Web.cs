using System.Collections.Generic;
using UnityEngine;

public class Web : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.HasSkill;

    public override void Activate(IDCardPair target)
    {
        AnimationManager.Instance.StartAnimation("Web", target.transform);
        target.card.innateSkills.Airborne = false;
        target.UpdateCard();
        return;
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0)
        {
            return new();
        }

        return possibleTargets.FindAll(x => x.IsTargetable() && x.card.innateSkills.Airborne);
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