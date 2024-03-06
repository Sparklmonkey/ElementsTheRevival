using System.Collections.Generic;

public class Hatch : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var cardToPlay = CardDatabase.Instance.GetRandomCard(CardType.Creature, targetCard.Id.IsUpgraded(), true);
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, cardToPlay, false));
    }
}
