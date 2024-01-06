using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PermanentManager : MonoBehaviour
    {
        [SerializeField] private GameObject permanentPrefab;
        [SerializeField] private List<Transform> permanentPositions;

        private readonly List<int> _permanentCardOrder = new() { 1, 3, 5, 7, 0, 2, 4, 6, 9, 11, 13, 8, 10, 12 };
        private OwnerEnum _owner;

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

        public void SetOwner(OwnerEnum owner) => _owner = owner;

        public List<Card> GetAllValidCards()
        {
            var firstList = permanentPositions.FindAll(p => p.childCount > 0);
            return firstList.Select(permanent => permanent.GetComponentInChildren<PermanentCardDisplay>().Card)
                .ToList();
        }

        public List<(ID id, Card card)> GetAllValidCardIds()
        {
            var firstList = permanentPositions.FindAll(p => p.childCount > 0);
            var list = firstList.Select(permanent => permanent.GetComponentInChildren<PermanentCardDisplay>()).ToList();

            return list.Select(item => (item.Id, item.Card)).ToList();
        }

        private void TowerCheck(Card card)
        {
            if (card.iD.IsUpgraded())
            {
                EventBus<QuantaChangeLogicEvent>.Raise(
                    card.costElement.Equals(Element.Other)
                        ? new QuantaChangeLogicEvent(3, card.costElement, _owner,
                            true)
                        : new QuantaChangeLogicEvent(1, card.costElement, _owner,
                            true));
            }
        }

        public void PlayPermanent(PlayPermanentOnFieldEvent playCardOnFieldEvent)
        {
            if (!playCardOnFieldEvent.Owner.Equals(_owner)) return;

            if (IsStacked(playCardOnFieldEvent.CardToPlay)) return;
            
            foreach (var orderIndex in _permanentCardOrder)
            {
                if (permanentPositions[orderIndex].childCount != 0) continue;
                var id = new ID(_owner, FieldEnum.Permanent, orderIndex);
                var permanentCardObject = Instantiate(permanentPrefab, permanentPositions[orderIndex]);
                permanentCardObject.GetComponent<PermanentCardDisplay>().SetupId(id);

                EventBus<UpdatePermanentCardEvent>.Raise(new UpdatePermanentCardEvent(id, playCardOnFieldEvent.CardToPlay));
                break;
            }
        }

        private bool IsStacked(Card card)
        {
            if (!card.cardType.Equals(CardType.Pillar)) return false;
            TowerCheck(card);
            
            if (!permanentPositions.Exists(t => t.childCount > 0)) return false;
            
            var filteredList = permanentPositions.FindAll(t => t.childCount > 0);
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

    public void PermanentTurnDown()
        {
            // foreach (var idCard in PairList)
            // {
            //     if (!idCard.HasCard()) { continue; }
            //     idCard.cardBehaviour.OnTurnStart();
            // }
        }
    }
}