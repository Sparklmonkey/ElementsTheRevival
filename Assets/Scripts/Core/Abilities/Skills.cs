using System.Collections;
using System.Collections.Generic;

public abstract class SkillBase
{
    public readonly PlayerManager Player = DuelManager.Instance.player;
    public readonly PlayerManager Enemy = DuelManager.Instance.enemy;
    public virtual List<ID> SetupValidTargets()
    {
        return new();
    }

    public virtual IEnumerator ActivateAbility(bool targetIsPlayer, Card targetCard, ID targetId)
    {
        yield break;
    }
}



