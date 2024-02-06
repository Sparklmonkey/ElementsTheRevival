using System.Collections.Generic;
using Core.Helpers;

public class Scarab : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var card = targetCard.iD.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("7qa")
            : CardDatabase.Instance.GetCardFromId("5rq");
        
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, targetId.IsOwnedBy(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, card));
    }
    
}
