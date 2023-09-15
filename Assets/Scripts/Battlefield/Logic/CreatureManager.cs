using System;
using System.Collections.Generic;

[Serializable]
public class CreatureManager : FieldManager
{
    private readonly List<int> _creatureCardOrder = new() { 11, 13, 9, 10, 12, 14, 8, 16, 18, 20, 22, 0, 2, 4, 6, 15, 17, 19, 21, 1, 3, 5, 7 };
    private readonly List<int> _safeZones = new() { 11, 13, 10, 12, 14 };

    public List<IDCardPair> GetCreaturesWithGravity()
    {
        var idCardList = GetAllValidCardIds();
        if (idCardList.Count == 0) { return new List<IDCardPair>(); }
        return idCardList.FindAll(x => x.card.passiveSkills.GravityPull);
    }

    public void CreatureTurnDown()
    {
        foreach (var idCard in PairList)
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
        if (DuelManager.FloodCount > 0 && !card.costElement.Equals(Element.Other) && !card.costElement.Equals(Element.Water))
        {

            foreach (int orderIndex in _safeZones)
            {
                if (!PairList[orderIndex].HasCard())
                {
                    PairList[orderIndex].PlayCard(card);
                    return PairList[orderIndex];
                }
            }
        }

        foreach (int orderIndex in _creatureCardOrder)
        {
            if (!PairList[orderIndex].HasCard())
            {
                PairList[orderIndex].PlayCard(card);
                return PairList[orderIndex];
            }

        }
        return null;
    }

    internal void ClearField()
    {
        foreach (var pair in PairList)
        {
            pair.card = null;
        }
    }
}
