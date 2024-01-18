using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlefield.Abstract
{
    public class CardFieldDisplay : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject validTargetGlow, isUsableGlow;
        [SerializeField] private FieldObjectAnimation fieldObjectAnimation;
        public Card Card { get; private set; }
        public ID Id { get; private set; }

        public void SetupId(ID newId)
        {
            Id = newId;
            fieldObjectAnimation.SetupId(newId);
        }

        protected void SetCard(Card card) => Card = card;
    
        private EventBinding<ShouldShowTargetableEvent> _shouldShowTargetableBinding;
        private EventBinding<ShouldShowUsableEvent> _shouldShowUsableBinding;
        private EventBinding<HideUsableDisplayEvent> _hideUsableDisplayBinding;
        private EventBinding<ActivateAbilityEffectEvent> _activateAbilityEffectBinding;

        private void OnDisable()
        {
            EventBus<ShouldShowTargetableEvent>.Unregister(_shouldShowTargetableBinding);
            EventBus<ShouldShowUsableEvent>.Unregister(_shouldShowUsableBinding);
            EventBus<HideUsableDisplayEvent>.Unregister(_hideUsableDisplayBinding);
            EventBus<ActivateAbilityEffectEvent>.Unregister(_activateAbilityEffectBinding);
        }

        private void Awake()
        {
            _shouldShowTargetableBinding = new EventBinding<ShouldShowTargetableEvent>(ShouldShowTarget);
            EventBus<ShouldShowTargetableEvent>.Register(_shouldShowTargetableBinding);
            _shouldShowUsableBinding = new EventBinding<ShouldShowUsableEvent>(ShouldShowUsableGlow);
            EventBus<ShouldShowUsableEvent>.Register(_shouldShowUsableBinding);
            _hideUsableDisplayBinding = new EventBinding<HideUsableDisplayEvent>(HideUsableGlow);
            EventBus<HideUsableDisplayEvent>.Register(_hideUsableDisplayBinding);
            _activateAbilityEffectBinding = new EventBinding<ActivateAbilityEffectEvent>(ActivateAbilityEffect);
            EventBus<ActivateAbilityEffectEvent>.Register(_activateAbilityEffectBinding);
            
            isUsableGlow.SetActive(false);
            validTargetGlow.SetActive(false);
        }

        private void ShouldShowTarget(ShouldShowTargetableEvent shouldShowTargetableEvent)
        {
            if (this == null) return;
            validTargetGlow.SetActive(false);
            if (shouldShowTargetableEvent.IsCardValidTarget is null) return;
            var isValid = shouldShowTargetableEvent.IsCardValidTarget(Id, Card);
            if (!isValid) return;
            validTargetGlow.SetActive(true);
            EventBus<AddTargetToListEvent>.Raise(new AddTargetToListEvent(Id, Card));
        }

        private void ShouldShowUsableGlow(ShouldShowUsableEvent shouldShowUsableEvent)
        {
            if (this == null) return;
            if (!shouldShowUsableEvent.Owner.Equals(Id.owner)) return;

            if (Id.field.Equals(FieldEnum.Hand))
            {
                if (isUsableGlow is null) return;
                isUsableGlow.SetActive(shouldShowUsableEvent.QuantaCheck(Card.costElement, Card.cost));
                return;
            }
            if (Card.skill == "") return;
            isUsableGlow.SetActive(shouldShowUsableEvent.QuantaCheck(Card.skillElement, Card.skillCost));
        }
        
        private void HideUsableGlow(HideUsableDisplayEvent hideUsableDisplayEvent)
        {
            if (this == null) return;
            validTargetGlow.SetActive(false);
        }
        
        private void ActivateAbilityEffect(ActivateAbilityEffectEvent activateAbilityEffectEvent)
        {
            if (this == null) return;
            if (!activateAbilityEffectEvent.TargetId.Equals(Id)) return;
            activateAbilityEffectEvent.ActivateAbilityEffect(Id, Card);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            EventBus<CardTappedEvent>.Raise(new CardTappedEvent(Id, Card));
        }
    }
}