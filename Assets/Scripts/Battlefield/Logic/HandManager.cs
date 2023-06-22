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

        public void UpdateHandVisual()
        {
            List<IDCardPair> handList = new(pairList);
            foreach (var card in pairList)
            {
                if (card.HasCard())
                {
                    card.RemoveCard();
                }
            }

            foreach (var card in handList)
            {
                AddCardToHand(card.card);
            }
        }

        public void AddCardToHand(Card card)
        {
            int availableIndex = pairList.FindIndex(x => !x.HasCard());
            pairList[availableIndex].PlayCard(card);
        }
    }
}