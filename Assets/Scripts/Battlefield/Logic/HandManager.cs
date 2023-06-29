using System;
using System.Collections.Generic;
using Elements.Duel.Visual;
using UnityEngine;

namespace Elements.Duel.Manager
{

    [Serializable]
    public class HandManager : FieldManager
    {
        public bool ShouldDiscard() => pairList.FindAll(x => x.card != null).Count >= 8;

        public void UpdateHandVisual(IDCardPair cardPair)
        {
            cardPair.card = null;
            if (pairList.FindAll(x => x.HasCard()).Count == 0)
            {
                cardPair.RemoveCard();
            }

            var cardList = new List<Card>();
            foreach (var item in pairList)
            {
                if (!item.HasCard()) { continue; }
                cardList.Add(CardDatabase.Instance.GetCardFromId(item.card.iD));
            }

            for (int i = 0; i < pairList.Count; i++)
            {
                if (i >= cardList.Count)
                {
                    if (pairList[i].transform.parent.gameObject.activeSelf)
                    {
                        pairList[i].RemoveCard();
                    }
                    continue;
                }
                pairList[i].PlayCard(cardList[i]);
            }
        }

        public void AddCardToHand(Card card)
        {
            int availableIndex = pairList.FindIndex(x => !x.HasCard());
            if (availableIndex < 0) { return; }
            pairList[availableIndex].PlayCard(card);
        }
    }
}