using UnityEngine;
namespace Elements.Duel.Manager
{

    public class CardDetailManager
    {
        private IDCardPair cardOnDisplay;

        public void SetCardOnDisplay(IDCardPair iD) => cardOnDisplay = iD;

        public IDCardPair GetCardID() => cardOnDisplay;
        public void ClearID() => cardOnDisplay = null;
    }
}