using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{

    public class PassiveInPlay : CardDisplayer
    {
        [SerializeField]
        private Image cardImage, activeAElement;
        [SerializeField]
        private TextMeshProUGUI activeAName, activeACost;
        [SerializeField]
        private GameObject upMovingText, activeAHolder, immaterialIndicator;
        // Start is called before the first frame update
        public void SetupDisplayer(OwnerEnum owner, FieldEnum field)
        {
            int index = int.Parse(transform.parent.gameObject.name.Replace("Passive_", ""));
            SetID(owner, field, index - 1, transform);
            isPassive = field.Equals(FieldEnum.Passive);
        }
        public void DisplayCard(Card cardToDisplay)
        {
            transform.parent.gameObject.SetActive(true);
            cardImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
            PlayMaterializeAnimation(cardToDisplay.costElement);
            SetCard(cardToDisplay);

            activeAHolder.SetActive(false);
            if (cardToDisplay.skill != "")
            {
                activeAHolder.SetActive(true);
                activeAName.text = cardToDisplay.skill;
                if (cardToDisplay.skillCost > 0)
                {
                    activeACost.text = cardToDisplay.skillCost.ToString();
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    activeAElement.sprite = ImageHelper.GetElementImage(cardToDisplay.skillElement.FastElementString());
                }
                else
                {
                    activeACost.text = "";
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
                }
            }
            else
            {
                activeAHolder.SetActive(false);
            }
            immaterialIndicator.SetActive(cardToDisplay.innate.Contains("immaterial"));
        }

        public IEnumerator ShowDamage(int damage)
        {
            if (damage == 0) { yield break; }
            GameObject sText = Instantiate(upMovingText, transform);
            Vector3 destination = sText.GetComponent<MovingText>().SetupObject(damage.ToString(), TextDirection.Up);
            for (int i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
                yield return null;
            }
            Destroy(sText);
        }

    }
}
