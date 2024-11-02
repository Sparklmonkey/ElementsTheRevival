using System.Collections.Generic;
using UnityEngine.Assertions.Must;

public class Bravery : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var opponent = DuelManager.Instance.GetNotIDOwner(targetId); 
        if (opponent.playerCounters.sanctuary > 0) return;

        var handCount = opponent.playerHand.GetHandCount();
        
        var cardToDraw = DuelManager.Instance.GetIDOwner(targetId).playerPassiveManager.GetMark().Item2.CostElement.Equals(Element.Fire) ? 3 : 2;

        while (cardToDraw > 0 && handCount < 8)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(targetId.owner));
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(targetId.owner.Not()));
            cardToDraw--;
            handCount++;
        }
    }
}
