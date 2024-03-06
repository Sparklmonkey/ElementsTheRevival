using System.Collections.Generic;
using System.Linq;
using Core.Helpers;

public class Blitz : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return id.IsCreatureField() && id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner);
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(75, Element.Air, BattleVars.Shared.AbilityIDOrigin.owner, false));
        
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "Dive", Element.Other));
        targetCard.passiveSkills.Dive = true;
        targetCard.AtkModify *= 2;
        targetCard.Atk *= 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
