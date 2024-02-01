using System;
using System.Collections.Generic;
using System.Linq;
using Battlefield.Abstract;
using UnityEngine;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PermanentManager : CardManager
    {
        private readonly List<int> _permanentCardOrder = new() { 1, 3, 5, 7, 0, 2, 4, 6, 9, 11, 13, 8, 10, 12 };
        
        private EventBinding<PlayPermanentOnFieldEvent> _playPermanentBinding;

        private void OnDisable()
        {
            EventBus<PlayPermanentOnFieldEvent>.Unregister(_playPermanentBinding);
        }

        private void OnEnable()
        {
            _playPermanentBinding = new EventBinding<PlayPermanentOnFieldEvent>(PlayPermanent);
            EventBus<PlayPermanentOnFieldEvent>.Register(_playPermanentBinding);
        }

        private void TowerCheck(Card card)
        {
            if (card.iD.IsUpgraded())
            {
                EventBus<QuantaChangeLogicEvent>.Raise(
                    card.costElement.Equals(Element.Other)
                        ? new QuantaChangeLogicEvent(3, card.costElement, owner,
                            true)
                        : new QuantaChangeLogicEvent(1, card.costElement, owner,
                            true));
            }
        }
        
        public int GetStackCountForId(ID id) => ((PermanentCardDisplay)_cardFieldDisplays[id]).StackCountValue;

        public void PlayPermanent(PlayPermanentOnFieldEvent playCardOnFieldEvent)
        {
            if (!playCardOnFieldEvent.Owner.Equals(owner)) return;

            if (IsStacked(playCardOnFieldEvent.CardToPlay)) return;
            
            foreach (var orderIndex in _permanentCardOrder)
            {
                if (cardPositions[orderIndex].childCount != 0) continue;
                var id = new ID(owner, field, orderIndex);
                InstantiateCardObject(id);
                EventBus<UpdatePermanentCardEvent>.Raise(new UpdatePermanentCardEvent(id, playCardOnFieldEvent.CardToPlay));
                break;
            }
        }

        private bool IsStacked(Card card)
        {
            if (!card.cardType.Equals(CardType.Pillar)) return false;
            TowerCheck(card);
            
            if (!cardPositions.Exists(t => t.childCount > 0)) return false;
            
            var filteredList = cardPositions.FindAll(t => t.childCount > 0);
            if (filteredList.Count <= 0) return false;
            
            var stackedCard = filteredList.FirstOrDefault(t =>
                    t.GetComponentInChildren<PermanentCardDisplay>().Card.iD ==
                    card.iD);
            if (stackedCard is null) return false;
            EventBus<UpdatePermanentCardEvent>.Raise(new UpdatePermanentCardEvent(
                    stackedCard.GetComponentInChildren<PermanentCardDisplay>().Id,
                    card));
            return true;
        }
    }
}