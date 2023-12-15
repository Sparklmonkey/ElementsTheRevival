using System;
using System.Collections.Generic;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PassiveManager : FieldManager
    {
        private bool _isPlayer;
        
        private EventBinding<PlayCardOnFieldEvent> _playCardOnFieldBinding;
    
        public void OnDisable() {
            EventBus<PlayCardOnFieldEvent>.Unregister(_playCardOnFieldBinding);
        }

        public void SetupManager(bool isPlayer)
        {
            _isPlayer = isPlayer;
            _playCardOnFieldBinding = new EventBinding<PlayCardOnFieldEvent>(PlayPassive);
            EventBus<PlayCardOnFieldEvent>.Register(_playCardOnFieldBinding);
        }
        
        public void PlayPassive(PlayCardOnFieldEvent playCardOnFieldEvent)
        {
            if (!playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Weapon) 
                && !playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Shield) 
                && !playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Mark))
            {
                return;
            }
            if (playCardOnFieldEvent.IsPlayer != _isPlayer)
            {
                return;
            }
            switch (playCardOnFieldEvent.CardToPlay.cardType)
            {
                case CardType.Weapon:
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[1].id, playCardOnFieldEvent.CardToPlay));
                    break;
                case CardType.Shield:
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[2].id, playCardOnFieldEvent.CardToPlay));
                    break;
                case CardType.Mark:
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[0].id, playCardOnFieldEvent.CardToPlay));
                    break;
            }
        }

        public IDCardPair GetShield()
        {
            return PairList[2];
        }
        public IDCardPair GetWeapon()
        {
            return PairList[1];
        }

        public IDCardPair GetMark()
        {
            return PairList[0];
        }
        public ID GetMarkID()
        {
            return PairList[0].id;
        }

        internal ID GetShieldID()
        {
            return PairList[2].id;
        }
        internal ID GetWeaponID()
        {
            return PairList[1].id;
        }
        private readonly List<string> _turnCount = new() { "7n8", "5oo", "61t", "80d" };
        public void PassiveTurnDown()
        {
            foreach (var idCard in PairList)
            {
                if (!idCard.HasCard()) { continue; }
                idCard.cardBehaviour.OnTurnStart();
                if (idCard.card.TurnsInPlay <= 0 && _turnCount.Contains(idCard.card.iD))
                {
                    if (idCard.id.index == 2)
                    {
                        RemoveShield();
                    }
                }
                idCard.UpdateCard();
            }
        }

        internal void RemoveWeapon()
        {
            var card = CardDatabase.Instance.GetPlaceholderCard(1);
            EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[1].id, card));
        }

        internal void RemoveShield()
        {
            var card = CardDatabase.Instance.GetPlaceholderCard(0);
            EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[2].id, card));
        }
    }
}