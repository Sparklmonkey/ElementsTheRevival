using System;
namespace Elements.Duel.Manager
{

    public class CardDetailManager
    {
        private Card _cardOnDisplay;
        private ID _idOnDisplay;


        public event Action<ID, Card> OnDisplayNewCard;
        public event Action OnRemoveCard;

        public void SetCardOnDisplay(ID id, Card card)
        {
            _cardOnDisplay = card;
            _idOnDisplay = id;
            OnDisplayNewCard?.Invoke(_idOnDisplay, _cardOnDisplay);
        }

        public ID GetID() => _idOnDisplay;
        public Card GetCard() => _cardOnDisplay;
        public void ClearID()
        {
            _cardOnDisplay = null;
            OnRemoveCard?.Invoke();
        }
    }
}