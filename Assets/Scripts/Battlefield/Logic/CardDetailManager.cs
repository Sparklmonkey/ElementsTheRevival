using System;
namespace Elements.Duel.Manager
{

    public class CardDetailManager
    {
        private IDCardPair _cardOnDisplay;


        public event Action<IDCardPair> OnDisplayNewCard;
        public event Action OnRemoveCard;

        public void SetCardOnDisplay(IDCardPair idCard)
        {
            _cardOnDisplay = idCard;
            OnDisplayNewCard?.Invoke(idCard);
        }

        public IDCardPair GetCardID() => _cardOnDisplay;
        public void ClearID()
        {
            _cardOnDisplay = null;
            OnRemoveCard?.Invoke();
        }
    }
}