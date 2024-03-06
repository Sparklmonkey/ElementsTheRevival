using System.Collections.Generic;
using UnityEngine;

public class Mitosiss : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Skill = new Mitosis();
        targetCard.Desc = "Mitosis: \n Generate a daughter creature";
        targetCard.SkillCost = targetCard.Cost;
        targetCard.SkillElement = targetCard.CostElement;
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.DefineAtk, 1, 25, 25);
    }
}