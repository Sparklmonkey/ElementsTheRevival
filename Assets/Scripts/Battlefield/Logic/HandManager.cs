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
            if (pairList.FindAll(x => x.HasCard()).Count == 0)
            {
                return;
            }

            var cardList = new List<Card>();
            foreach (var item in pairList)
            {
                if (!item.HasCard()) { continue; }
                if (item.id.Index == cardPair.id.Index) { continue; }
                cardList.Add(CardDatabase.Instance.GetCardFromId(item.card.iD));
            }

            for (int i = 0; i < 8; i++)
            {
                if(i < cardList.Count)
                {
                    pairList[i].PlayCard(cardList[i]);
                    continue;
                }
                if (pairList[i].IsActive())
                {
                    pairList[i].RemoveCard();
                }
            }
        }

        public void AddCardToHand(Card card)
        {
            int availableIndex = pairList.FindIndex(x => !x.HasCard());
            if (availableIndex < 0) { return; }
            pairList[availableIndex].PlayCard(card);
        }

        public void ShowCardsForPrecog()
        {
            foreach (var item in pairList)
            {
                if (!item.HasCard()) { continue; }
                item.isHidden = false;
                item.UpdateCard();
            }
        }
    }
}