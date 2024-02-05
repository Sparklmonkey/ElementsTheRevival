using System.Collections.Generic;
using Core.Helpers;

public class Luciferin : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card)
    { 
        if(!id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner)) return false;
        if(id.IsPlayerField()) return true;
        return id.IsCreatureField() && card.skill == "";
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;

        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(10, false, true, targetId.owner));
        }
        else
        {
            targetCard.passiveSkills.Light = true;
            targetCard.desc = "Bioluminescence : \n Each turn <sprite=3> is generated";
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        }
    }
}
