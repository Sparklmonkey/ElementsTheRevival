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
        private IDCardPair _idCard;
        public void SetupCardDisplay(IDCardPair idCard)
        {
            if (!idCard.HasCard()) { return; }
            this._idCard = idCard;
            SetupButton();
            cardDisplay.SetupCardView(idCard.card);
        }

        private void SetupButton()
        {
            var element = _idCard.id.field == FieldEnum.Hand ? _idCard.card.costElement : _idCard.card.skillElement;
            var cost = _idCard.id.field == FieldEnum.Hand ? _idCard.card.cost : _idCard.card.skillCost;


            var hasQuanta = DuelManager.Instance.player.HasSufficientQuanta(element, cost);
            var isPlayerTurn = BattleVars.Shared.IsPlayerTurn;
            gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);

            if (_idCard.id.owner == OwnerEnum.Opponent || !isPlayerTurn)
            {
                buttonText.text = "";
                actionButton.name = "";
                actionButton.gameObject.SetActive(false);
                return;
            }

            if (_idCard.card.cardType == CardType.Spell)
            {
                if (!SkillManager.Instance.ShouldAskForTarget(_idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.Shared.AbilityOrigin = _idCard;
                    return;
                }
                else if (SkillManager.Instance.ShouldAskForTarget(_idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.Shared.AbilityOrigin = _idCard;
                    return;
                }
                else if (!hasQuanta)
                {
                    buttonText.text = "Insufficient Quanta";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
            }

            if (_idCard.id.field == FieldEnum.Hand)
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

            if (_idCard.card.skill != "")
            {
                if (_idCard.card.AbilityUsed)
                {
                    buttonText.text = "Already Used";
                    actionButton.name = "Insufficient Quanta";
                    return;
                }
                if (!SkillManager.Instance.ShouldAskForTarget(_idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Activate";
                    actionButton.name = "Activate";
                    BattleVars.Shared.AbilityOrigin = _idCard;
                    return;
                }
                else if (SkillManager.Instance.ShouldAskForTarget(_idCard) && hasQuanta && isPlayerTurn)
                {
                    buttonText.text = "Select Target";
                    actionButton.name = "Select Target";
                    BattleVars.Shared.AbilityOrigin = _idCard;
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
            BattleVars.Shared.AbilityOrigin = null;
            gameObject.SetActive(false);
        }


    }
}