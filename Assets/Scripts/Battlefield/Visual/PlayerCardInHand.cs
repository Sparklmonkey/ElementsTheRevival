using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elements.Duel.Visual
{

    public class PlayerCardInHand : CardDisplayer
    {
        [SerializeField]
        private Image cardBackground, cardImage, cardElement, isHidden;
        [SerializeField]
        private TextMeshProUGUI cardName, cardCost;

        public void SetupDisplayer(OwnerEnum owner, FieldEnum field)
        {
            int index = int.Parse(transform.parent.gameObject.name.Replace("CardInHand_", ""));
            SetID(owner, field, index - 1, transform);
            if (owner.Equals(OwnerEnum.Opponent))
            {
                Destroy(GetComponent<CardDetailToolTip>());
            }
        }

        public void DisplayCard(Card cardToDisplay)
        {
            transform.parent.gameObject.SetActive(true);
            if (!GetObjectID().Owner.Equals(OwnerEnum.Player))
            {
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
            
            SetCard(cardToDisplay);
        }

        internal void ShowCardForPrecog(Card cardToDisplay)
        {
            isHidden.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
            SetCard(cardToDisplay);
        }

        internal void HideCardForPrecog()
        {
            isHidden.sprite = ImageHelper.GetCardBackImage();
            SetCard(null);
        }

        private void SetCardImage(string imageId, bool isPendulum, bool shouldShowMarkElement, Element costElement)
        {
            if (isPendulum)
            {
                bool isPlayer = GetObjectID().Owner.Equals(OwnerEnum.Player);
                Element markElement = isPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
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
    }

}