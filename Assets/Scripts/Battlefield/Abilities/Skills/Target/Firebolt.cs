using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Firebolt : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var quantaElement = DuelManager.Instance.GetIDOwner(BattleVars.Shared.AbilityIDOrigin).GetAllQuantaOfElement(Element.Fire);
        var damageToDeal = 3 + Mathf.FloorToInt(quantaElement / 10) * 3;

        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, true, true, targetId.owner));
            return;
        }

        targetCard.DefDamage += damageToDeal;
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(targetCard.DefNow < damageToDeal ? targetCard.DefNow : damageToDeal, true, false, targetId.owner.Not()));
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