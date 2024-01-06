using System.Collections.Generic;
using UnityEngine.Assertions.Must;

public class Bravery : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var cardToDraw = Owner.playerPassiveManager.GetMark().Item2.costElement.Equals(Element.Fire) ? 3 : 2;
        for (var i = 0; i < cardToDraw; i++)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner.Owner));
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner.Owner.Not()));
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;

    public override TargetPriority GetPriority() => TargetPriority.Any;
}
