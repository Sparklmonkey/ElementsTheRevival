using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Elements.Duel.Visual
{
    public class HealthDisplayer : Identifiable
    {
        [SerializeField]
        private TextMeshProUGUI maxHp, currentHp;
        [SerializeField]
        private Slider hpSlider;
        [SerializeField]
        private GameObject upMovingText;
        [SerializeField]
        private Image validTargetGlow;

        public void SetHPStart(int hpToSet)
        {
            maxHp.text = currentHp.text = hpToSet.ToString();
            hpSlider.maxValue = hpToSet;
            hpSlider.value = hpToSet;
        }

        public void UpdateHPView(int currentHP, bool isPlayer)
        {
            if (currentHP.ToString() != currentHp.text)
            {
                int current = int.Parse(currentHp.text);
                int difference = current - currentHP;
                if (difference > 0)
                {
                    AnimateTextChange($"-{difference}");
                }
                else
                {
                    AnimateTextChange($"+{Math.Abs(difference)}");
                }
                currentHp.text = currentHP.ToString();
                hpSlider.value = currentHP;


                if (currentHP <= 0)
                {
                    Debug.Log("Game Over");
                    GameOverVisual.ShowGameOverScreen(!isPlayer);
                }
            }
        }

        private void AnimateTextChange(string difference)
        {
            GameObject sText = Instantiate(upMovingText, transform);
            sText.GetComponent<MovingText>().SetupObject(difference, TextDirection.Up);
        }

        public void UpdateMaxHPView(int newMax)
        {
            maxHp.text = $"{newMax}";
            hpSlider.maxValue = newMax;
        }
    }
}