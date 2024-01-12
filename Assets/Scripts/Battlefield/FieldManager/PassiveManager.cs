using System;
using System.Collections.Generic;
using Battlefield.Abstract;
using UnityEngine;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PassiveManager : CardManager
    {
        private EventBinding<PlayPassiveOnFieldEvent> _playCardOnFieldBinding;
    
        public void OnDisable() {
            EventBus<PlayPassiveOnFieldEvent>.Unregister(_playCardOnFieldBinding);
        }

        private void OnEnable()
        {
            _playCardOnFieldBinding = new EventBinding<PlayPassiveOnFieldEvent>(PlayPassive);
            EventBus<PlayPassiveOnFieldEvent>.Register(_playCardOnFieldBinding);
        }
        
        private void PlayPassive(PlayPassiveOnFieldEvent playCardOnFieldEvent)
        {
            if (!playCardOnFieldEvent.Owner.Equals(owner)) return;

            var id = new ID(owner, field, 0);

            id.index = playCardOnFieldEvent.CardToPlay.cardType switch
            {
                CardType.Weapon => 1,
                CardType.Shield => 2,
                CardType.Mark => 0,
                _ => id.index
            };
            
            InstantiateCardObject(id);
            EventBus<UpdatePassiveDisplayEvent>.Raise(new UpdatePassiveDisplayEvent(id, playCardOnFieldEvent.CardToPlay, false));
            // var handCardObject = Instantiate(cardPrefab, cardPositions[id.index]);
            // handCardObject.GetComponent<PassiveCardDisplay>().SetupId(id);
            //
            // EventBus<UpdatePassiveDisplayEvent>.Raise(new UpdatePassiveDisplayEvent(id, playCardOnFieldEvent.CardToPlay, false));
        }

        public (ID, Card) GetShield()
        {
            var passiveDisplay = _cardFieldDisplays[new ID(owner, field, 0)];
            return (passiveDisplay.Id, passiveDisplay.Card);
        }
        public (ID, Card) GetWeapon()
        {
            var passiveDisplay = _cardFieldDisplays[new ID(owner, field, 1)];
            return (passiveDisplay.Id, passiveDisplay.Card);
        }

        public (ID, Card) GetMark()
        {
            var passiveDisplay = _cardFieldDisplays[new ID(owner, field, 0)];
            return (passiveDisplay.Id, passiveDisplay.Card);
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
    }
}