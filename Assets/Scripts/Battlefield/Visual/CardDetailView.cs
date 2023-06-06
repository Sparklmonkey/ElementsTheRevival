using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elements.Duel.Visual
{
    public class CardDetailView : MonoBehaviour
    {
        [SerializeField]
        private CardDisplay cardDisplay;
        [SerializeField]
        private TextMeshProUGUI buttonText;

        [SerializeField]
        private Button actionButton;
        private ID cardID;
        public void SetupCardDisplay(ID cardID, Card cardToDisplay, bool canPlay)
        {
            if(cardID.Field.Equals(FieldEnum.Player)) { return; }
            gameObject.SetActive(true);
            this.cardID = cardID;
            cardDisplay.SetupCardView(cardToDisplay);
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
                if (CardDatabase.Instance.skillsNoTarget.Contains(card.skill) && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.cardOnStandBy = card;
                    return;
                }
                else if (CardDatabase.Instance.skillsWithTarget.Contains(card.skill) && canPlay)
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
                if (CardDatabase.Instance.skillsNoTarget.Contains(card.skill) && canPlay)
                {
                    actionButton.gameObject.SetActive(true);
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.originId = cardID;
                    BattleVars.shared.cardOnStandBy = card;
                    return;
                }
                else if(CardDatabase.Instance.skillsWithTarget.Contains(card.skill) && canPlay)
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

        public void CancelButtonAction()
        {
            BattleVars.shared.cardOnStandBy = null;
            BattleVars.shared.originId = null;
            gameObject.SetActive(false);
        }
    }
}