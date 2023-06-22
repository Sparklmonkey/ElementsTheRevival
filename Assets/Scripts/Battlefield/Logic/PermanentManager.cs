using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PermanentManager : FieldManager
    {
        public PermanentManager(List<ID> idList)
        {
        }

        public IDCardPair PlayPermanent(Card card)
        {
            if (card.cardType.Equals(CardType.Pillar))
            {
                IDCardPair stackedCard = pairList.Find(x => x.card.iD == card.iD);
                if (stackedCard != null)
                {
                    stackedCard.PlayCard(card);
                    return stackedCard;
                }
            }
            List<int> permanentCardOrder = new() { 1, 3, 5, 7, 0, 2, 4, 6, 9, 11, 13, 8, 10, 12, 14 };

            foreach (int orderIndex in permanentCardOrder)
            {
                if (!pairList[orderIndex].HasCard())
                {
                    if (stackCountList.Count > 0)
                    {
                        stackCountList[orderIndex]++;
                    }
                    pairList[orderIndex].PlayCard(card);
                    return pairList[orderIndex];
                }
            }
            return null;
        }

        public List<(QuantaObject, ID)> GetQuantaToGenerate()
        {
            List<(QuantaObject, ID)> listToReturn = new List<(QuantaObject, ID)>();
            //List<QuantaObject> listToReturn = new List<QuantaObject>();
            //List<ID> ids = new List<ID>();
            for (int i = 0; i < pairList.Count; i++)
            {
                if (pairList[i].card != null)
                {
                    Card card = pairList[i].card;

                    Element elementnow = card.costElement;
                    Element mark;
                    bool isPlayer = pairList[i].id.Owner.Equals(OwnerEnum.Player);
                    mark = isPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
                    if (card.skill == " " && elementnow == card.skillElement)
                    {
                        QuantaObject quantaObject = new (elementnow, stackCountList[i]);
                        listToReturn.Add((quantaObject, pairList[i].id));
                        card.skillElement = mark;
                    }
                    else if (card.skill == " " && elementnow != card.skillElement)
                    {
                        QuantaObject quantaObject = new (card.skillElement, stackCountList[i]);
                        listToReturn.Add((quantaObject, pairList[i].id));
                        card.skillElement = card.costElement;
                    }
                    else
                    {
                        QuantaObject quantaObject = new (card.costElement, stackCountList[i]);
                        listToReturn.Add((quantaObject, pairList[i].id));
                    }
                }
            }

            return listToReturn;
        }

        public List<int> GetIndexToAnimate()
        {
            List<int> listToReturn = new();

            foreach (IDCardPair item in pairList)
            {
                if (item.HasCard())
                {
                    if (item.card.cardType.Equals(CardType.Pillar))
                    {
                        listToReturn.Add(item.id.Index);
                    }
                }
            }

            return listToReturn;
        }

        public int GetStackAt(int index)
        {
            return stackCountList[index];
        }

        public void PermanentTurnDown()
        {
            List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
            foreach (var idCard in pairList)
            {
                if(!idCard.HasCard()) { continue; }
                idCard.cardBehaviour.OnTurnStart();
                idCard.UpdateCard();

            }
        }
    }

}