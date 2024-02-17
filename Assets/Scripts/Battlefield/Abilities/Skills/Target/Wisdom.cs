using System.Collections.Generic;
using Battlefield.Abilities;
using UnityEngine;

public class Wisdom : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.AtkModify += 4;
        targetCard.passiveSkills.Psion = true;
        targetCard.Desc = $"{targetCard.CardName}'s attacks deal spell damage.";
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.innateSkills.Immaterial;
    }

    public override AiTargetType GetTargetType()
    {
        if (DuelManager.Instance.enemy.playerPassiveManager.GetShield().Item2.ShieldPassive is ReflectSkill)
        {
            return new AiTargetType(true, false, false, TargetType.Immortals, 1, 0, 0);
        }

        if (DuelManager.Instance.player.playerPassiveManager.GetShield().Item2.ShieldPassive is ReflectSkill)
        {
            return new AiTargetType(false, true, false, TargetType.Immortals, 1, 0, 0);
        }

        return null;
    }
}