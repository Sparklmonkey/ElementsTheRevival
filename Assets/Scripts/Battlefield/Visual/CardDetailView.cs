using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private ID _id;
        private Card _card;
        public void SetupCardDisplay(ID id, Card card)
        {
            _card = card;
            _id = id;
            SetupButton();
            cardDisplay.SetupCardView(_card);
        }

        private void SetupButton()
        {
            var element = _id.field == FieldEnum.Hand ? _card.costElement : _card.skillElement;
            var cost = _id.field == FieldEnum.Hand ? _card.cost : _card.skillCost;


            var hasQuanta = DuelManager.Instance.player.HasSufficientQuanta(element, cost);
            var isPlayerTurn = BattleVars.Shared.IsPlayerTurn;
            gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);

            if (_id.owner == OwnerEnum.Opponent || !isPlayerTurn)
            {
                buttonText.text = "";
                actionButton.name = "";
                actionButton.gameObject.SetActive(false);
                return;
            }

            if (_card.cardType == CardType.Spell)
            {
                if (!SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.Shared.AbilityIDOrigin = _id;
                    BattleVars.Shared.AbilityCardOrigin = _card;
                    return;
                }
                
                if (SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.Shared.AbilityIDOrigin = _id;
                    BattleVars.Shared.AbilityCardOrigin = _card;
                    return;
                }
                
                if (!hasQuanta)
                {
                    buttonText.text = "Insufficient Quanta";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
            }

            if (_id.IsFromHand())
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

            if (_card.skill != "")
            {
                if (_card.AbilityUsed)
                {
                    buttonText.text = "Already Used";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
                if (!SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.Shared.AbilityIDOrigin = _id;
                    BattleVars.Shared.AbilityCardOrigin = _card;
                    return;
                }
                
                if (SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.Shared.AbilityIDOrigin = _id;
                    BattleVars.Shared.AbilityCardOrigin = _card;
                    return;
                }
                
                if (!hasQuanta)
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
            BattleVars.Shared.AbilityCardOrigin = null;
            BattleVars.Shared.AbilityIDOrigin = null;
            gameObject.SetActive(false);
        }


    }
}