using System.Collections.Generic;
using Core.Helpers;

public class Mitosis : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var card = CardDatabase.Instance.GetCardFromId(targetCard.Id);
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.IsOwnedBy(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, card));
    }
}
