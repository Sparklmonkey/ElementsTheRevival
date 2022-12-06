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
            if (deck.Count == 0) { return null; }
            Card newCard = new Card(deck[0]);
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
            if(deck.Count == 0) { return null; }
            return deck[0];
        }
    }
}