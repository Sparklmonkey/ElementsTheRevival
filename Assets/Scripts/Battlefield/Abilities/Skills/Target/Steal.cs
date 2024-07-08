using System.Collections.Generic;
using Battlefield.Abilities;
using Core.Helpers;
using UnityEngine;

public class Steal : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(targetCard.Clone(), targetId.IsOwnedBy(OwnerEnum.Player)));
        
        switch (targetCard.Type)
        {
            case CardType.Artifact:
            case CardType.Pillar:
                EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(BattleVars.Shared.AbilityIDOrigin.owner, targetCard.Clone()));
                break;
            case CardType.Weapon:
            case CardType.Shield:
            case CardType.Mark:
                EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(BattleVars.Shared.AbilityIDOrigin.owner, targetCard.Clone()));
                break;
        }
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "Steal", Element.Other));
        if (targetCard.ShieldPassive is BoneSkill)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, targetId.owner, -1));
        }
        else
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
        }
    }
    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        if (id.IsPermanentField() && card.IsTargetable(id))
        {
            return true;
        }

        if (card.Id is "4t1" or "4t2") return false;
        if (card.Type.Equals(CardType.Mark)) return false;
        return id.field.Equals(FieldEnum.Passive) && card.Type is CardType.Shield or CardType.Weapon && card.IsTargetable(id);
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Permanent, -1, 0, 0);
    }
}