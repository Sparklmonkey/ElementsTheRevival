﻿using System.Collections.Generic;
using UnityEngine;

public class Tsunami : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.RemoveCard();

        if (target.HasCard())
        {
            target.RemoveCard();

            if (target.HasCard())
            {
                target.RemoveCard();
            }
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = Owner.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(enemy.playerPermanentManager.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return null; }
        return possibleTargets[Random.Range(0, possibleTargets.Count)];
    }
}