using System.Collections.Generic;

public class Rebirth : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var card = targetCard.Id.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("7ds")
            : CardDatabase.Instance.GetCardFromId("5fc");
        
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, card, false));
    }
}
