using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Duality : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var cardToAdd = DuelManager.GetNotIDOwner(target.id).deckManager.GetTopCard();
        if(cardToAdd == null) { return; }
        Owner.playerHand.AddCardToHand(new(cardToAdd));
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}
