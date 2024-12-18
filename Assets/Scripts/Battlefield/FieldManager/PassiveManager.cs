﻿using System;
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

            id.index = playCardOnFieldEvent.CardToPlay.Type switch
            {
                CardType.Weapon => 1,
                CardType.Shield => 2,
                CardType.Mark => 0,
                _ => id.index
            };
            if (!_cardFieldDisplays.ContainsKey(id))
            {
                InstantiateCardObject(id);
            }
            EventBus<UpdatePassiveDisplayEvent>.Raise(new UpdatePassiveDisplayEvent(id, playCardOnFieldEvent.CardToPlay, false));
        }

        public (ID id, Card card) GetShield()
        {
            var passiveDisplay = _cardFieldDisplays[new ID(owner, field, 2)];
            return (passiveDisplay.Id, passiveDisplay.Card);
        }
        public (ID id, Card card) GetWeapon()
        {
            var passiveDisplay = _cardFieldDisplays[new ID(owner, field, 1)];
            return (passiveDisplay.Id, passiveDisplay.Card);
        }

        public (ID id, Card card) GetMark()
        {
            var passiveDisplay = _cardFieldDisplays[new ID(owner, field, 0)];
            return (passiveDisplay.Id, passiveDisplay.Card);
        }
        
        private readonly List<string> _turnCount = new() { "7n8", "5oo", "61t", "80d" };
    }
}