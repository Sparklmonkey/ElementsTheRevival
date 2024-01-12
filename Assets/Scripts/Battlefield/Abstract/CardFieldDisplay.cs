using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlefield.Abstract
{
    public class CardFieldDisplay : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject validTargetGlow, isUsableGlow;
        public Card Card { get; private set; }
        public ID Id { get; private set; }
        public void SetupId(ID newId) => Id = newId;
        protected void SetCard(Card card) => Card = card;
    
        private EventBinding<ShouldShowTargetableEvent> _shouldShowTargetableBinding;
        private EventBinding<ShouldShowUsableEvent> _shouldShowUsableBinding;
        private EventBinding<HideUsableDisplayEvent> _hideUsableDisplayBinding;

        private void OnDisable()
        {
            EventBus<ShouldShowTargetableEvent>.Unregister(_shouldShowTargetableBinding);
            EventBus<ShouldShowUsableEvent>.Unregister(_shouldShowUsableBinding);
            EventBus<HideUsableDisplayEvent>.Unregister(_hideUsableDisplayBinding);
        }

        private void Awake()
        {
            _shouldShowTargetableBinding = new EventBinding<ShouldShowTargetableEvent>(ShouldShowTarget);
            EventBus<ShouldShowTargetableEvent>.Register(_shouldShowTargetableBinding);
            _shouldShowUsableBinding = new EventBinding<ShouldShowUsableEvent>(ShouldShowUsableGlow);
            EventBus<ShouldShowUsableEvent>.Register(_shouldShowUsableBinding);
            _hideUsableDisplayBinding = new EventBinding<HideUsableDisplayEvent>(HideUsableGlow);
            EventBus<HideUsableDisplayEvent>.Register(_hideUsableDisplayBinding);
            
            isUsableGlow.SetActive(false);
            validTargetGlow.SetActive(false);
        }

        private void ShouldShowTarget(ShouldShowTargetableEvent shouldShowTargetableEvent)
        {
            if (this == null) return;
            if (!shouldShowTargetableEvent.DisplayerId.Equals(Id)) return;
            validTargetGlow.SetActive(shouldShowTargetableEvent.ShouldShow);
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
        
        public void OnPointerClick(PointerEventData eventData)
        {
            EventBus<CardTappedEvent>.Raise(new CardTappedEvent(Id, Card));
        }
    }
}