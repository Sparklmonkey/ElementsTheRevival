using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

namespace Elements.Duel.Visual
{
    public class HealthDisplayer : Identifiable
    {

        [SerializeField]
        private TextMeshProUGUI maxHp, currentHp;
        [SerializeField]
        private Slider hpSlider, damageSlider;
        [SerializeField]
        private GameObject upMovingText;

        public void OnHealthChanged(int currentHP, bool isPlayer)
        {
            if (currentHP.ToString() == currentHp.text) { return; }
            int current = int.Parse(currentHp.text);
            int difference = current - currentHP;

            string toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
            currentHp.text = currentHP.ToString();

            StartCoroutine(AnimateTextChange(toShow));
            int temp = currentHP - DuelManager.GetPossibleDamage(isPlayer);
            hpSlider.value = temp < 0 ? 0 : temp;

            damageSlider.value = currentHP;

            if (currentHP <= 0)
            {
                Debug.Log("Game Over");
                GameOverVisual.ShowGameOverScreen(!isPlayer);
            }
        }

        public void OnMaxHealthChanged(int newMaxHp, bool isPlayer)
        {
            if (newMaxHp.ToString() == maxHp.text) { return; }
            int currentHP = int.Parse(currentHp.text);
            int current = int.Parse(maxHp.text);
            int difference = current - newMaxHp;

            string toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
            maxHp.text = newMaxHp.ToString();

            StartCoroutine(AnimateTextChange(toShow));
            int temp = currentHP - DuelManager.GetPossibleDamage(isPlayer);
            hpSlider.value = temp < 0 ? 0 : temp;

            damageSlider.value = currentHP;

            if (currentHP <= 0)
            {
                Debug.Log("Game Over");
                GameOverVisual.ShowGameOverScreen(!isPlayer);
            }
        }

        public void SetHPStart(int hpToSet)
        {
            maxHp.text = currentHp.text = hpToSet.ToString();
            hpSlider.maxValue = hpToSet;
            hpSlider.value = hpToSet;
            damageSlider.maxValue = hpToSet;
            damageSlider.value = hpToSet;
        }

        public IEnumerator UpdateHPView(int currentHP, bool isPlayer)
        {
            if (currentHP.ToString() == currentHp.text) { yield break; }
            int current = int.Parse(currentHp.text);
            int difference = current - currentHP;

            string toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
            currentHp.text = currentHP.ToString();


            StartCoroutine(AnimateTextChange(toShow));
            int temp = currentHP - DuelManager.GetPossibleDamage(isPlayer);
            hpSlider.value = temp < 0 ? 0 : temp;

            damageSlider.value = currentHP;

            if (currentHP <= 0)
            {
                Debug.Log("Game Over");
                GameOverVisual.ShowGameOverScreen(!isPlayer);
            }

        }

        private IEnumerator AnimateTextChange(string difference)
        {
            GameObject sText = Instantiate(upMovingText, transform);
            Vector3 destination = sText.GetComponent<MovingText>().SetupObject(difference, TextDirection.Up);
            for (int i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
                yield return null;
            }
            Destroy(sText);
        }

        public void UpdateMaxHPView(int newMax)
        {
            maxHp.text = $"{newMax}";
            hpSlider.maxValue = newMax;
        }
    }
}