using System.Collections.Generic;
using UnityEngine;

public class Serendipity : AbilityEffect
{
    public override bool NeedsTarget() => false;

    public override void Activate(IDCardPair target)
    {
        CardType typeToAdd = ExtensionMethods.GetSerendipityWeighted();
        Element elementToAdd = Element.Entropy;

        for (int i = 0; i < 3; i++)
        {
            Owner.playerHand.AddCardToHand(CardDatabase.Instance.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd, target.card.iD.IsUpgraded()));

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

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        return null;
    }
}
