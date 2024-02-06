using System.Collections.Generic;
using UnityEngine;

public class Serendipity : ActivatedAbility
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
        var typeToAdd = ExtensionMethods.GetSerendipityWeighted();
        var elementToAdd = Element.Entropy;

        for (var i = 0; i < 3; i++)
        {
            var cardToAdd =
                CardDatabase.Instance.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd,
                    BattleVars.Shared.AbilityCardOrigin.iD.IsUpgraded());
            EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(targetId.owner, new(cardToAdd)));
            typeToAdd = ExtensionMethods.GetSerendipityWeighted();
            elementToAdd = (Element)Random.Range(0, 12);

            if (typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
            {
                typeToAdd = CardType.Creature;
                elementToAdd = Element.Death;
            }
        }
    }
}
