using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
        private IDCardPair idCard;
        public void SetupCardDisplay(IDCardPair idCard)
        {
            if(!idCard.HasCard()) { return; }
            this.idCard = idCard;
            SetupButton();
            cardDisplay.SetupCardView(idCard.card);
        }

        private void SetupButton()
        {
            var element = idCard.id.Field == FieldEnum.Hand ? idCard.card.costElement : idCard.card.skillElement;
            var cost = idCard.id.Field == FieldEnum.Hand ? idCard.card.cost : idCard.card.skillCost;


            bool hasQuanta = DuelManager.Instance.player.HasSufficientQuanta(element, cost);
            var isPlayerTurn = BattleVars.shared.isPlayerTurn;
            gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);

            if (idCard.id.Owner == OwnerEnum.Opponent || !isPlayerTurn)
            {
                buttonText.text = "";
                actionButton.name = "";
                actionButton.gameObject.SetActive(false);
                return;
            }
                
            if (idCard.card.cardType == CardType.Spell)
            {
                if (!SkillManager.Instance.ShouldAskForTarget(idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.abilityOrigin = idCard;
                    return;
                }
                else if (SkillManager.Instance.ShouldAskForTarget(idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.shared.abilityOrigin = idCard;
                    return;
                }
                else if (!hasQuanta)
                {
                    buttonText.text = "Insufficient Quanta";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
            }

            if (idCard.id.Field == FieldEnum.Hand)
            {
                if (hasQuanta)
                {
                    buttonText.text = "Play";
                    actionButton.name = "Play";
                    return;
                }

                buttonText.text = "";
                actionButton.name = "";
                actionButton.gameObject.SetActive(false);
                return;
            }

            if (idCard.card.skill != "")
            {
                if (idCard.card.AbilityUsed)
                {
                    buttonText.text = "Already Used";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
                if (!SkillManager.Instance.ShouldAskForTarget(idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.shared.abilityOrigin = idCard;
                    return;
                }
                else if (SkillManager.Instance.ShouldAskForTarget(idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.shared.abilityOrigin = idCard;
                    return;
                }
                else if (!hasQuanta)
                {
                    buttonText.text = "Insufficient Quanta";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
            }

            buttonText.text = "";
            actionButton.name = "";
            actionButton.gameObject.SetActive(false);
        }

        public void CancelButtonAction()
        {
            BattleVars.shared.abilityOrigin = null;
            gameObject.SetActive(false);
        }


    }
}