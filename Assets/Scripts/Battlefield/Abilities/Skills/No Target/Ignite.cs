using System.Collections.Generic;
using System.Linq;

public class Ignite : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        if (id.field.Equals(FieldEnum.Player) && !id.owner.Equals(BattleVars.Shared.AbilityIDOrigin.owner)) return true;
        return id.field.Equals(FieldEnum.Creature);
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));

        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(20, true, false, targetId.owner));
        }
        else
        {
            targetCard.DefDamage += 1;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        }
    }
}
