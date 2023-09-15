using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{

    public class PlayerCardInHand : CardDisplayer
    {
        public bool belongsToPlayer = true;
        [SerializeField]
        private Image cardBackground, cardImage, cardElement, isHidden;
        [SerializeField]
        private TextMeshProUGUI cardName, cardCost;

        public override void DisplayCard(Card cardToDisplay, int stack = 1, bool isHidden = true)
        {
            if (cardToDisplay == null)
            {
                ClearDisplay();
            }

            transform.parent.gameObject.SetActive(true);
            if (!belongsToPlayer)
            {
                if (isHidden)
                {
                    this.isHidden.sprite = ImageHelper.GetCardBackImage();
                    this.isHidden.color = ElementColours.GetWhiteColor();
                    cardName.text = " ";
                    PlayMaterializeAnimation(Element.Aether);
                    return;
                }
                this.isHidden.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
                PlayMaterializeAnimation(Element.Aether);
                return;
            }
            this.isHidden.color = ElementColours.GetInvisibleColor();

            cardName.text = cardToDisplay.cardName;
            cardName.font = cardToDisplay.iD.IsUpgraded() ? underlayWhite : underlayBlack;
            cardName.color = cardToDisplay.iD.IsUpgraded() ? ElementColours.GetBlackColor() : ElementColours.GetWhiteColor();

            cardCost.text = cardToDisplay.cost.ToString();
            cardElement.sprite = ImageHelper.GetElementImage(cardToDisplay.costElement.ToString());

            cardElement.color = ElementColours.GetWhiteColor();


            if (CardDatabase.Instance.CardNameToBackGroundString.TryGetValue(cardToDisplay.cardName, out string backGroundString))
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

        private void SetCardImage(string imageId, bool isPendulum, bool shouldShowMarkElement, Element costElement)
        {
            if (isPendulum)
            {
                Element markElement = belongsToPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
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