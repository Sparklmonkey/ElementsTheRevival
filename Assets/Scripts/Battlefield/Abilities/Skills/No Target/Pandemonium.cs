using System.Collections.Generic;
using System.Linq;

public class Pandemonium : ActivatedAbility
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card)
    {
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var chaos = new Chaos();
        chaos.Activate(targetId, targetCard);
    }
}