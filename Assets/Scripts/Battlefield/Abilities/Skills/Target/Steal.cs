using System.Collections.Generic;
using UnityEngine;

public class Steal : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        Owner.PlayCardOnFieldLogic(new(target.card));
        Game_AnimationManager.shared.StartAnimation("Steal", target.transform);
        target.RemoveCard();
        return;
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerPassiveManager.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}