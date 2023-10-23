using System;
using System.Collections.Generic;
using System.Linq;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PermanentManager : FieldManager
    {
        public PermanentManager(List<ID> idList)
        {
        }

        public void PlayPermanent(Card card)
        {
            if (card.cardType.Equals(CardType.Pillar))
            {
                var stackedCard = PairList.FirstOrDefault(idCard => idCard.HasCard() && idCard.card.iD == card.iD);
                if (stackedCard is not null)
                {
                    stackedCard.PlayCard(card);
                    return;
                }
            }

            List<int> permanentCardOrder = new() { 1, 3, 5, 7, 0, 2, 4, 6, 9, 11, 13, 8, 10, 12 };

            foreach (int orderIndex in permanentCardOrder)
            {
                if (!PairList[orderIndex].HasCard())
                {
                    if (StackCountList.Count > 0)
                    {
                        StackCountList[orderIndex]++;
                    }
                    PairList[orderIndex].PlayCard(card);
                    break;
                }
            }
        }

        public List<(QuantaObject, ID)> GetQuantaToGenerate()
        {
            List<(QuantaObject, ID)> listToReturn = new List<(QuantaObject, ID)>();
            //List<QuantaObject> listToReturn = new List<QuantaObject>();
            //List<ID> ids = new List<ID>();
            for (int i = 0; i < PairList.Count; i++)
            {
                if (PairList[i].card is not null)
                {
                    var card = PairList[i].card;

                    var elementnow = card.costElement;
                    Element mark;
                    bool isPlayer = PairList[i].id.owner.Equals(OwnerEnum.Player);
                    mark = isPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
                    if (card.skill == " " && elementnow == card.skillElement)
                    {
                        QuantaObject quantaObject = new(elementnow, StackCountList[i]);
                        listToReturn.Add((quantaObject, PairList[i].id));
                        card.skillElement = mark;
                    }
                    else if (card.skill == " " && elementnow != card.skillElement)
                    {
                        QuantaObject quantaObject = new(card.skillElement, StackCountList[i]);
                        listToReturn.Add((quantaObject, PairList[i].id));
                        card.skillElement = card.costElement;
                    }
                    else
                    {
                        QuantaObject quantaObject = new(card.costElement, StackCountList[i]);
                        listToReturn.Add((quantaObject, PairList[i].id));
                    }
                }
            }

            return listToReturn;
        }

        public List<int> GetIndexToAnimate()
        {
            List<int> listToReturn = new();

            foreach (IDCardPair item in PairList)
            {
                if (item.HasCard())
                {
                    if (item.card.cardType.Equals(CardType.Pillar))
                    {
                        listToReturn.Add(item.id.index);
                    }
                }
            }

            return listToReturn;
        }

        public int GetStackAt(int index)
        {
            return StackCountList[index];
        }

        public void PermanentTurnDown()
        {
            foreach (var idCard in PairList)
            {
                if (!idCard.HasCard()) { continue; }
                idCard.cardBehaviour.OnTurnStart();
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

}