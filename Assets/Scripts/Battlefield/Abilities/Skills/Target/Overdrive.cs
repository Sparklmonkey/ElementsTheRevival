using System.Collections.Generic;
using Battlefield.Abilities;
using UnityEngine;

public class Overdrive : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.Desc = "Overdrive: \n Gain +3 /-1 per turn";
        targetCard.TurnEndAbility = new OverdriveEndTurn();
        targetCard.Skill = null;
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Creature) && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.DefineDef, 1, 3, 0);
    }
}