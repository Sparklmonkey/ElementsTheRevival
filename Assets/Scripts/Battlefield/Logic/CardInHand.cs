using System;
namespace Elements.Duel.Manager
{
    public class CardInHand
    {
        public ID Id;
        private Card _card;

        public event Action<Card> OnCardChanged;

        public void SetCardID(ID iD) => Id = iD;

        public void PlayCard(Card card)
        {
            _card = card;
            OnCardChanged?.Invoke(card);
        }

        public void RemoveCard()
        {
            _card = null;
            OnCardChanged?.Invoke(null);
        }

        public Card GetCard() => _card;
    }
}