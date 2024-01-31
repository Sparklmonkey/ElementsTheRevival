using System.Collections.Generic;
using System.Linq;

public class Lightning : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(5, true, true, targetId.owner));
            return;
        }

        targetCard.DefDamage += 5;
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(5, true, false, targetId.owner.Not()));
        }

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null)
        {
            return id.field.Equals(FieldEnum.Player);
        }
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}