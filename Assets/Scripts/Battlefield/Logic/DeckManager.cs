using System.Collections.Generic;

namespace Elements.Duel.Manager
{

    public class DeckManager
    {
        private List<Card> _deck;
        private bool _isPlayer;
        
        private EventBinding<DrawCardFromDeckEvent> _drawCardFromDeckBinding;
    
        public void OnDisable() {
            EventBus<DrawCardFromDeckEvent>.Unregister(_drawCardFromDeckBinding);
        }
        public DeckManager(List<Card> deck, bool isPlayer)
        {
            _isPlayer = isPlayer;
            _deck = deck;
            _drawCardFromDeckBinding = new EventBinding<DrawCardFromDeckEvent>(DrawCard);
            EventBus<DrawCardFromDeckEvent>.Register(_drawCardFromDeckBinding);
        }


        private void DrawCard(DrawCardFromDeckEvent drawCardFromDeckEvent)
        {
            if (drawCardFromDeckEvent.IsPlayer != _isPlayer)
            {
                return;
            }
            if (_deck.Count == 0)
            {
                EventBus<GameEndEvent>.Raise(new GameEndEvent(_isPlayer));
                return;
            }
            Card newCard = new(_deck[0]);
            _deck.RemoveAt(0);
            EventBus<AddDrawCardActionEvent>.Raise(new AddDrawCardActionEvent(newCard, _isPlayer));
            EventBus<DeckCountChangeEvent>.Raise(new DeckCountChangeEvent(_deck.Count, _isPlayer));
            EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(_isPlayer, newCard));
        }

        public int GetDeckCount() => _deck.Count;

        public void AddCardToTop(Card card)
        {
            _deck.Insert(0, card);
            EventBus<DeckCountChangeEvent>.Raise(new DeckCountChangeEvent(_deck.Count, _isPlayer));
        }

        public Card GetTopCard()
        {
            if (_deck.Count == 0) { return null; }
            return _deck[0];
        }
    }
}