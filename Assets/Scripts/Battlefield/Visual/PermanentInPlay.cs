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

        public int stackCountValue = 0;
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

        public void DisplayCard(Card cardToDisplay)
        {
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
                stackCount.text = "";
                PlayMaterializeAnimation(cardToDisplay.costElement);
                stackCountValue++;
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
            stackCountValue++;
            stackCount.text = $"{stackCountValue} X";
            //SetCard(cardToDisplay);
        }

        public void ChangeStackCount(int newCount)
        {
            stackCountValue = newCount;
            if(stackCountValue > 1)
            {
                stackCount.text = $"{stackCountValue} X";
            }
            else
            {
                stackCount.text = "";
            }
            Command.CommandExecutionComplete();
        }

        private void OnDisable()
        {
            stackCountValue = 0;
        }

        public void ClearPassive()
        {
            stackCountValue = 0;
            ClearDisplay();
            //gameObject.SetActive(false);
        }
    }
}