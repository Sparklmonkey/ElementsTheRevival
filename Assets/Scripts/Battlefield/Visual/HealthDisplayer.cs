using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{
    public class HealthDisplayer : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI maxHp, currentHp;
        [SerializeField]
        private Slider hpSlider, damageSlider;
        [SerializeField]
        private GameObject upMovingText;

        public void OnHealthChanged(int currentHp, bool isPlayer)
        {
            if (currentHp.ToString() == this.currentHp.text) { return; }
            int current = int.Parse(this.currentHp.text);
            int difference = current - currentHp;

            string toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
            this.currentHp.text = currentHp.ToString();

            StartCoroutine(AnimateTextChange(toShow));
            int temp = currentHp - DuelManager.Instance.GetPossibleDamage(isPlayer);
            hpSlider.value = temp < 0 ? 0 : temp;

            damageSlider.value = currentHp;
        }

        public void OnMaxHealthChanged(int newMaxHp, bool isPlayer)
        {
            if (newMaxHp.ToString() == maxHp.text) { return; }
            int currentHp = int.Parse(this.currentHp.text);
            int current = int.Parse(maxHp.text);
            int difference = current - newMaxHp;

            string toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
            maxHp.text = newMaxHp.ToString();

            StartCoroutine(AnimateTextChange(toShow));
            int temp = currentHp - DuelManager.Instance.GetPossibleDamage(isPlayer);
            hpSlider.value = temp < 0 ? 0 : temp;

            damageSlider.value = currentHp;
        }

        public void SetHpStart(int hpToSet)
        {
            maxHp.text = currentHp.text = hpToSet.ToString();
            hpSlider.maxValue = hpToSet;
            hpSlider.value = hpToSet;
            damageSlider.maxValue = hpToSet;
            damageSlider.value = hpToSet;
        }

        public IEnumerator UpdateHpView(int currentHp, bool isPlayer)
        {
            if (currentHp.ToString() == this.currentHp.text) { yield break; }
            int current = int.Parse(this.currentHp.text);
            int difference = current - currentHp;

            string toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
            this.currentHp.text = currentHp.ToString();


            StartCoroutine(AnimateTextChange(toShow));
            int temp = currentHp - DuelManager.Instance.GetPossibleDamage(isPlayer);
            hpSlider.value = temp < 0 ? 0 : temp;

            damageSlider.value = currentHp;
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

        public void UpdateMaxHpView(int newMax)
        {
            maxHp.text = $"{newMax}";
            hpSlider.maxValue = newMax;
        }
    }
}