using System.Collections.Generic;
using UnityEngine;

public class Steal : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(new(targetCard), targetId.owner.Equals(OwnerEnum.Player)));
        
        switch (targetCard.cardType)
        {
            case CardType.Artifact:
            case CardType.Pillar:
                EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(BattleVars.Shared.AbilityIDOrigin.owner, new(targetCard)));
                break;
            case CardType.Weapon:
            case CardType.Shield:
            case CardType.Mark:
                EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(BattleVars.Shared.AbilityIDOrigin.owner, new(targetCard)));
                break;
        }
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "Steal", Element.Other));
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        if (id.field.Equals(FieldEnum.Permanent) && card.IsTargetable())
        {
            return true;
        }

        if (card.iD is "4t1" or "4t2") return false;
        if (card.cardType.Equals(CardType.Mark)) return false;
        return id.field.Equals(FieldEnum.Passive) && card.cardType is CardType.Shield or CardType.Weapon && card.IsTargetable();
    }
}