using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public delegate bool QuantaCheck(Element element, int amount);

    [Serializable]
    public class HandManager : MonoBehaviour
    {
        [SerializeField] private GameObject handPrefab;
        [SerializeField] private List<Transform> handPositions;
        private OwnerEnum _owner;
        
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
        
        public void SetOwner(OwnerEnum owner) => _owner = owner;
        public bool ShouldDiscard()
        {
            var cardCount = handPositions.FindAll(x => x.childCount > 0).Count;
            return cardCount == 8;
        }

        public int GetHandCount()
        {
            var cardCount = handPositions.FindAll(x => x.childCount > 0).Count;
            return cardCount;
        }

        public List<(ID, Card)> GetPlayableCards(QuantaCheck quantaCheck)
        {
            var handList = handPositions.FindAll(x => x.childCount > 0);

            var returnList = handList.Select(hand => hand.GetComponentInChildren<HandCardDisplay>()).Select(component => (component.GetId(), component.GetCard())).ToList();

            return returnList.Where(x => quantaCheck(x.Item2.costElement, x.Item2.cost)).ToList();
        }

        public List<(ID id, Card card)> GetAllCards()
        {
            var handList = handPositions.FindAll(x => x.childCount > 0);

            var returnList = handList.Select(hand => hand.GetComponentInChildren<HandCardDisplay>()).Select(component => (component.GetId(), component.GetCard())).ToList();

            return returnList;
        }

        private IEnumerator MoveCardPosition()
        {
            yield return null;
            for (var j = 0; j < handPositions.Count; j++)
            {
                if (handPositions[j].childCount != 0) continue;
                for (var i = j + 1; i < handPositions.Count; i++)
                {
                    if (handPositions[i].childCount == 0) continue;
                    handPositions[i].GetChild(0).parent = handPositions[j];
                    handPositions[j].GetChild(0).position = handPositions[j].position;
                    handPositions[j].GetChild(0).GetComponent<HandCardDisplay>().SetupId(new ID(_owner, FieldEnum.Hand, j));
                    break;
                }
            }
        }
        
        private void UpdateHandCards(UpdateHandDisplayEvent updateHandDisplayEvent)
        {
            if (!updateHandDisplayEvent.Owner.Equals(_owner))
            {
                return;
            }
            StartCoroutine(MoveCardPosition());
        }

        private void AddCardToHand(AddCardToHandEvent addCardToHandEvent)
        {
            if (!addCardToHandEvent.Owner.Equals(_owner)) return;

            var index = GetNextAvailablePosition();
            var id = new ID(_owner, FieldEnum.Hand, index);
            
            var handCardObject = Instantiate(handPrefab, handPositions[index]);
            handCardObject.GetComponent<HandCardDisplay>().SetupId(id);

            EventBus<UpdateCardDisplayEvent>.Raise(new UpdateCardDisplayEvent(id, addCardToHandEvent.CardToAdd, 0,
                false));
        }

        private int GetNextAvailablePosition()
        {
            return handPositions.FindIndex(x => x.childCount == 0);
        }
        public void ShowCardsForPrecog()
        {
            // foreach (var item in PairList)
            // {
            //     if (!item.HasCard()) { continue; }
            //     item.isHidden = false;
            //     item.UpdateCard();
            // }
        }
    }
}