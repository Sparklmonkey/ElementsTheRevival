using System;
using System.Collections.Generic;
using System.Linq;
using Elements.Duel.Visual;
using UnityEngine;

[Serializable]
public class CreatureManager : FieldManager
{
    private readonly List<int> creatureCardOrder = new (){ 11, 13, 9, 10, 12, 14, 8, 16, 18, 20, 22, 0, 2, 4, 6, 15, 17, 19, 21, 1, 3, 5, 7 };
    private readonly List<int> safeZones = new (){ 11, 13, 10, 12, 14 };

    public List<IDCardPair> GetCreaturesWithGravity()
    {
        var idCardList = GetAllValidCardIds();
        if (idCardList.Count == 0) { return new List<IDCardPair>(); }
        return idCardList.FindAll(x => x.card.passive.Contains("gravity pull"));
    }

    public void CreatureTurnDown()
    {
        foreach (var idCard in pairList)
        {
            if (idCard.HasCard())
            {
                idCard.cardBehaviour.OnTurnStart();
                idCard.UpdateCard();
            }
        }

    }

    public IDCardPair PlayCreature(Card card)
    {
        if (DuelManager.floodCount > 0 && !card.costElement.Equals(Element.Other) && !card.costElement.Equals(Element.Water))
        {

            foreach (int orderIndex in safeZones)
            {
                if (!pairList[orderIndex].HasCard())
                {
                    pairList[orderIndex].PlayCard(card);
                    return pairList[orderIndex];
                }
            }
        }

        foreach (int orderIndex in creatureCardOrder)
        {
            if (!pairList[orderIndex].HasCard())
            {
                pairList[orderIndex].PlayCard(card);
                return pairList[orderIndex];
            }

        }
        return null;
    }

    internal void ClearField()
    {
        foreach (var pair in pairList)
        {
            pair.card = null;
        }
    }
}
