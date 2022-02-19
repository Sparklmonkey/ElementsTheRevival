using UnityEngine;
namespace Elements.Duel.Manager
{

    public class CardDetailManager
    {
        private ID cardOnDisplay;

        public void SetCardOnDisplay(ID iD) => cardOnDisplay = iD;

        public ID GetCardID() => cardOnDisplay;
        public void ClearID() => cardOnDisplay = null;
    }
}