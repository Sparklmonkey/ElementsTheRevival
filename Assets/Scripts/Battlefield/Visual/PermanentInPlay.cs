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
        private Image cardImage, stackFrame;
        [SerializeField]
        private TextMeshProUGUI stackCount;

        private int stackCountValue = 0;
        public void SetupDisplayer(OwnerEnum owner, FieldEnum field)
        {
            int index = int.Parse(transform.parent.gameObject.name.Replace("Permanent_", ""));
            SetID(owner, field, index - 1, transform);
        }

        public void DisplayCard(Card cardToDisplay)
        {
            transform.parent.gameObject.SetActive(true);
            if (stackCountValue == 0)
            {
                Element mark;
                bool isPlayer = GetObjectID().Owner.Equals(OwnerEnum.Player);
                if (BattleVars.shared.isPvp)
                {
                    mark = isPlayer ? PlayerData.shared.markElement : Element.Aether;//"Game_PvpHubConnection.shared.GetOpponentMark()";
                }
                else
                {
                    mark = isPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
                }
                cardImage.sprite = cardToDisplay.name.Contains("Pendulum") ? ImageHelper.GetPendulumImage(cardToDisplay.imageID, mark.FastElementString()) : ImageHelper.GetCardImage(cardToDisplay.imageID);
                stackFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                stackCount.text = "";
                PlayMaterializeAnimation(cardToDisplay.element);
                stackCountValue++;
                SetCard(cardToDisplay);
                return;
            }
            stackCountValue++;
            stackFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            stackCount.text = $"X{stackCountValue}";
            SetCard(cardToDisplay);
        }

        public void ChangeStackCount(int newCount)
        {
            stackCountValue = newCount;
            if(stackCountValue > 1)
            {
                stackCount.text = $"X{stackCountValue}";
            }
            else
            {
                stackFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
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