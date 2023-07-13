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
        private Image cardImage, upgradeShine, rareIndicator, cardHeadBack, activeAElement;
        [SerializeField]
        private TextMeshProUGUI creatureValue, cardName, activeAName, activeACost;
        [SerializeField]
        private GameObject upMovingText, activeAHolder;

        public override void DisplayCard(Card cardToDisplay, int stackAmount)
        {
            transform.parent.gameObject.SetActive(true);
            cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
            var creatureAtk = cardToDisplay.passive.Contains("antimatter") ? -cardToDisplay.AtkNow : cardToDisplay.AtkNow;
            creatureValue.text = $"{creatureAtk}|{cardToDisplay.DefNow}";
            cardName.text = cardToDisplay.cardName;
            cardHeadBack.sprite = ImageHelper.GetCardHeadBackground(cardToDisplay.costElement.FastElementString());
            if (cardToDisplay.IsRare())
            {
                upgradeShine.gameObject.SetActive(true);
                cardName.font = underlayWhite;
                cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
            }
            else
            {
                upgradeShine.gameObject.SetActive(false);
                cardName.font = underlayBlack;
                cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
            rareIndicator.gameObject.SetActive(cardToDisplay.IsRare());

            PlayMaterializeAnimation(cardToDisplay.costElement);
            activeAHolder.SetActive(false);
            if (cardToDisplay.skill != "")
            {
                activeAHolder.SetActive(true);
                activeAName.text = cardToDisplay.skill;
                if(cardToDisplay.skillCost > 0)
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
                if (cardToDisplay.passive.Contains("vampire"))
                {
                    activeAName.text = "Vamprire";
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
                    activeACost.text = "";
                }
                else
                {
                    activeAHolder.SetActive(false);
                }
            }
        }

        public IEnumerator ShowDamage(int damage)
        {
            if (damage <= 0) { yield break; }
            GameObject sText = Instantiate(upMovingText, transform);
            Vector3 destination = sText.GetComponent<MovingText>().SetupObject(damage.ToString(), TextDirection.Up);
            for (int i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
                yield return null;
            }
            Destroy(sText);
        }


        public override void HideCard(Card card, int stack)
        {
            PlayDissolveAnimation();
        }

    }
}
