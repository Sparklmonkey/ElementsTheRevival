using System.Collections.Generic;
using UnityEngine;

public class Serendipity : AbilityEffect
{
    public override bool NeedsTarget() => false;
    public override bool IsCardValid(ID id, Card card) => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        var typeToAdd = ExtensionMethods.GetSerendipityWeighted();
        var elementToAdd = Element.Entropy;

        for (var i = 0; i < 3; i++)
        {
            var cardToAdd =
                CardDatabase.Instance.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd,
                    targetCard.iD.IsUpgraded());
            EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(Owner.Owner, new(cardToAdd)));
            typeToAdd = ExtensionMethods.GetSerendipityWeighted();
            elementToAdd = (Element)Random.Range(0, 12);

            if (typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
            {
                typeToAdd = CardType.Creature;
                elementToAdd = Element.Death;
            }
        }
    }

    public override List<(ID, Card)> GetPossibleTargets(PlayerManager enemy) => new List<(ID, Card)>();

    public override (ID, Card) SelectRandomTarget(List<(ID, Card)> possibleTargets) => default;
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
