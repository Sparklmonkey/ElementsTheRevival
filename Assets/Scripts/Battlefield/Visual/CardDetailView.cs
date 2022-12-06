using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elements.Duel.Visual
{
    public class CardDetailView : CardDisplayer
    {
        [SerializeField]
        private Image cardImage, elementImage, cardBack, poisonCounter, freezeCounter, delayCounter, purityCounter, invisibilityCounter, chargeCounter, cardTypeFrame, cardType, creatureVFrame, rareIndicator;
        [SerializeField]
        private TextMeshProUGUI cardCost, cardName, cardDescription, poisonCount, freezeCount, delayCount, purityCount, invisibilityCount, chargeCount, creatureValues, buttonText;

        [SerializeField]
        private Button actionButton;
        private ID cardID;
        private void Start()
        {
            SetRayCastTarget(false);
        }
        public void SetupCardDisplay(ID cardID, Card cardToDisplay, bool canPlay)
        {
            if(cardID.Field.Equals(FieldEnum.Player)) { return; }
            gameObject.SetActive(true);
            this.cardID = cardID;
            SetupBaseCardDetails(cardToDisplay, cardID.Owner);
            if (cardID.Owner.Equals(OwnerEnum.Player))
            {
                SetupButton(cardToDisplay, cardID.Field.Equals(FieldEnum.Hand), canPlay, cardToDisplay.AbilityUsed);
            }
            else
            {
                actionButton.gameObject.SetActive(false);
            }
        }

        private void SetupButton(Card card, bool isFromHand, bool canPlay, bool abilityUsed)
        {
            if(card.skill != "" && card.cardType.Equals(CardType.Spell))
            {
                if (CardDatabase.skillsNoTarget.Contains(card.skill) && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.cardOnStandBy = card;
                    return;
                }
                else if (CardDatabase.skillsWithTarget.Contains(card.skill) && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.cardOnStandBy = card;
                    return;
                }
                else 
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Insufficient Quanta";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
            }

            if(card.skill != null && !isFromHand)
            {
                canPlay = DuelManager.GetIDOwner(cardID).HasSufficientQuanta(card.skillElement, card.skillCost) && !abilityUsed;
                if (CardDatabase.skillsNoTarget.Contains(card.skill) && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.cardOnStandBy = card;
                    return;
                }
                else if(CardDatabase.skillsWithTarget.Contains(card.skill) && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.cardOnStandBy = card;
                    return;
                }
                else if(abilityUsed)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Already Used";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
                else
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Insufficient Quanta";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
            }

            if (isFromHand && canPlay)
            {
                actionButton.gameObject.SetActive(true);
                buttonText.text = "Play";
                actionButton.name = "Play";
                return;
            }
            buttonText.text = "";
            actionButton.name = "";
            actionButton.gameObject.SetActive(false);
        }

        private void SetupBaseCardDetails(Card cardToDisplay, OwnerEnum owner)
        {

            if (cardToDisplay.cardName.Contains("Pendulum"))
            {
                Element pendulumElement = cardToDisplay.costElement;
                Element markElement = owner.Equals(OwnerEnum.Player) ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
                if (cardToDisplay.costElement == cardToDisplay.skillElement)
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(pendulumElement.FastElementString(), markElement.FastElementString());
                }
                else
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(markElement.FastElementString(), pendulumElement.FastElementString());
                }
            }
            else
            {
                cardImage.sprite = ImageHelper.GetCardImage(cardToDisplay.imageID);
            }
            cardName.text = cardToDisplay.cardName;
            string backGroundString = cardToDisplay.cardName == "Animate Weapon" ? "Air" :
                                    cardToDisplay.cardName == "Luciferin" || cardToDisplay.cardName == "Luciferase" ? "Light" :
                                    cardToDisplay.costElement.ToString();
            cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
            cardCost.text = cardToDisplay.cost.ToString();
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.costElement.ToString());
            cardDescription.text = cardToDisplay.desc;
            if (cardToDisplay.cost == 0)
            {
                cardCost.text = "";
                elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            if (cardToDisplay.costElement.Equals(Element.Other))
            {
                elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            if (cardToDisplay.cardType.Equals(CardType.Creature))
            {
                cardTypeFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                creatureValues.text = cardToDisplay.atk + "/" + cardToDisplay.def;
            }
            else
            {
                creatureVFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                creatureValues.text = "";
                cardType.sprite = ImageHelper.GetCardTypeImage(cardToDisplay.cardType.ToString());
            }

            rareIndicator.color = cardToDisplay.IsRare() ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
        }

        private void SetupCounterDisplay(Counters counters)
        {
            if (counters.freeze > 0)
            {
                freezeCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                freezeCount.text = counters.freeze.ToString();
            }
            else
            {
                freezeCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                freezeCount.text = "";
            }

            if (counters.delay > 0)
            {
                delayCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                delayCount.text = counters.delay.ToString();
            }
            else
            {
                delayCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                delayCount.text = "";
            }

            if (counters.poison > 0)
            {
                poisonCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                poisonCount.text = counters.poison.ToString();
            }
            else
            {
                poisonCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                poisonCount.text = "";
            }

            if (counters.invisibility > 0)
            {
                invisibilityCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                invisibilityCount.text = counters.invisibility.ToString();
            }
            else
            {
                invisibilityCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                invisibilityCount.text = "";
            }

            if (counters.poison < 0)
            {
                purityCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                purityCount.text = "+" + Mathf.Abs(counters.poison).ToString();
            }
            else
            {
                purityCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                purityCount.text = "";
            }

            if (counters.charge > 0)
            {
                chargeCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                chargeCount.text = counters.charge.ToString();
            }
            else
            {
                chargeCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                chargeCount.text = "";
            }
        }

        public void CancelButtonAction()
        {
            BattleVars.shared.cardOnStandBy = null;
            BattleVars.shared.originId = null;
            gameObject.SetActive(false);
        }
    }
}