using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{

    public class PassiveInPlay : CardDisplayer
    {
        [SerializeField]
        private Image cardImage;
        [SerializeField]
        private GameObject upMovingText;
        // Start is called before the first frame update
        public void SetupDisplayer(OwnerEnum owner, FieldEnum field)
        {
            int index = int.Parse(transform.parent.gameObject.name.Replace("Passive_", ""));
            SetID(owner, field, index - 1, transform);
        }
        public void DisplayCard(Card cardToDisplay, bool shouldAnim)
        {
            transform.parent.gameObject.SetActive(true);
            cardImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            cardImage.sprite = cardToDisplay.cardImage;//ImageHelper.GetCardImage(cardToDisplay.imageID);
            if (shouldAnim)
            {
                PlayMaterializeAnimation(cardToDisplay.element);
            }
            SetCard(cardToDisplay);
        }

        public void ShowDamage(int damage)
        {
            if (damage <= 0) { return; }
            GameObject sText = Instantiate(upMovingText, transform);
            sText.GetComponent<MovingText>().SetupObject(damage.ToString(), TextDirection.Up);
        }
    }
}
