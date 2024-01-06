using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PassiveManager : MonoBehaviour
    {
        [SerializeField] private GameObject passivePrefab;
        [SerializeField] private List<Transform> passivePositions;
        private OwnerEnum _owner;
        private EventBinding<PlayCardOnFieldEvent> _playCardOnFieldBinding;
    
        public void OnDisable() {
            EventBus<PlayCardOnFieldEvent>.Unregister(_playCardOnFieldBinding);
        }

        private void OnEnable()
        {
            _playCardOnFieldBinding = new EventBinding<PlayCardOnFieldEvent>(PlayPassive);
            EventBus<PlayCardOnFieldEvent>.Register(_playCardOnFieldBinding);
        }
        public void SetOwner(OwnerEnum owner) => _owner = owner;
        
        private void PlayPassive(PlayCardOnFieldEvent playCardOnFieldEvent)
        {
            if (!playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Weapon) 
                && !playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Shield) 
                && !playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Mark))
            {
                return;
            }
            if (!playCardOnFieldEvent.Owner.Equals(_owner)) return;

            var id = new ID(_owner, FieldEnum.Passive, 0);

            id.index = playCardOnFieldEvent.CardToPlay.cardType switch
            {
                CardType.Weapon => 1,
                CardType.Shield => 2,
                CardType.Mark => 0,
                _ => id.index
            };

            var handCardObject = Instantiate(passivePrefab, passivePositions[id.index]);
            handCardObject.GetComponent<PassiveCardDisplay>().SetupId(id);

            EventBus<UpdatePassiveDisplayEvent>.Raise(new UpdatePassiveDisplayEvent(id, playCardOnFieldEvent.CardToPlay, false));
        }

        public (ID, Card) GetShield()
        {
            var passiveDisplay = passivePositions[2].GetComponentInChildren<PassiveCardDisplay>();
            return (passiveDisplay.Id, passiveDisplay.Card);
        }
        public (ID, Card) GetWeapon()
        {
            var passiveDisplay = passivePositions[1].GetComponentInChildren<PassiveCardDisplay>();
            return (passiveDisplay.Id, passiveDisplay.Card);
        }

        public (ID, Card) GetMark()
        {
            var passiveDisplay = passivePositions[0].GetComponentInChildren<PassiveCardDisplay>();
            return (passiveDisplay.Id, passiveDisplay.Card);
        }
        public ID GetMarkID()
        {
            var passiveDisplay = passivePositions[0].GetComponentInChildren<PassiveCardDisplay>();
            return passiveDisplay.Id;
        }

        internal ID GetShieldID()
        {
            var passiveDisplay = passivePositions[2].GetComponentInChildren<PassiveCardDisplay>();
            return passiveDisplay.Id;
        }
        internal ID GetWeaponID()
        {
            var passiveDisplay = passivePositions[1].GetComponentInChildren<PassiveCardDisplay>();
            return passiveDisplay.Id;
        }
        private readonly List<string> _turnCount = new() { "7n8", "5oo", "61t", "80d" };
        public void PassiveTurnDown()
        {
            // foreach (var idCard in PairList)
            // {
            //     if (!idCard.HasCard()) { continue; }
            //     idCard.cardBehaviour.OnTurnStart();
            //     if (idCard.card.TurnsInPlay <= 0 && _turnCount.Contains(idCard.card.iD))
            //     {
            //         if (idCard.id.index == 2)
            //         {
            //             RemoveShield();
            //         }
            //     }
            //     idCard.UpdateCard();
            // }
        }

        internal void RemoveWeapon()
        {
            var card = CardDatabase.Instance.GetPlaceholderCard(1);
            EventBus<UpdatePassiveDisplayEvent>.Raise(new UpdatePassiveDisplayEvent(new ID(_owner, FieldEnum.Passive, 1), card, false));
        }

        internal void RemoveShield()
        {
            var card = CardDatabase.Instance.GetPlaceholderCard(0);
            EventBus<UpdatePassiveDisplayEvent>.Raise(new UpdatePassiveDisplayEvent(new ID(_owner, FieldEnum.Passive, 2), card, false));
        }
    }
}