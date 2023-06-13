using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class SkillBase
{
    public readonly PlayerManager player = DuelManager.Instance.player;
    public readonly PlayerManager enemy = DuelManager.Instance.enemy;
    public virtual List<ID> SetupValidTargets()
    {
        return new();
    }

    public virtual IEnumerator ActivateAbility(bool targetIsPlayer, Card targetCard, ID targetId)
    {
        yield break;
    }
}



