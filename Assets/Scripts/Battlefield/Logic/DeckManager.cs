using System;
using System.Collections.Generic;

namespace Elements.Duel.Manager
{

    public class DeckManager
    {
        private List<Card> _deck;

        public DeckManager(List<Card> deck) => this._deck = deck;

        public event Action<int> OnDeckCountChange;

        public Card DrawCard()
        {
            if (_deck.Count == 0) { return null; }
            Card newCard = new(_deck[0]);
            _deck.RemoveAt(0);
            OnDeckCountChange?.Invoke(_deck.Count);
            return newCard;
        }

        public int GetDeckCount() => _deck.Count;

        public void AddCardToTop(Card card)
        {
            _deck.Insert(0, card);
            OnDeckCountChange?.Invoke(_deck.Count);
        }

        public Card GetTopCard()
        {
            if (_deck.Count == 0) { return null; }
            return _deck[0];
        }
    }
}