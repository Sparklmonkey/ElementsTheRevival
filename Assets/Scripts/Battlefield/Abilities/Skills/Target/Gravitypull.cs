using System;
using System.Collections.Generic;
using UnityEngine;

public class Gravitypull : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.passiveSkills.GravityPull = true;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
    
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }
    public override AiTargetType GetTargetType()
    {
        var hasNoGravityCreature =
            DuelManager.Instance.player.playerCreatureField.GetCreatureWithGravity().Equals(default);
        var prge = DuelManager.Instance.enemy.HealthManager.GetCurrentHealth() /
            (Math.Abs(DuelManager.Instance.GetPossibleDamage(true)) + 1) < 5;
        if (hasNoGravityCreature && prge)
        {
            return new AiTargetType(true, false, false, TargetType.DefineDef, 1, 5, 0);
        }

        return null;
    }
}