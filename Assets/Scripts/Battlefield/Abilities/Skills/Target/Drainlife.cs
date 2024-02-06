using System.Collections.Generic;
using System.Linq;
using Core.Helpers;
using UnityEngine;

public class Drainlife : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var quantaElement = DuelManager.Instance.GetIDOwner(BattleVars.Shared.AbilityIDOrigin).GetAllQuantaOfElement(Element.Darkness);
        var damageToDeal = 2 + Mathf.FloorToInt(quantaElement / 10) * 2;

        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, true, true, targetId.owner));
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, false, true, BattleVars.Shared.AbilityIDOrigin.owner));
            return;
        }

        var defPlaceHolder = targetCard.DefNow;
        targetCard.DefDamage += damageToDeal;
        var amountToHeal = targetCard.DefNow > 0 ? damageToDeal : defPlaceHolder;

        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(amountToHeal, false, false, BattleVars.Shared.AbilityIDOrigin.owner));
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, true, false, targetId.owner.Not()));
        }

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null)
        {
            return id.IsPlayerField();
        }
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
    
}