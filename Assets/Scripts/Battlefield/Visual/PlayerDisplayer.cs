using Core.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{
    public class PlayerDisplayer : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image validTargetGlow;
        public ID playerID;
        [SerializeField]
        private Sprite poisonSprite, puritySprite, neurotoxinSprite;
        public Image poisonImg, sanctImage, silenceImage;
        public TextMeshProUGUI poisonLabel, boneShieldLabel, purityLabel;
        [SerializeField]
        private GameObject cloakVisual;

        private EventBinding<ShouldShowTargetableEvent> _shouldShowTargetableBinding;
        private EventBinding<UpdatePlayerCountersVisualEvent> _updatePlayerCountersVisualBinding;
        private EventBinding<ActivateAbilityEffectEvent> _activateAbilityEffectBinding;


        private void OnDisable()
        {
            EventBus<ShouldShowTargetableEvent>.Unregister(_shouldShowTargetableBinding);
            EventBus<UpdatePlayerCountersVisualEvent>.Unregister(_updatePlayerCountersVisualBinding);
            EventBus<ActivateAbilityEffectEvent>.Unregister(_activateAbilityEffectBinding);
        }
        
        private void OnEnable()
        {
            _shouldShowTargetableBinding = new EventBinding<ShouldShowTargetableEvent>(ShouldShowTarget);
            EventBus<ShouldShowTargetableEvent>.Register(_shouldShowTargetableBinding);
            
            _updatePlayerCountersVisualBinding = new EventBinding<UpdatePlayerCountersVisualEvent>(UpdatePlayerIndicators);
            EventBus<UpdatePlayerCountersVisualEvent>.Register(_updatePlayerCountersVisualBinding);
            
            _activateAbilityEffectBinding = new EventBinding<ActivateAbilityEffectEvent>(ActivateAbilityEffect);
            EventBus<ActivateAbilityEffectEvent>.Register(_activateAbilityEffectBinding);
        }
        private void ShouldShowTarget(ShouldShowTargetableEvent shouldShowTargetableEvent)
        {
            if (this == null) return;
            validTargetGlow.color = new Color(0, 0, 0, 0);
            if (shouldShowTargetableEvent.IsCardValidTarget is null) return;
            var isValid = shouldShowTargetableEvent.IsCardValidTarget(playerID, null);
            if (!isValid) return;
            validTargetGlow.color = new Color(15, 255, 0, 255);
            EventBus<AddTargetToListEvent>.Raise(new AddTargetToListEvent(playerID, null));
        }

        private void UpdatePlayerIndicators(UpdatePlayerCountersVisualEvent updatePlayerCountersVisualEvent)
        {
            if (!updatePlayerCountersVisualEvent.PlayerId.Equals(playerID)) return;
            if (updatePlayerCountersVisualEvent.Counters.poison != 0)
            {
                poisonImg.gameObject.SetActive(true);
                if (updatePlayerCountersVisualEvent.Counters.poison < 0)
                {
                    poisonImg.sprite = puritySprite;
                    poisonLabel.text = "+" + Mathf.Abs(updatePlayerCountersVisualEvent.Counters.poison);
                }
                else if (updatePlayerCountersVisualEvent.Counters.neurotoxin > 0)
                {
                    poisonImg.sprite = neurotoxinSprite;
                    poisonLabel.text = updatePlayerCountersVisualEvent.Counters.poison.ToString();
                }
                else if (updatePlayerCountersVisualEvent.Counters.poison == 0)
                {
                    poisonImg.gameObject.SetActive(false);
                }
                else
                {
                    poisonImg.sprite = poisonSprite;
                    poisonLabel.text = updatePlayerCountersVisualEvent.Counters.poison.ToString();
                }
            }
            
            if (updatePlayerCountersVisualEvent.Counters.invisibility != 0)
            {
                if (playerID.IsOwnedBy(OwnerEnum.Opponent) && updatePlayerCountersVisualEvent.Counters.invisibility > 0)
                {
                    cloakVisual.SetActive(true);
                }
                if (updatePlayerCountersVisualEvent.Counters.invisibility <= 0)
                {
                    updatePlayerCountersVisualEvent.Counters.invisibility = 0;
                    if (playerID.IsOwnedBy(OwnerEnum.Opponent))
                    {
                        cloakVisual.SetActive(false);
                    }
                }
            }
            silenceImage.gameObject.SetActive(updatePlayerCountersVisualEvent.Counters.silence > 0);
            sanctImage.gameObject.SetActive(updatePlayerCountersVisualEvent.Counters.sanctuary > 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventBus<CardTappedEvent>.Raise(new CardTappedEvent(playerID, null));
        }
        
        private void ActivateAbilityEffect(ActivateAbilityEffectEvent activateAbilityEffectEvent)
        {
            if (this == null) return;
            if (!activateAbilityEffectEvent.TargetId.Equals(playerID)) return;
            activateAbilityEffectEvent.ActivateAbilityEffect(playerID, null);
        }
    }
}
