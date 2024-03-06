using Core.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{
    public class CardDetailView : MonoBehaviour
    {
        [SerializeField] private CardDisplay cardDisplay;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button actionButton, cancelButton;
        private ID _id;
        private Card _card;
        private ButtonCase _buttonCase;
        private EventBinding<SetupCardDisplayEvent> _setupCardDisplayBinding;

        private void OnDisable()
        {
            EventBus<SetupCardDisplayEvent>.Unregister(_setupCardDisplayBinding);
        }

        private void OnEnable()
        {
            _setupCardDisplayBinding = new EventBinding<SetupCardDisplayEvent>(SetupCardDisplay);
            EventBus<SetupCardDisplayEvent>.Register(_setupCardDisplayBinding);
        }

        private void SetupCardDisplay(SetupCardDisplayEvent setupCardDisplayEvent)
        {
            _card = setupCardDisplayEvent.Card;
            _id = setupCardDisplayEvent.Id;
            cardDisplay.gameObject.SetActive(true);
            actionButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
            SetupButton(setupCardDisplayEvent.IsPlayable, setupCardDisplayEvent.IsAbilityUsable);
            cardDisplay.SetupCardView(_card);
        }
        
        private void SetupButton(bool isPlayable, bool isAbilityUsable)
        {
            var isPlayerTurn = BattleVars.Shared.IsPlayerTurn;

            actionButton.gameObject.SetActive(true);

            if (_id.owner == OwnerEnum.Opponent || !isPlayerTurn)
            {
                _buttonCase = ButtonCase.None;
                SetButtonProperties("");
                actionButton.gameObject.SetActive(false);
                return;
            }

            if (_card.Type == CardType.Spell)
            {
                if (SetButtonForSpell(isPlayable)) return;
            }

            if (_id.IsFromHand())
            {
                SetButtonForHand(isPlayable);
                return;
            }

            if (_card.Skill is not null)
            {
                if (_card.AbilityUsed)
                {
                    _buttonCase = ButtonCase.None;
                    SetButtonProperties("Ability used");
                    return;
                }

                if (SetButtonForSpell(isAbilityUsable)) return;
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

        private bool SetButtonForSpell(bool hasQuanta)
        {
            if (!SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta)
            {
                _buttonCase = ButtonCase.Activate;
                SetButtonProperties("Activate");
                return true;
            }

            if (SkillManager.Instance.ShouldAskForTarget(_card) && hasQuanta)
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
            ClearCard();
        }

        private void ClearCard()
        {
            _card = null;
            _id = null;
            cardDisplay.gameObject.SetActive(false);
            actionButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        }

        public void ActionButton()
        {
            switch (_buttonCase)
            {
                case ButtonCase.Play:
                    EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(_card, _id));
                    break;
                case ButtonCase.Activate:
                    EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(_id, _card));
                    break;
                case ButtonCase.SelectTarget:
                    BattleVars.Shared.IsSelectingTarget = true;
                    EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(DuelManager.Instance.player, _card));
                    break;
            }

            ClearCard();
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
    public bool ShouldHideGraphic;

    public SetupAbilityTargetsEvent(PlayerManager abilityOwner, Card abilityCard, bool shouldHideGraphic = false)
    {
        AbilityOwner = abilityOwner;
        AbilityCard = abilityCard;
        ShouldHideGraphic = shouldHideGraphic;
    }
}

public enum ButtonCase
{
    Play,
    Activate,
    SelectTarget,
    None
}