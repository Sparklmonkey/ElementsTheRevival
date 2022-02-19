using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elements.Duel.Manager
{

    [Serializable]
    public class HandManager : FieldManager
    {
        public HandManager(List<ID> idList)
        {
            pairList = new List<IDCardPair>();
            for (int i = 0; i < idList.Count; i++)
            {
                pairList.Add(new IDCardPair(idList[i], null));
            }
        }

        public void AddObjectToHand(ID id, Card card) => pairList.Add(new IDCardPair(id, card));

        public void PlayCardWithID(ID id) => pairList[id.Index].card = null;

        public ID AddCardToHand(Card card)
        {
            foreach (IDCardPair hand in pairList)
            {
                if (hand.card == null)
                {
                    hand.card = card;
                    return hand.id;
                }
            }
            return null;
        }



        public void UpdateHand()
        {
            List<Card> list = GetAllCards();
            ClearAllCards();
            int handIndex = 0;
            foreach (Card item in list)
            {
                pairList[handIndex].card = item;
                handIndex++;
            }
        }
    }
}