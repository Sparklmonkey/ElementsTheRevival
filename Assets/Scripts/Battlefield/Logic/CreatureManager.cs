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

    public void PlayCreature(Card card)
    {
        var index = 0;
        if (DuelManager.FloodCount > 0 && !card.costElement.Equals(Element.Other) && !card.costElement.Equals(Element.Water))
        {
            for (var i = 0; i < _safeZones.Count; i++)
            {
                var orderIndex = _safeZones[i];
                if (PairList[orderIndex].HasCard()) {continue;}
                PairList[orderIndex].PlayCard(card);
                index = orderIndex;
                break;
            }
        }
        else
        {
            foreach (int orderIndex in _creatureCardOrder)
            {
                if (!PairList[orderIndex].HasCard())
                {
                    PairList[orderIndex].PlayCard(card);
                    index = orderIndex;
                    break;
                }

            }
        }
        if (PairList[index].card.costElement.Equals(Element.Darkness) 
            || PairList[index].card.costElement.Equals(Element.Death))
        {
            if (DuelManager.Instance.GetCardCount(new() { "7ta" }) > 0)
            {
                PairList[index].card.DefModify += 1;
                PairList[index].card.AtkModify += 2;
            }
            else if (DuelManager.Instance.GetCardCount(new() { "5uq" }) > 0)
            {
                PairList[index].card.DefModify += 1;
                PairList[index].card.AtkModify += 1;
            }
            PairList[index].UpdateCard();
        }
    }

    internal void ClearField()
    {
        foreach (var pair in PairList)
        {
            pair.card = null;
        }
    }
}
