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
            SetupCounterDisplay(cardToDisplay.cardCounters);
            SetupBaseCardDetails(cardToDisplay, cardID.Owner);
            if (cardID.Owner.Equals(OwnerEnum.Player))
            {
                SetupButton(cardToDisplay, cardID.Field.Equals(FieldEnum.Hand), canPlay, cardToDisplay.firstTurn, cardToDisplay.abilityUsed);
            }
            else
            {
                actionButton.gameObject.SetActive(false);
            }
        }

        private void SetupButton(Card card, bool isFromHand, bool canPlay, bool isFirstTurn, bool abilityUsed)
        {
            if(card.spellAbility != null)
            {
                if (card.spellAbility.isTargetFixed && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.spellOnStandBy = card.spellAbility;
                    return;
                }
                else if (!card.spellAbility.isTargetFixed && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.spellOnStandBy = card.spellAbility;
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

            if(card.activeAbility != null && !isFromHand)
            {
                canPlay = DuelManager.GetIDOwner(cardID).HasSufficientQuanta(card.activeAbility.AbilityElement, card.activeAbility.AbilityCost) && !isFirstTurn && !abilityUsed;
                if (!card.activeAbility.ShouldSelectTarget && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.abilityOnStandBy = card.activeAbility;
                    return;
                }
                else if(card.activeAbility.ShouldSelectTarget && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.abilityOnStandBy = card.activeAbility;
                    return;
                }
                else if(abilityUsed)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Already Used";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
                else if (isFirstTurn)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Have Patience";
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

            if (cardToDisplay.name.Contains("Pendulum"))
            {
                cardImage.sprite = ImageHelper.GetPendulumImage(cardToDisplay.imageID, owner.Equals(OwnerEnum.Player) ? PlayerData.shared.markElement.FastElementString() : BattleVars.shared.enemyAiData.mark.FastElementString());
            }
            else
            {
                cardImage.sprite = cardToDisplay.cardImage;// ImageHelper.GetCardImage(cardToDisplay.imageID);
            }
            cardName.text = cardToDisplay.name;
            string backGroundString = cardToDisplay.name == "Animate Weapon" ? "Air" :
                                    cardToDisplay.name == "Luciferin" || cardToDisplay.name == "Luciferase" ? "Light" :
                                    cardToDisplay.element.ToString();
            cardBack.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
            cardCost.text = cardToDisplay.cost.ToString();
            elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            elementImage.sprite = ImageHelper.GetElementImage(cardToDisplay.element.ToString());
            cardDescription.text = cardToDisplay.description;
            if (cardToDisplay.cost == 0)
            {
                cardCost.text = "";
                elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            if (cardToDisplay.element.Equals(Element.Other))
            {
                elementImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
            }

            if (cardToDisplay.type.Equals(CardType.Creature))
            {
                cardTypeFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                cardType.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                creatureValues.text = cardToDisplay.power + "/" + cardToDisplay.hp;
            }
            else
            {
                creatureVFrame.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                creatureValues.text = "";
                cardType.sprite = ImageHelper.GetCardTypeImage(cardToDisplay.type.ToString());
            }

            rareIndicator.color = cardToDisplay.isRare ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120) : new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
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

            if (counters.purity > 0)
            {
                purityCounter.color = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                purityCount.text = counters.purity.ToString();
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
            BattleVars.shared.abilityOnStandBy = null;
            BattleVars.shared.spellOnStandBy = null;
            BattleVars.shared.originId = null;
            gameObject.SetActive(false);
        }
    }
}