using System.Collections.Generic;

namespace Elements.Duel.Manager
{

    public class DeckManager
    {
        private List<Card> _deck;
        private OwnerEnum _owner;
        
        private EventBinding<DrawCardFromDeckEvent> _drawCardFromDeckBinding;
    
        public void OnDisable() {
            EventBus<DrawCardFromDeckEvent>.Unregister(_drawCardFromDeckBinding);
        }
        public DeckManager(List<Card> deck, OwnerEnum owner)
        {
            _owner = owner;
            _deck = deck;
            _drawCardFromDeckBinding = new EventBinding<DrawCardFromDeckEvent>(DrawCard);
            EventBus<DrawCardFromDeckEvent>.Register(_drawCardFromDeckBinding);
        }


        private void DrawCard(DrawCardFromDeckEvent drawCardFromDeckEvent)
        {
            if (!drawCardFromDeckEvent.Owner.Equals(_owner)) return;
            if (_deck.Count == 0)
            {
                EventBus<GameEndEvent>.Raise(new GameEndEvent(_owner));
                return;
            }
            Card newCard = new(_deck[0]);
            _deck.RemoveAt(0);
            EventBus<AddDrawCardActionEvent>.Raise(new AddDrawCardActionEvent(newCard, _owner));
            EventBus<DeckCountChangeEvent>.Raise(new DeckCountChangeEvent(_deck.Count, _owner));
            EventBus<AddCardToHandEvent>.Raise(new AddCardToHandEvent(_owner, newCard));
        }

        public int GetDeckCount() => _deck.Count;

        public void AddCardToTop(Card card)
        {
            _deck.Insert(0, card);
            EventBus<DeckCountChangeEvent>.Raise(new DeckCountChangeEvent(_deck.Count, _owner));
        }

        public Card GetTopCard()
        {
            if (_deck.Count == 0) { return null; }
            return _deck[0];
        }
    }
}