using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

public class Nymph : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var element = targetCard.CostElement;
        var card = targetCard.Id.IsUpgraded()
            ? CardDatabase.Instance.GetRandomEliteNymph(element)
            : CardDatabase.Instance.GetRandomRegularNymph(element);
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.IsOwnedBy(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, card));
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.Type.Equals(CardType.Pillar) && id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner) && card.IsTargetable();
    }
    
    public override AiTargetType GetTargetType()
    {
        return new AiTargetType(false, false, false, TargetType.Tears, 1, 0, 0);
    }
}