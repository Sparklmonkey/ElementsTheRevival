using System;
using System.Collections.Generic;

namespace Elements.Duel.Manager
{

    [Serializable]
    public class HandManager : FieldManager
    {
        public bool ShouldDiscard() => PairList.FindAll(x => x.card != null).Count >= 8;

        public void UpdateHandVisual(IDCardPair cardPair)
        {
            if (PairList.FindAll(x => x.HasCard()).Count == 0)
            {
                return;
            }

            var cardList = new List<Card>();
            foreach (var item in PairList)
            {
                if (!item.HasCard()) { continue; }
                if (item.id.index == cardPair.id.index) { continue; }
                cardList.Add(CardDatabase.Instance.GetCardFromId(item.card.iD));
            }

            for (int i = 0; i < 8; i++)
            {
                if (i < cardList.Count)
                {
                    PairList[i].PlayCard(cardList[i]);
                    continue;
                }
                if (PairList[i].IsActive())
                {
                    PairList[i].RemoveCard();
                }
            }
        }

        public void AddCardToHand(Card card)
        {
            int availableIndex = PairList.FindIndex(x => !x.HasCard());
            if (availableIndex < 0) { return; }
            PairList[availableIndex].PlayCard(card);
        }

        public void ShowCardsForPrecog()
        {
            foreach (var item in PairList)
            {
                if (!item.HasCard()) { continue; }
                item.isHidden = false;
                item.UpdateCard();
            }
        }
    }
}