using System.Collections.Generic;
using Core.Helpers;

public class Unstablegas : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var card = targetCard.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n6") : CardDatabase.Instance.GetCardFromId("5om");
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.IsOwnedBy(OwnerEnum.Player)));
        EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(targetId.owner, card));
    }
}
