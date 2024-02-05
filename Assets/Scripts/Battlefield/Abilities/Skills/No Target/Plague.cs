using System.Collections.Generic;
using System.Linq;
using Core.Helpers;

public class Plague : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override bool IsCardValid(ID id, Card card)
    {
        return !id.IsOwnedBy(BattleVars.Shared.AbilityIDOrigin.owner) && id.IsCreatureField();
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;

        targetCard.Poison += 1;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
        
        if (BattleVars.Shared.AbilityCardOrigin.cardType.Equals(CardType.Creature))
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(BattleVars.Shared.AbilityIDOrigin));
        }
    }
}
