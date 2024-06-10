using System.Collections.Generic;
using System.Linq;
using Core.Helpers;
using UnityEngine;

public class Icebolt : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var quantaElement = DuelManager.Instance.GetIDOwner(BattleVars.Shared.AbilityIDOrigin).GetAllQuantaOfElement(Element.Water);
        var damageToDeal = 2 + Mathf.FloorToInt(quantaElement / 10) * 2;
        var willFreeze = Random.Range(0, 100) > 30 + damageToDeal * 5;

        if (targetCard is null)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(damageToDeal, true, true, targetId.owner));
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Freeze,  targetId.owner.Not(), willFreeze ? 3 : 0));
            return;
        }

        targetCard.DefDamage += damageToDeal;
        targetCard.Counters.Freeze += willFreeze ? 3 : 0;
        if (targetCard.DefNow > 0 && targetCard.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(targetCard.DefNow < damageToDeal ? targetCard.DefNow : damageToDeal, true, false, targetId.owner.Not()));
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Freeze, targetId.owner.Not(), willFreeze ? 3 : 0));
        }

        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null)
        {
            return id.IsPlayerField();
        }
        return card.Type.Equals(CardType.Creature) && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.CreatureAndPlayer, -2, 0, 0);
    }
}