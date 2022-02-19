using System.Collections.Generic;
using UnityEngine;

namespace Elements.Duel.Manager
{

    public class DeckManager
    {
        private List<Card> deck;

        public DeckManager(List<Card> deck) => this.deck = deck;

        public Card DrawCard()
        {
            Card toReturn = deck[0];
            Card newCard = Object.Instantiate(toReturn);
            newCard.name = toReturn.name;
            deck.RemoveAt(0);
            return newCard;
        }

        public int GetDeckCount()
        {
            return deck.Count;
        }

        public void AddCardToTop(Card card)
        {
            deck.Insert(0, card);
        }

        public Card GetTopCard()
        {
            return deck[0];
        }
    }
}