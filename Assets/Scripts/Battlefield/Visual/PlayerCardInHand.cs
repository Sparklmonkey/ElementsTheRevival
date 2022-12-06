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
        public void DisplayCard(Card cardToDisplay, bool quickPlay = false)
        {
            transform.parent.gameObject.SetActive(true);
            if (!GetObjectID().Owner.Equals(OwnerEnum.Player))
            {
                cardName.text = " ";
                PlayMaterializeAnimation(Element.Aether);
                return;
            }
            isHidden.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            cardName.text = cardToDisplay.cardName;


            if (cardToDisplay.iD.IsUpgraded())
            {
                cardName.font = underlayWhite;
                cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
            }
            else
            {
                cardName.font = underlayBlack;
                cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }

            cardCost.text = cardToDisplay.cost.ToString();
            cardElement.sprite = ImageHelper.GetElementImage(cardToDisplay.costElement.ToString());

            cardElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

            string backGroundString = cardToDisplay.cardName == "Animate Weapon" ? "Air" :
                                    cardToDisplay.cardName == "Luciferin" || cardToDisplay.cardName == "Luciferase" ? "Light" :
                                    cardToDisplay.costElement.ToString();

            if (cardToDisplay.cost == 0)
            {
                cardCost.text = "";
                cardElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            if (cardToDisplay.costElement.Equals(Element.Other))
            {
                cardElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            cardBackground.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
            bool isPlayer = GetObjectID().Owner.Equals(OwnerEnum.Player);
            if (cardToDisplay.cardName.Contains("Pendulum"))
            {
                Element pendulumElement = cardToDisplay.costElement;
                Element markElement = isPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
                if (cardToDisplay.costElement == cardToDisplay.skillElement)
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(pendulumElement.FastElementString(), markElement.FastElementString());
                }
                else
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(markElement.FastElementString(), pendulumElement.FastElementString());
                }
            }
            else
            {
                cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
            }
            if (!quickPlay)
            {
                PlayMaterializeAnimation(cardToDisplay.costElement);
            }
            else
            {
                SetRayCastTarget(true);
            }
            SetCard(cardToDisplay);
        }

        internal void ShowCardForPrecog(Card cardToDisplay)
        {
            isHidden.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
            SetCard(cardToDisplay);
        }
    }

}