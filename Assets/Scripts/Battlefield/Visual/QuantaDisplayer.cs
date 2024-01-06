using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Elements.Duel.Visual
{

    public class QuantaDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI quantaCount;
        [SerializeField]
        private GameObject sideMovingText;
        [SerializeField]
        private Element element;
        [SerializeField]
        private OwnerEnum owner;
        
        private EventBinding<QuantaChangeVisualEvent> _quantaSpentVisualBinding;

        private void OnEnable() {    
            _quantaSpentVisualBinding = new EventBinding<QuantaChangeVisualEvent>(UpdateQuantaManager);
            EventBus<QuantaChangeVisualEvent>.Register(_quantaSpentVisualBinding);
        }

        private void OnDisable() {
            EventBus<QuantaChangeVisualEvent>.Unregister(_quantaSpentVisualBinding);
        }

        private void UpdateQuantaManager(QuantaChangeVisualEvent quantaChangeVisualEvent)
        {
            if (!owner.Equals(quantaChangeVisualEvent.Owner) || element != quantaChangeVisualEvent.Element)
            {
                return;
            }
            QuantaChanged(quantaChangeVisualEvent.Amount);
        }

        // Start is called before the first frame update
        private void Start()
        {
            quantaCount.text = "0";
        }

        private void QuantaChanged(int newValue)
        {
            if (newValue.ToString() != quantaCount.text)
            {
                var current = int.Parse(quantaCount.text);
                var newAmount = newValue;
                var difference = current - newAmount;
                var toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";

                quantaCount.text = newValue.ToString();
                StartCoroutine(AnimateTextChange(toShow));
            }
        }

        private IEnumerator AnimateTextChange(string difference)
        {
            var sText = Instantiate(sideMovingText, transform);
            var destination = sText.GetComponent<MovingText>().SetupObject(difference, TextDirection.Right);
            for (var i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
                yield return null;
            }
            Destroy(sText);
        }

    }
}