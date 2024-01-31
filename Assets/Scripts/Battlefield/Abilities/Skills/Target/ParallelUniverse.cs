using System.Collections.Generic;
using System.Linq;

public class Paralleluniverse : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "ParallelUniverse", Element.Other));
        Card dupe = new(targetCard);
        dupe.DefDamage = targetCard.DefDamage;
        dupe.DefModify = targetCard.DefModify;
        dupe.AtkModify = targetCard.AtkModify;

        if (dupe.innateSkills.Voodoo)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(targetCard.DefDamage, true, false, targetId.owner.Not()));
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, targetId.owner.Not(), targetCard.Poison));
        }

        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(dupe, BattleVars.Shared.AbilityIDOrigin.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(BattleVars.Shared.AbilityIDOrigin.owner, dupe));
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}