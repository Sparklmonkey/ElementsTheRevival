using System;
namespace Elements.Duel.Manager
{

    public class CardDetailManager
    {
        private IDCardPair cardOnDisplay;


        public event Action<IDCardPair> OnDisplayNewCard;
        public event Action OnRemoveCard;

        public void SetCardOnDisplay(IDCardPair idCard)
        {
            cardOnDisplay = idCard;
            OnDisplayNewCard?.Invoke(idCard);
        }

        public IDCardPair GetCardID() => cardOnDisplay;
        public void ClearID()
        {
            cardOnDisplay = null;
            OnRemoveCard?.Invoke();
        }
    }
}