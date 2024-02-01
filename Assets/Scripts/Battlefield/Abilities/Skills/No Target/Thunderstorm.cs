using System.Collections.Generic;
using System.Linq;

public class Thunderstorm : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return !id.owner.Equals(BattleVars.Shared.AbilityIDOrigin.owner) && id.field.Equals(FieldEnum.Creature);
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.DefDamage += 2;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
