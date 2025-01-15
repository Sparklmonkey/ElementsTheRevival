using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battlefield.Abstract;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public delegate bool QuantaCheck(Element element, int amount);

    [Serializable]
    public class HandManager : CardManager
    {
        private EventBinding<UpdateHandDisplayEvent> _updateHandDisplayBinding;
        private EventBinding<AddCardToHandEvent> _playHandCardBinding;

        private void OnDisable()
        {
            EventBus<UpdateHandDisplayEvent>.Unregister(_updateHandDisplayBinding);
            EventBus<AddCardToHandEvent>.Unregister(_playHandCardBinding);
        }

        private void OnEnable()
        {
            _updateHandDisplayBinding = new EventBinding<UpdateHandDisplayEvent>(UpdateHandCards);
            EventBus<UpdateHandDisplayEvent>.Register(_updateHandDisplayBinding);
            
            _playHandCardBinding = new EventBinding<AddCardToHandEvent>(AddCardToHand);
            EventBus<AddCardToHandEvent>.Register(_playHandCardBinding);
        }
        
        public bool ShouldDiscard() => GetAllValidCards().Count > 7;
        
        public int GetHandCount() => GetAllValidCards().Count;

        public List<(ID id, Card card)> GetPlayableCards(QuantaCheck quantaCheck)
        {
            var returnList = GetAllValidCardIds();
            return returnList.Where(x => quantaCheck(x.card.CostElement, x.card.Cost)).ToList();
        }
        
        public bool HasCardOfType(CardType cardType)
        {
            var returnList = GetAllValidCardIds();
            return returnList.Exists(x => x.card.Type == cardType);
        }

        public List<(ID id, Card card)> GetPlayableCardsOfType(QuantaCheck quantaCheck, CardType cardType)
        {
            var returnList = GetAllValidCardIds();
            return returnList.Where(x => quantaCheck(x.card.CostElement, x.card.Cost) && x.card.Type == cardType).ToList();
        }
        
        private IEnumerator MoveCardPosition()
        {
            yield return null;
            for (var j = 0; j < cardPositions.Count; j++)
            {
                if (cardPositions[j].childCount != 0) continue;
                for (var i = j + 1; i < cardPositions.Count; i++)
                {
                    if (cardPositions[i].childCount == 0) continue;
                    cardPositions[i].GetChild(0).SetParent(cardPositions[j], false);
                    cardPositions[j].GetChild(0).position = cardPositions[j].position;
                    cardPositions[j].GetChild(0).GetComponent<HandCardDisplay>().SetupId(new ID(owner, field, j));
                    break;
                }
            }

            ResetCardFieldDisplays();
        }
        
        private void UpdateHandCards(UpdateHandDisplayEvent updateHandDisplayEvent)
        {
            if (!updateHandDisplayEvent.Owner.Equals(owner)) return;
            StartCoroutine(MoveCardPosition());
        }

        private void AddCardToHand(AddCardToHandEvent addCardToHandEvent)
        {
            if (!addCardToHandEvent.Owner.Equals(owner)) return;
            var index = GetNextAvailablePosition();
            if (index == -1) return;
            var id = new ID(owner, field, index);
            
            InstantiateCardObject(id);

            EventBus<UpdateCardDisplayEvent>.Raise(new UpdateCardDisplayEvent(id, addCardToHandEvent.CardToAdd, 0,
                false));
        }

        private int GetNextAvailablePosition() => cardPositions.FindIndex(x => x.childCount == 0);
        
        public void ShowCardsForPrecog()
        {
            foreach (var cardFieldDisplay in _cardFieldDisplays)
            {
                EventBus<UpdatePrecogEvent>.Raise(new UpdatePrecogEvent(cardFieldDisplay.Key));
            }
        }
    }
}