using System.Collections.Generic;
using System.Linq;
using Core.Helpers;

public class Desiccation : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return id.IsCreatureField() && !id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner);
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard.IsBurrowedOrImmaterial())
        {
            return;
        }
        var victim = DuelManager.Instance.GetNotIDOwner(BattleVars.Shared.AbilityIDOrigin);
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("Lightning"));
        
        targetCard.SetDefDamage(2);
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(2, true, false, targetId.owner.Not()));
        }
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(2, Element.Water, BattleVars.Shared.AbilityIDOrigin.owner, true));
        victim.RemoveAllCloaks();
    }
}
