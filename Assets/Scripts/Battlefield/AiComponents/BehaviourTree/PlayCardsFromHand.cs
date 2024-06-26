using System.Collections.Generic;

namespace Battlefield.AiComponents.BehaviourTree
{
    public class PlayCardsFromHand
    {
        private Dictionary<CardType, IStrategy> _strategies;
        private readonly PlayerManager _aiOwner;
        private (Card card, ID id) _cardId;
        
        public PlayCardsFromHand((Card card, ID id) cardId, PlayerManager aiOwner)
        {
            _cardId = cardId;
            _aiOwner = aiOwner;
            _strategies = new();
            _strategies.Add(CardType.Pillar, new PlayPillarFromHandStrategy());
            _strategies.Add(CardType.Weapon, new PlayWeaponFromHandStrategy(_aiOwner));
            _strategies.Add(CardType.Shield, new PlayShieldFromHandStrategy(_aiOwner));
            _strategies.Add(CardType.Spell, new PlaySpellFromHandStrategy(_aiOwner));
            _strategies.Add(CardType.Creature, new PlayCreatureFromHandStrategy(_aiOwner));
        }

        public Node.Status PlayCard()
        {
            return _strategies[_cardId.card.Type].Process(_cardId);
        }
    }
}