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
        [SerializeField]
        private OwnerEnum owner;

        private EventBinding<ModifyPlayerHealthVisualEvent> _modifyPlayerHealthLogicBinding;
    
        private void OnDisable() {
            EventBus<ModifyPlayerHealthVisualEvent>.Unregister(_modifyPlayerHealthLogicBinding);
        }
        
        private void OnEnable()
        {
            _modifyPlayerHealthLogicBinding = new EventBinding<ModifyPlayerHealthVisualEvent>(ModifyPlayerHealth);
            EventBus<ModifyPlayerHealthVisualEvent>.Register(_modifyPlayerHealthLogicBinding);
        }

        private void ModifyPlayerHealth(ModifyPlayerHealthVisualEvent modifyPlayerHealthVisualEvent)
        {
            if (!modifyPlayerHealthVisualEvent.Owner.Equals(owner)) return;


            if (modifyPlayerHealthVisualEvent.CurrentHp <= 0)
            {
                EventBus<GameEndEvent>.Raise(new GameEndEvent(owner));
            }
            var current = int.Parse(currentHp.text);
            var difference = current - modifyPlayerHealthVisualEvent.CurrentHp;
            var toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
            currentHp.text = modifyPlayerHealthVisualEvent.CurrentHp.ToString();

            if (modifyPlayerHealthVisualEvent.MaxHp != (int)hpSlider.maxValue)
            {
                difference = (int)hpSlider.maxValue - modifyPlayerHealthVisualEvent.MaxHp;
                maxHp.text = modifyPlayerHealthVisualEvent.MaxHp.ToString();
                toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";
                hpSlider.maxValue = modifyPlayerHealthVisualEvent.MaxHp;
                damageSlider.maxValue = modifyPlayerHealthVisualEvent.MaxHp;
            }
                
            var temp = modifyPlayerHealthVisualEvent.CurrentHp - DuelManager.Instance.GetPossibleDamage(owner.Equals(OwnerEnum.Player));
            hpSlider.value = temp < 0 ? 0 : temp;

            damageSlider.value = modifyPlayerHealthVisualEvent.CurrentHp;
            if (difference == 0) return;
            StartCoroutine(AnimateTextChange(toShow));
        }
        
        public void SetHpStart(int hpToSet)
        {
            maxHp.text = currentHp.text = hpToSet.ToString();
            hpSlider.maxValue = hpToSet;
            hpSlider.value = hpToSet;
            damageSlider.maxValue = hpToSet;
            damageSlider.value = hpToSet;
        }

        private IEnumerator AnimateTextChange(string difference)
        {
            var sText = Instantiate(upMovingText, transform);
            var destination = sText.GetComponent<MovingText>().SetupObject(difference, TextDirection.Up);
            for (var i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
                yield return null;
            }
            Destroy(sText);
        }
    }
}