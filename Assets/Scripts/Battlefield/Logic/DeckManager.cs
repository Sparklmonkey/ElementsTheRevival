using System.Collections.Generic;
using System;

namespace Elements.Duel.Manager
{

    public class DeckManager
    {
        private List<Card> deck;

        public DeckManager(List<Card> deck) => this.deck = deck;

        public event Action<int> OnDeckCountChange;

        public Card DrawCard()
        {
            if (deck.Count == 0) { return null; }
            Card newCard = new(deck[0]);
            deck.RemoveAt(0);
            OnDeckCountChange?.Invoke(deck.Count);
            return newCard;
        }

        public int GetDeckCount() => deck.Count;

        public void AddCardToTop(Card card)
        {
            deck.Insert(0, card);
            OnDeckCountChange?.Invoke(deck.Count);
        }

        public Card GetTopCard()
        {
            if(deck.Count == 0) { return null; }
            return deck[0];
        }
    }
}