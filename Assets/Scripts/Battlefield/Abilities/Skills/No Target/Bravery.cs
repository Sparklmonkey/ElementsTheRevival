using System.Collections.Generic;
using UnityEngine.Assertions.Must;

public class Bravery : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var cardToDraw = DuelManager.Instance.GetIDOwner(targetId).playerPassiveManager.GetMark().Item2.costElement.Equals(Element.Fire) ? 3 : 2;
        for (var i = 0; i < cardToDraw; i++)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(targetId.owner));
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(targetId.owner.Not()));
        }
    }
}
