using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{
    public class CardDetailView : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplay;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button actionButton;
        private ID _id;
        private Card _card;

        private EventBinding<SetupCardDisplayEvent> _modifyPlayerCounterBinding;

        private void OnDisable()
        {
            EventBus<SetupCardDisplayEvent>.Unregister(_modifyPlayerCounterBinding);
        }

        private void OnEnable()
        {
            _modifyPlayerCounterBinding = new EventBinding<SetupCardDisplayEvent>(SetupCardDisplay);
            EventBus<SetupCardDisplayEvent>.Register(_modifyPlayerCounterBinding);
        }

        private void SetupCardDisplay(SetupCardDisplayEvent setupCardDisplayEvent)
        {
            _card = setupCardDisplayEvent.Card;
            _id = setupCardDisplayEvent.Id;
            SetupButton(setupCardDisplayEvent.IsPlayable);
            cardDisplay.SetupCardView(_card);
        }
        
        private void SetupButton(bool isPlayable)
        {
            var isPlayerTurn = BattleVars.Shared.IsPlayerTurn;

            gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);

            if (_id.owner == OwnerEnum.Opponent || !isPlayerTurn)
            {
                SetButtonProperties("");
                actionButton.gameObject.SetActive(false);
                return;
            }

            if (_card.cardType == CardType.Spell)
            {
                if (SetButtonForSpell(isPlayable, isPlayerTurn)) return;
            }

            if (_id.IsFromHand())
            {
                SetButtonForHand(isPlayable);
                return;
            }

            if (!string.IsNullOrEmpty(_card.skill))
            {
                if (_card.AbilityUsed)
                {
                    SetButtonProperties("Insufficient Quanta");
                    return;
                }

                if (SetButtonForSpell(isPlayable, isPlayerTurn)) return;
            }

            SetButtonProperties("");
            actionButton.gameObject.SetActive(false);
        }
        
        private void SetButtonProperties(string buttonName)
        {
            buttonText.text = buttonName;
            actionButton.name = buttonName;
            BattleVars.Shared.AbilityIDOrigin = _id;
            BattleVars.Shared.AbilityCardOrigin = _card;
        }

        private bool SetButtonForSpell(bool hasQuanta, bool isPlayerTurn)
        {
            if (!SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta && isPlayerTurn)
            {
                SetButtonProperties("Activate");
                return true;
            }

            if (SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta && isPlayerTurn)
            {
                SetButtonProperties("Select Target");
                return true;
            }

            if (!hasQuanta)
            {
                SetButtonProperties("Insufficient Quanta");
                return true;
            }

            return false;
        }

        private void SetButtonForHand(bool hasQuanta)
        {
            if (hasQuanta)
            {
                SetButtonProperties("Play");
                return;
            }

            SetButtonProperties("");
            actionButton.gameObject.SetActive(false);
        }

        public void CancelButtonAction()
        {
            BattleVars.Shared.AbilityCardOrigin = null;
            BattleVars.Shared.AbilityIDOrigin = null;
            gameObject.SetActive(false);
        }


        // public void CardDetailButton(Button buttonCase)
        // {
        //     switch (buttonCase.name)
        //     {
        //         case "Play":
        //             if (playerCounters.silence > 0) { return; }
        //             PlayCardFromHandLogic(CardDetailManager.GetID(), CardDetailManager.GetCard());
        //             CardDetailManager.ClearID();
        //             break;
        //         case "Activate":
        //             ActivateAbility(CardDetailManager.GetID(), CardDetailManager.GetCard());
        //             CardDetailManager.ClearID();
        //             break;
        //         case "Select Target":
        //             BattleVars.Shared.IsSelectingTarget = true;
        //             SkillManager.Instance.SetupTargetHighlights(this, BattleVars.Shared.AbilityCardOrigin);
        //             break;
        //         default:
        //             CardDetailManager.ClearID();
        //             break;
        //     }
        // }
    }
}