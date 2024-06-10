using System.Collections.Generic;
using System.Linq;
using Core.Helpers;

public class Thunderstorm : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return !id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner) && id.IsCreatureField();
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (!targetCard.IsTargetable(targetId)) return;
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("Lightning"));
        targetCard.DefDamage += 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
