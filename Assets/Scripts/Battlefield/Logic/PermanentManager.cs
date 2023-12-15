using System;
using System.Collections.Generic;
using System.Linq;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PermanentManager : FieldManager
    {
        private bool _isPlayer;
        
        private EventBinding<PlayCardOnFieldEvent> _playCardOnFieldBinding;
    
        public void OnDisable() {
            EventBus<PlayCardOnFieldEvent>.Unregister(_playCardOnFieldBinding);
        }

        public void SetupManager(bool isPlayer)
        {
            _isPlayer = isPlayer;
            _playCardOnFieldBinding = new EventBinding<PlayCardOnFieldEvent>(PlayPermanent);
            EventBus<PlayCardOnFieldEvent>.Register(_playCardOnFieldBinding);
        }

        public void PlayPermanent(PlayCardOnFieldEvent playCardOnFieldEvent)
        {
            if (!playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Pillar) &&
                !playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Artifact))
            {
                return;
            }

            if (playCardOnFieldEvent.IsPlayer != _isPlayer)
            {
                return;
            }
            if (playCardOnFieldEvent.CardToPlay.cardType.Equals(CardType.Pillar))
            {
                if (playCardOnFieldEvent.CardToPlay.iD.IsUpgraded())
                {
                    EventBus<QuantaChangeLogicEvent>.Raise(
                        playCardOnFieldEvent.CardToPlay.costElement.Equals(Element.Other)
                            ? new QuantaChangeLogicEvent(3, playCardOnFieldEvent.CardToPlay.costElement, _isPlayer,
                                true)
                            : new QuantaChangeLogicEvent(1, playCardOnFieldEvent.CardToPlay.costElement, _isPlayer,
                                true));
                }
                var stackedCard = PairList.FirstOrDefault(idCard => idCard.HasCard() && idCard.card.iD == playCardOnFieldEvent.CardToPlay.iD);
                if (stackedCard is not null)
                {
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(stackedCard.id, playCardOnFieldEvent.CardToPlay));
                    return;
                }
            }

            List<int> permanentCardOrder = new() { 1, 3, 5, 7, 0, 2, 4, 6, 9, 11, 13, 8, 10, 12 };

            foreach (var orderIndex in permanentCardOrder)
            {
                if (!PairList[orderIndex].HasCard())
                {
                    if (StackCountList.Count > 0)
                    {
                        StackCountList[orderIndex]++;
                    }
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(PairList[orderIndex].id, playCardOnFieldEvent.CardToPlay));
                    break;
                }
            }
        }

        public void PermanentTurnDown()
        {
            foreach (var idCard in PairList)
            {
                if (!idCard.HasCard()) { continue; }
                idCard.cardBehaviour.OnTurnStart();
            }
        }

        internal void ClearField()
        {
            foreach (var pair in PairList)
            {
                pair.card = null;
            }
        }
    }

}