using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{

    public class CreatureInPlay : CardDisplayer
    {
        [SerializeField]
        private Image cardImage;
        [SerializeField]
        private TextMeshProUGUI creatureValue;
        [SerializeField]
        private GameObject upMovingText;

        public void SetupDisplayer(OwnerEnum owner, FieldEnum field)
        {
            int index = int.Parse(transform.parent.gameObject.name.Replace("Creature_", ""));
            SetID(owner, field, index - 1, transform);
            //PlayDissolveAnimation();
        }

        public void DisplayCard(Card cardToDisplay, bool playAnim = true)
        {
            transform.parent.gameObject.SetActive(true);
            cardImage.sprite = cardToDisplay.cardImage;//ImageHelper.GetCardImage(cardToDisplay.imageID);
            creatureValue.text = $"{cardToDisplay.power}/{cardToDisplay.hp}";
            if (playAnim)
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

        public void ClearPassive()
        {
            ClearDisplay();
            //gameObject.SetActive(false);
        }

    }
}
