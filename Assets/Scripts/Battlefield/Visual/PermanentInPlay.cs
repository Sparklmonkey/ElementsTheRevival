using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elements.Duel.Visual
{

    public class PermanentInPlay : CardDisplayer
    {
        [SerializeField]
        private Image cardImage, activeAElement;
        [SerializeField]
        private TextMeshProUGUI stackCount, activeAName, activeACost;
        [SerializeField]
        private GameObject activeAHolder, immaterialIndicator;

        public override void HideCard(Card cardToDisplay, int stackCountValue) => ManagePermanent(cardToDisplay, stackCountValue);

        public override void DisplayCard(Card cardToDisplay, int stackCountValue) => ManagePermanent(cardToDisplay, stackCountValue);

        private void ManagePermanent(Card cardToDisplay, int stackCountValue)
        {
            if (stackCountValue == 0)
            {
                PlayDissolveAnimation();
                return;
            }

            stackCount.text = stackCountValue > 1 ? $"{stackCountValue} X" : "";
            transform.parent.gameObject.SetActive(true);
            immaterialIndicator.SetActive(cardToDisplay.innate.Contains("immaterial"));
            bool isPlayer = true;// GetObjectID().Owner.Equals(OwnerEnum.Player);
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
            stackCount.text = stackCountValue.ToString();
            PlayMaterializeAnimation(cardToDisplay.costElement);
            activeAHolder.SetActive(false);
            if (cardToDisplay.skill != "")
            {
                activeAHolder.SetActive(true);
                activeAName.text = cardToDisplay.skill;
                if (cardToDisplay.skillCost > 0)
                {
                    activeACost.text = cardToDisplay.skillCost.ToString();
                    activeAElement.sprite = ImageHelper.GetElementImage(cardToDisplay.skillElement.FastElementString());
                }
                else
                {
                    activeACost.text = "";
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
                }
            }
            else
            {
                activeAHolder.SetActive(false);
            }
            return;
        }
    }
}