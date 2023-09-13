using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public override void DisplayCard(Card cardToDisplay, int stackCountValue, bool isHidden = true) => ManagePermanent(cardToDisplay, stackCountValue);

        private void ManagePermanent(Card cardToDisplay, int stackCountValue)
        {
            if (stackCountValue == 0)
            {
                PlayDissolveAnimation();
                return;
            }

            List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
            if (permanentsWithCountdown.Contains(cardToDisplay.iD))
            {
                stackCount.text = $"{cardToDisplay.TurnsInPlay}";
            }
            else
            {
                stackCount.text = stackCountValue > 1 ? $"{stackCountValue}X" : "";
            }
            transform.parent.gameObject.SetActive(true);
            immaterialIndicator.SetActive(cardToDisplay.innateSkills.Immaterial);
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