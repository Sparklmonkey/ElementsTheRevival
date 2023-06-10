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

        public void SetupDisplayer(OwnerEnum owner, FieldEnum field)
        {
            int index = int.Parse(transform.parent.gameObject.name.Replace("Permanent_", ""));
            SetID(owner, field, index - 1, transform);
        }

        public void UpdatePendulumDisplay()
        {
            Card cardToDisplay = GetCardOnDisplay();
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
        }

        public void DisplayCard(Card cardToDisplay, int stackCountValue)
        {
            if(stackCountValue == 0)
            {
                ClearDisplay();
                return;
            }
            UpdateStackCount(stackCountValue);
            transform.parent.gameObject.SetActive(true);
            immaterialIndicator.SetActive(cardToDisplay.innate.Contains("immaterial"));
            if (stackCountValue == 0)
            {
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
                stackCount.text = stackCountValue.ToString();
                PlayMaterializeAnimation(cardToDisplay.costElement);
                SetCard(cardToDisplay);
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
            //SetCard(cardToDisplay);
        }

        private void UpdateStackCount(int stackCount)
        {
            this.stackCount.text = stackCount > 1 ? $"{stackCount} X" : "";
        }

        public void ClearPassive()
        {
            ClearDisplay();
            //gameObject.SetActive(false);
        }
    }
}