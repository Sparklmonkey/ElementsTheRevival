using System.Collections.Generic;

public class Evolve : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var card = targetCard.Id.IsUpgraded()
            ? CardDatabase.Instance.GetCardFromId("77h")
            : CardDatabase.Instance.GetCardFromId("591");
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, card, false));
    }
}
