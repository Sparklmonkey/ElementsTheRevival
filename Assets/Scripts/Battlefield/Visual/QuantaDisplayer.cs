using System;
using System.Collections;
using System.Collections.Generic;
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

        // Start is called before the first frame update
        void Start()
        {
            quantaCount.text = "0";
        }

        public IEnumerator SetNewQuantaAmount(string newValue)
        {
            if (newValue != quantaCount.text)
            {
                int current = int.Parse(quantaCount.text);
                int newAmount = int.Parse(newValue);
                int difference = current - newAmount;
                string toShow = difference > 0 ? $"-{difference}" : $"+{Math.Abs(difference)}";

                quantaCount.text = newValue;
                StartCoroutine(AnimateTextChange(toShow));
                yield return null;
            }
        }

        public string GetNameOfElement() => gameObject.name;

        private IEnumerator AnimateTextChange(string difference)
        {
            GameObject sText = Instantiate(sideMovingText, transform);
            Vector3 destination = sText.GetComponent<MovingText>().SetupObject(difference, TextDirection.Right);
            for (int i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
                yield return null;
            }
            Destroy(sText);
        }

    }
}