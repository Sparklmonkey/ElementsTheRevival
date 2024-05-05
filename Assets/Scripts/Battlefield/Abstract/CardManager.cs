using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battlefield.Abstract
{
    public class CardManager : MonoBehaviour
    {
        public GameObject cardPrefab;
        public List<Transform> cardPositions;
        public FieldEnum field;
        public OwnerEnum owner;
        protected Dictionary<ID, CardFieldDisplay> _cardFieldDisplays = new Dictionary<ID, CardFieldDisplay>();
        
        private EventBinding<RemoveCardFromManagerEvent> _removeCardFromManagerBinding;

        private void OnDisable()
        {
            EventBus<RemoveCardFromManagerEvent>.Unregister(_removeCardFromManagerBinding);
        }

        private void Awake()
        {
            _removeCardFromManagerBinding = new EventBinding<RemoveCardFromManagerEvent>(UpdateCardFieldDisplays);
            EventBus<RemoveCardFromManagerEvent>.Register(_removeCardFromManagerBinding);
        }

        private void UpdateCardFieldDisplays(RemoveCardFromManagerEvent removeCardFromManagerEvent)
        {
            if (!_cardFieldDisplays.ContainsKey(removeCardFromManagerEvent.Id)) return;
            _cardFieldDisplays.Remove(removeCardFromManagerEvent.Id);
        }

        protected void ResetCardFieldDisplays()
        {
            _cardFieldDisplays.Clear();
            foreach (var cardPosition in cardPositions)
            {
                var fieldObject = cardPosition.GetComponentInChildren<CardFieldDisplay>();
                if (fieldObject is null) continue;
                _cardFieldDisplays.Add(fieldObject.Id, fieldObject);    
            }
        }
        
        public void SetOwner(OwnerEnum newOwner) => owner = newOwner;
        internal void InstantiateCardObject(ID id) 
        {
            var creatureCardObject = Instantiate(cardPrefab, cardPositions[id.index]);
            var fieldObject = creatureCardObject.GetComponent<CardFieldDisplay>();
            fieldObject.SetupId(id);
            if (_cardFieldDisplays.ContainsKey(id))
            {
                _cardFieldDisplays.Remove(id);
            }
            _cardFieldDisplays.Add(id, fieldObject);    
        }

        public List<(ID id, Card card)> GetAllValidCardIds()
        {
            return _cardFieldDisplays.MapDictToTupleList();
        }
    
        public (ID id, Card card) GetCreatureWithGravity()
        {
            var idCardList = GetAllValidCardIds();
            return idCardList.Count == 0 ? default : idCardList.FirstOrDefault(x => x.Item2.passiveSkills.GravityPull);
        }
    
        public List<Card> GetAllValidCards()
        {
            return _cardFieldDisplays.Select(permanent => permanent.Value.Card).ToList();
        }
    }
}