using System.Collections.Generic;
using Core.Helpers;

public class Queen : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var card = targetCard.Id.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("7n4")
            : CardDatabase.Instance.GetCardFromId("5ok");
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.IsOwnedBy(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, card));
    }
}
