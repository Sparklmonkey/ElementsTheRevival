using System.Collections.Generic;
using System.Linq;

public class Rainoffire : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return !id.owner.Equals(BattleVars.Shared.AbilityIDOrigin.owner) && id.field.Equals(FieldEnum.Creature);
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var victim = DuelManager.Instance.GetNotIDOwner(targetId);
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("Lightning"));
        
        targetCard.DefDamage += 3;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));

        if (victim.playerCounters.invisibility > 0)
        {
            victim.RemoveAllCloaks();
        }
    }
}
