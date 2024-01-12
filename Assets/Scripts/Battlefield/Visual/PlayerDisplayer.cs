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

        private void OnDisable()
        {
            EventBus<ShouldShowTargetableEvent>.Unregister(_shouldShowTargetableBinding);
        }
        
        private void OnEnable()
        {
            _shouldShowTargetableBinding = new EventBinding<ShouldShowTargetableEvent>(ShouldShowTarget);
            EventBus<ShouldShowTargetableEvent>.Register(_shouldShowTargetableBinding);
        }
        private void ShouldShowTarget(ShouldShowTargetableEvent shouldShowTargetableEvent)
        {
            if (!shouldShowTargetableEvent.DisplayerId.Equals(playerID)) return;
            validTargetGlow.color = shouldShowTargetableEvent.ShouldShow ? new Color(15, 255, 0, 255) : new Color(0, 0, 0, 0);
        }

        public void UpdatePlayerIndicators(Counters playerCounters)
        {
            if (playerCounters.poison != 0)
            {
                poisonImg.gameObject.SetActive(true);
                if (playerCounters.poison < 0)
                {
                    poisonImg.sprite = puritySprite;
                    poisonLabel.text = "+" + Mathf.Abs(playerCounters.poison).ToString();
                }
                else if (playerCounters.neurotoxin > 0)
                {
                    poisonImg.sprite = neurotoxinSprite;
                    poisonLabel.text = playerCounters.poison.ToString();
                }
                else if (playerCounters.poison == 0)
                {
                    poisonImg.gameObject.SetActive(false);
                }
                else
                {
                    poisonImg.sprite = poisonSprite;
                    poisonLabel.text = playerCounters.poison.ToString();
                }
            }
            
            if (playerCounters.invisibility != 0)
            {
                if (playerID.owner.Equals(OwnerEnum.Opponent) && playerCounters.invisibility > 0)
                {
                    cloakVisual.SetActive(true);
                }
                if (playerCounters.invisibility <= 0)
                {
                    playerCounters.invisibility = 0;
                    if (playerID.owner.Equals(OwnerEnum.Opponent))
                    {
                        cloakVisual.SetActive(false);
                    }
                }
            }
            silenceImage.gameObject.SetActive(playerCounters.silence > 0);
            sanctImage.gameObject.SetActive(playerCounters.sanctuary > 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventBus<CardTappedEvent>.Raise(new CardTappedEvent(playerID, null));
        }
    }
}
