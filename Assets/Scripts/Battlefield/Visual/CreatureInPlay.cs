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
        private TextMeshProUGUI creatureValue, poisonCountValue, purifyCountValue, aflatoxinCountValue, cardName, activeAName, activeACost;
        [SerializeField]
        private GameObject upMovingText, poisonCounter, purifyCounter, freezeObject, otherEffectParent, aflatoxinCounter, activeAHolder;

        public void SetupDisplayer(OwnerEnum owner, FieldEnum field)
        {
            int index = int.Parse(transform.parent.gameObject.name.Replace("Creature_", ""));
            SetID(owner, field, index - 1, transform);
            //PlayDissolveAnimation();
        }

        public void DisplayCard(Card cardToDisplay, bool playAnim = true)
        {
            transform.parent.gameObject.SetActive(true);
            cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
            creatureValue.text = $"{cardToDisplay.AtkNow}|{cardToDisplay.DefNow}";
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

            if (playAnim)
            {
                PlayMaterializeAnimation(cardToDisplay.costElement);
            }
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

            SetCard(cardToDisplay);
            UpdateOtherEffects();
        }

        public void UpdateOtherEffects()
        {
            aflatoxinCounter.SetActive(false);
            poisonCounter.SetActive(false);
            purifyCounter.SetActive(false);
            Card card = GetCardOnDisplay();
            otherEffectParent.transform.Find("Burrowed").gameObject.SetActive(card.innate.Contains("burrow"));
            otherEffectParent.transform.Find("Momentum").gameObject.SetActive(card.passive.Contains("momentum"));
            otherEffectParent.transform.Find("Immaterial").gameObject.SetActive(card.innate.Contains("immaterial"));
            otherEffectParent.transform.Find("Adrenaline").gameObject.SetActive(card.passive.Contains("adrenaline"));
            otherEffectParent.transform.Find("GravityPull").gameObject.SetActive(card.passive.Contains("gravity pull"));
            otherEffectParent.transform.Find("Psion").gameObject.SetActive(card.passive.Contains("psion"));
            otherEffectParent.transform.Find("Delayed").gameObject.SetActive(card.IsDelayed);
            freezeObject.SetActive(card.Freeze > 0);
            aflatoxinCounter.SetActive(false);
            poisonCounter.SetActive(false);
            purifyCounter.SetActive(false);
            if (card.IsAflatoxin)
            {
                aflatoxinCounter.SetActive(true);
                aflatoxinCountValue.text = card.Poison.ToString();
            }
            else if(card.Poison > 0)
            {
                poisonCounter.SetActive(true);
                poisonCountValue.text = card.Poison.ToString();
            }
            else if( card.Poison < 0)
            {
                purifyCounter.SetActive(true);
                purifyCountValue.text = "+" + (Mathf.Abs(card.Poison)).ToString();
            }
        }

        public IEnumerator ShowDamage(int damage)
        {
            if (damage <= 0) { yield break; }
            GameObject sText = Instantiate(upMovingText, transform);
            Vector3 destination = sText.GetComponent<MovingText>().SetupObject(damage.ToString(), TextDirection.Up);
            for (int i = 0; i < 15; i++)
            {
                sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 100f);
                yield return null;
            }
            Destroy(sText);
        }


        public void ClearPassive()
        {
            ClearDisplay();
            //gameObject.SetActive(false);
        }

    }
}
