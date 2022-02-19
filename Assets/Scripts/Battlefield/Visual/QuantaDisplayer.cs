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

        public void SetNewQuantaAmount(string newValue)
        {
            if (newValue != quantaCount.text)
            {
                int current = int.Parse(quantaCount.text);
                int newAmount = int.Parse(newValue);
                int difference = current - newAmount;
                if (difference > 0)
                {
                    AnimateTextChange($"-{difference}");
                }
                else
                {
                    AnimateTextChange($"+{Math.Abs(difference)}");
                }
                quantaCount.text = newValue;
            }
        }

        public string GetNameOfElement() => gameObject.name;
        private void AnimateTextChange(string difference)
        {
            GameObject sText = Instantiate(sideMovingText, transform);
            sText.GetComponent<MovingText>().SetupObject(difference, TextDirection.Right);
        }
    }
}