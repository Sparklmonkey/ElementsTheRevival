﻿using System.Collections.Generic;
using System.Linq;

public class Paralleluniverse : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        Game_AnimationManager.shared.StartAnimation("ParallelUniverse", target.transform);
        Owner.PlayCardOnFieldLogic(new(target.card));
        return;
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets.Aggregate((i1, i2) => i1.card.AtkNow > i2.card.AtkNow ? i1 : i2);
    }
}