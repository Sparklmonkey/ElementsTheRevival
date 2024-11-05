using System.Collections.Generic;
using System.Linq;
using Core.Helpers;

public class Dryspell : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return id.IsCreatureField();
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
        
        targetCard.SetDefDamage(1);
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(1, true, false, targetId.owner.Not()));
        }
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Water, BattleVars.Shared.AbilityIDOrigin.owner, true));
        victim.RemoveAllCloaks();
    }
}
