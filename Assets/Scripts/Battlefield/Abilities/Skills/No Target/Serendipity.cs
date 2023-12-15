using System.Collections.Generic;
using UnityEngine;

public class Serendipity : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        var typeToAdd = ExtensionMethods.GetSerendipityWeighted();
        var elementToAdd = Element.Entropy;

        for (var i = 0; i < 3; i++)
        {
            var cardToAdd =
                CardDatabase.Instance.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd,
                    target.card.iD.IsUpgraded());
            EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(Owner.isPlayer, new(cardToAdd)));
            typeToAdd = ExtensionMethods.GetSerendipityWeighted();
            elementToAdd = (Element)Random.Range(0, 12);

            if (typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
            {
                typeToAdd = CardType.Creature;
                elementToAdd = Element.Death;
            }
        }
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        return new();
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        return null;
    }
    public override TargetPriority GetPriority() => TargetPriority.Any;
}
