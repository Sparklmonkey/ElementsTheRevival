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
        private ButtonCase _buttonCase;
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
                _buttonCase = ButtonCase.None;
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
                    _buttonCase = ButtonCase.None;
                    SetButtonProperties("Insufficient Quanta");
                    return;
                }

                if (SetButtonForSpell(isPlayable, isPlayerTurn)) return;
            }

            _buttonCase = ButtonCase.None;
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
                _buttonCase = ButtonCase.Activate;
                SetButtonProperties("Activate");
                return true;
            }

            if (SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta && isPlayerTurn)
            {
                _buttonCase = ButtonCase.SelectTarget;
                SetButtonProperties("Select Target");
                return true;
            }

            if (hasQuanta) return false;
            _buttonCase = ButtonCase.None;
            SetButtonProperties("Insufficient Quanta");
            return true;

        }

        private void SetButtonForHand(bool hasQuanta)
        {
            if (hasQuanta)
            {
                _buttonCase = ButtonCase.Play;
                SetButtonProperties("Play");
                return;
            }

            _buttonCase = ButtonCase.None;
            SetButtonProperties("");
            actionButton.gameObject.SetActive(false);
        }

        public void CancelButtonAction()
        {
            BattleVars.Shared.AbilityCardOrigin = null;
            BattleVars.Shared.AbilityIDOrigin = null;
            gameObject.SetActive(false);
        }

        private void ClearCard()
        {
            _card = null;
            _id = null;
        }

        public void ActionButton()
        {
            switch (_buttonCase)
            {
                case ButtonCase.Play:
                    EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(_card, _id));
                    ClearCard();
                    break;
                case ButtonCase.Activate:
                    EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(_id, _card));
                    ClearCard();
                    break;
                case ButtonCase.SelectTarget:
                    BattleVars.Shared.IsSelectingTarget = true;
                    EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(DuelManager.Instance.player, _card));
                    break;
                case ButtonCase.None:
                    ClearCard();
                    break;
            }
        }
    }
}

public struct ActivateSpellOrAbilityEvent : IEvent
{
    public ID TargetId;
    public Card TargetCard;

    public ActivateSpellOrAbilityEvent(ID targetId, Card targetCard)
    {
        TargetId = targetId;
        TargetCard = targetCard;
    }
}

public struct SetupAbilityTargetsEvent : IEvent
{
    public PlayerManager AbilityOwner;
    public Card AbilityCard;

    public SetupAbilityTargetsEvent(PlayerManager abilityOwner, Card abilityCard)
    {
        AbilityOwner = abilityOwner;
        AbilityCard = abilityCard;
    }
}

public enum ButtonCase
{
    Play,
    Activate,
    SelectTarget,
    None
}