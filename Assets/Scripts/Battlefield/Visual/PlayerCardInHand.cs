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
            cardName.text = cardToDisplay.name;

            if(cardToDisplay.element.Equals(Element.Aether) ||cardToDisplay.element.Equals(Element.Other) || cardToDisplay.element.Equals(Element.Air) || cardToDisplay.element.Equals(Element.Light) || !cardToDisplay.isUpgradable)
            {
                cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
            }
            else
            {
                cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }


            cardCost.text = cardToDisplay.cost.ToString();
            cardElement.sprite = ImageHelper.GetElementImage(cardToDisplay.element.ToString());

            cardElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

            string backGroundString = cardToDisplay.name == "Animate Weapon" ? "Air" :
                                    cardToDisplay.name == "Luciferin" || cardToDisplay.name == "Luciferase" ? "Light" :
                                    cardToDisplay.element.ToString();

            if (cardToDisplay.cost == 0)
            {
                cardCost.text = "";
                cardElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            if (cardToDisplay.element.Equals(Element.Other))
            {
                cardElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            cardBackground.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
            Element mark = GetObjectID().Owner.Equals(OwnerEnum.Player) ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
            cardImage.sprite = cardToDisplay.name.Contains("Pendulum") ? ImageHelper.GetPendulumImage(cardToDisplay.imageID, mark.FastElementString()) : cardToDisplay.cardImage;// ImageHelper.GetCardImage(cardToDisplay.imageID);
            //cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);

            if (!quickPlay)
            {
                PlayMaterializeAnimation(cardToDisplay.element);
            }
            else
            {
                SetRayCastTarget(true);
            }
            SetCard(cardToDisplay);
        }

        internal void ShowCardForPrecog(Card cardToDisplay)
        {
            isHidden.sprite = cardToDisplay.cardImage;// ImageHelper.GetCardImage(cardToDisplay.imageID);
            SetCard(cardToDisplay);
        }
    }

}