using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elements.Duel.Visual
{

    public class PlayerCardInHand : CardDisplayer
    {
        public bool belongsToPlayer = true;
        [SerializeField]
        private Image cardBackground, cardImage, cardElement, isHidden;
        [SerializeField]
        private TextMeshProUGUI cardName, cardCost;

        public override void DisplayCard(Card cardToDisplay, int stack = 1)
        {
            if(cardToDisplay == null)
            {
                ClearDisplay();
            }

            transform.parent.gameObject.SetActive(true);
            if (!belongsToPlayer)
            {
                isHidden.color = ElementColours.GetWhiteColor();
                cardName.text = " ";
                PlayMaterializeAnimation(Element.Aether);
                return;
            }
            isHidden.color = ElementColours.GetInvisibleColor();

            cardName.text = cardToDisplay.cardName;
            cardName.font = cardToDisplay.iD.IsUpgraded() ? underlayWhite : underlayBlack;
            cardName.color = cardToDisplay.iD.IsUpgraded() ? ElementColours.GetBlackColor() : ElementColours.GetWhiteColor();
            
            cardCost.text = cardToDisplay.cost.ToString();
            cardElement.sprite = ImageHelper.GetElementImage(cardToDisplay.costElement.ToString());

            cardElement.color = ElementColours.GetWhiteColor();


            if (CardDatabase.Instance.cardNameToBackGroundString.TryGetValue(cardToDisplay.cardName, out string backGroundString))
            {
                cardBackground.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
            }
            else
            {
                cardBackground.sprite = ImageHelper.GetCardBackGroundImage(cardToDisplay.costElement.ToString());
            }

            if (cardToDisplay.cost == 0)
            {
                cardCost.text = "";
                cardElement.color = ElementColours.GetInvisibleColor();
            }

            if (cardToDisplay.costElement.Equals(Element.Other))
            {
                cardElement.color = ElementColours.GetInvisibleColor();
            }

            SetCardImage(cardToDisplay.imageID, cardToDisplay.cardName.Contains("Pendulum"), cardToDisplay.costElement == cardToDisplay.skillElement, cardToDisplay.costElement);

            PlayMaterializeAnimation(cardToDisplay.costElement);
        }

        internal void ShowCardForPrecog(Card cardToDisplay)
        {
            isHidden.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
        }

        internal void HideCardForPrecog()
        {
            isHidden.sprite = ImageHelper.GetCardBackImage();
        }

        private void SetCardImage(string imageId, bool isPendulum, bool shouldShowMarkElement, Element costElement)
        {
            if (isPendulum)
            {
                Element markElement = belongsToPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
                if (!shouldShowMarkElement)
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(costElement.FastElementString(), markElement.FastElementString());
                }
                else
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(markElement.FastElementString(), costElement.FastElementString());
                }
            }
            else
            {
                cardImage.sprite = ImageHelper.GetCardImage(imageId);
            }
        }

        public override void HideCard(Card card, int stack)
        {
            PlayDissolveAnimation();
        }
    }

}