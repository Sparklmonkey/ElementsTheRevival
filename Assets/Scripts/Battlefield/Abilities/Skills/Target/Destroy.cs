﻿using System.Collections.Generic;
using UnityEngine;

public class Destroy : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        target.RemoveCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerPermanentManager.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerPermanentManager.GetAllValidCardIds());
        if (enemy.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetWeapon());
        }
        if (Owner.playerPassiveManager.GetWeapon().HasCard())
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetWeapon());
        }
        if (enemy.playerPassiveManager.GetShield().HasCard())
        {
            possibleTargets.Add(enemy.playerPassiveManager.GetShield());
        }
        if (Owner.playerPassiveManager.GetShield().HasCard())
        {
            possibleTargets.Add(Owner.playerPassiveManager.GetShield());
        }

        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }
        return posibleTargets[Random.Range(0, posibleTargets.Count)];
    }
}