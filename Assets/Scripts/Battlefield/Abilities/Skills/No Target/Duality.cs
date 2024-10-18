using System.Collections.Generic;

public class Duality : ActivatedAbility
{
    public override bool NeedsTarget() => false; 
    public override bool IsCardValid(ID id, Card card)
    {
        return id.Equals(
            new ID(BattleVars.Shared.AbilityIDOrigin.owner, FieldEnum.Player, 0));
    }
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var cardToAdd = DuelManager.Instance.GetNotIDOwner(targetId).DeckManager.GetTopCard();
        if (cardToAdd == null) { return; }
        EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(BattleVars.Shared.AbilityIDOrigin.owner, cardToAdd.Clone()));
    }
}
