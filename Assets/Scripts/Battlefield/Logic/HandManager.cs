using System;
using System.Collections.Generic;

namespace Elements.Duel.Manager
{

    [Serializable]
    public class HandManager : FieldManager
    {
        private bool _isPlayer;
        private EventBinding<AddCardToHandEvent> _addCardToHandBinding;
        private EventBinding<RemoveCardFromHandEvent> _removeCardFromHandBinding;
    
        public void OnDisable() {
            EventBus<AddCardToHandEvent>.Unregister(_addCardToHandBinding);
            EventBus<RemoveCardFromHandEvent>.Unregister(_removeCardFromHandBinding);
        }
        public void SetIsPlayer(bool isPlayer)
        {
            _isPlayer = isPlayer;
            _addCardToHandBinding = new EventBinding<AddCardToHandEvent>(AddCardToHand);
            EventBus<AddCardToHandEvent>.Register(_addCardToHandBinding);
            _removeCardFromHandBinding = new EventBinding<RemoveCardFromHandEvent>(RemoveCardFromHand);
            EventBus<RemoveCardFromHandEvent>.Register(_removeCardFromHandBinding);
        }
        public bool ShouldDiscard() => PairList.FindAll(x => x.card is not null).Count >= 8;

        private void RemoveCardFromHand(RemoveCardFromHandEvent removeCardFromHandEvent)
        {
            if (removeCardFromHandEvent.IsPlayer != _isPlayer)
            {
                return;
            }

            var index = PairList.FindIndex(x => x.id.Equals(removeCardFromHandEvent.CardToRemove));
            EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(PairList[index].id));

            var cardList = new List<Card>();
            foreach (var item in PairList)
            {
                if (!item.HasCard()) { continue; }
                cardList.Add(CardDatabase.Instance.GetCardFromId(item.card.iD));
            }

            for (var i = 0; i < 8; i++)
            {
                if (i < cardList.Count)
                {
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[i].id, cardList[i]));
                    continue;
                }
                if (PairList[i].IsActive())
                {
                    EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(PairList[i].id));
                }
            }
        }

        private void AddCardToHand(AddCardToHandEvent addCardToHandEvent)
        {
            if (addCardToHandEvent.IsPlayer != _isPlayer)
            {
                return;
            }
            var availableIndex = PairList.FindIndex(x => !x.HasCard());
            if (availableIndex < 0) { return; }
            EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[availableIndex].id, addCardToHandEvent.CardToAdd));
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