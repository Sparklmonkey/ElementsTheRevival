﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PermanentManager : FieldManager
    {
        private readonly List<int> permanentCardOrder = new(){ 1, 3, 5, 7, 0, 2, 4, 6, 9, 11, 13, 8, 10, 12, 14 };
        private readonly List<string> permanentsWithCountdown = new(){ "7q9", "5rp", "5v2", "7ti" };

        public PermanentManager(List<ID> idList)
        {
            pairList = new List<IDCardPair>();
            for (int i = 0; i < idList.Count; i++)
            {
                stackCountList.Add(0);
                pairList.Add(new IDCardPair(idList[i], null));
            }
        }

        public ID PlayPermanent(Card card)
        {
            if (card.cardType.Equals(CardType.Pillar))
            {
                IDCardPair stackedCard = pairList.Find(x => x.card.iD == card.iD);
                if (stackedCard != null)
                {
                    stackedCard.PlayCard(card);
                    return stackedCard.id;
                }
            }

            foreach (int orderIndex in permanentCardOrder)
            {
                if (!pairList[orderIndex].HasCard())
                {
                    if (stackCountList.Count > 0)
                    {
                        stackCountList[orderIndex]++;
                    }
                    pairList[orderIndex].PlayCard(card);
                    return pairList[orderIndex].id;
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
                        QuantaObject quantaObject = new QuantaObject(elementnow, stackCountList[i]);
                        listToReturn.Add((quantaObject, pairList[i].id));
                        card.skillElement = mark;
                    }
                    else if (card.skill == " " && elementnow != card.skillElement)
                    {
                        QuantaObject quantaObject = new QuantaObject(card.skillElement, stackCountList[i]);
                        listToReturn.Add((quantaObject, pairList[i].id));
                        card.skillElement = card.costElement;
                    }
                    else
                    {
                        QuantaObject quantaObject = new QuantaObject(card.costElement, stackCountList[i]);
                        listToReturn.Add((quantaObject, pairList[i].id));
                    }
                }
            }

            return listToReturn;
        }

        public List<int> GetIndexToAnimate()
        {
            List<int> listToReturn = new List<int>();

            foreach (IDCardPair item in pairList)
            {
                if (item.card != null)
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
            foreach (var idCard in pairList)
            {
                if(!idCard.HasCard()) { continue; }
                idCard.card.AbilityUsed = false;
                if (permanentsWithCountdown.Contains(idCard.card.iD))
                {
                    idCard.card.TurnsInPlay--;

                    if (idCard.card.TurnsInPlay == 0)
                    {
                        idCard.RemoveCard();
                        return;
                    }
                }
                idCard.UpdateCard();

            }
        }
    }

}