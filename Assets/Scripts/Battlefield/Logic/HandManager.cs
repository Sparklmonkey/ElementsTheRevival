﻿using System;
using System.Collections.Generic;
using Elements.Duel.Visual;
using UnityEngine;

namespace Elements.Duel.Manager
{

    [Serializable]
    public class HandManager : FieldManager
    {
        private List<PlayerCardInHand> handDisplayers;

        public HandManager(List<ID> idList, List<PlayerCardInHand> handDisplayers)
        {
            this.handDisplayers = handDisplayers;
            pairList = new List<IDCardPair>();
            for (int i = 0; i < idList.Count; i++)
            {
                pairList.Add(new IDCardPair(idList[i], null));
                pairList[i].OnCardChanged += handDisplayers[i].DisplayCard;
            }
        }

        public bool ShouldDiscard() => pairList.FindAll(x => x.card != null).Count >= 8;

        public void PlayCardWithID(ID id)
        {
            pairList.Find(x => x.id == id).RemoveCard();

            //Update Hand To Remove Empty Space
            foreach (var item in handDisplayers)
            {
                item.transform.parent.gameObject.SetActive(false);
            }
            List<Card> list = GetAllCards();

            ClearAllCards();
            int handIndex = 0;
            foreach (Card item in list)
            {
                pairList[handIndex].PlayCard(item);
                handIndex++;
            }
        }

        public void AddCardToHand(Card card)
        {
            int availableIndex = pairList.FindIndex(x => x.card == null);
            pairList[availableIndex].PlayCard(card);
        }
    }
}