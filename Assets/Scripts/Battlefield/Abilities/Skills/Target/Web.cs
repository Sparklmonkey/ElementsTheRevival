using System.Collections.Generic;
using UnityEngine;

public class Web : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        Game_AnimationManager.shared.StartAnimation("Web", target.transform);
        target.card.innateSkills.Airborne = false;
        target.UpdateCard();
        return;
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable() && x.card.innateSkills.Airborne);
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}