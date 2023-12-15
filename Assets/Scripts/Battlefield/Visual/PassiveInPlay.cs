using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{

    public class PassiveInPlay : CardDisplayer
    {
        [SerializeField]
        private Image cardImage, activeAElement;
        [SerializeField]
        private TextMeshProUGUI activeAName, activeACost;
        [SerializeField]
        private GameObject activeAHolder, immaterialIndicator;

        private EventBinding<ClearCardDisplayEvent> _clearCardDisplayBinding;
        private EventBinding<UpdateCardDisplayEvent> _updateCardDisplayBinding;
        private EventBinding<ShouldShowTargetableEvent> _shouldShowTargetableBinding;
        private EventBinding<ShouldShowUsableEvent> _shouldShowUsableBinding;
    
        private void OnDisable() {
            EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
            EventBus<UpdateCardDisplayEvent>.Unregister(_updateCardDisplayBinding);
            EventBus<ShouldShowTargetableEvent>.Unregister(_shouldShowTargetableBinding);
            EventBus<ShouldShowUsableEvent>.Unregister(_shouldShowUsableBinding);
        }
        private void OnEnable()
        {
            _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
            EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
            _updateCardDisplayBinding = new EventBinding<UpdateCardDisplayEvent>(DisplayCard);
            EventBus<UpdateCardDisplayEvent>.Register(_updateCardDisplayBinding);
        
            _shouldShowTargetableBinding = new EventBinding<ShouldShowTargetableEvent>(ShouldShowTarget);
            EventBus<ShouldShowTargetableEvent>.Register(_shouldShowTargetableBinding);
            _shouldShowUsableBinding = new EventBinding<ShouldShowUsableEvent>(ShouldShowUsableGlow);
            EventBus<ShouldShowUsableEvent>.Register(_shouldShowUsableBinding);
        }
        private void DisplayCard(UpdateCardDisplayEvent updateCardDisplayEvent)
        {
            if (!updateCardDisplayEvent.Id.Equals(displayerId))
            {
                return;
            }
            cardImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            cardImage.sprite = ImageHelper.GetCardImage(updateCardDisplayEvent.Card.imageID);

            activeAHolder.SetActive(false);
            if (updateCardDisplayEvent.Card.skill != "")
            {
                activeAHolder.SetActive(true);
                activeAName.text = updateCardDisplayEvent.Card.skill;
                if (updateCardDisplayEvent.Card.skillCost > 0)
                {
                    activeACost.text = updateCardDisplayEvent.Card.skillCost.ToString();
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    activeAElement.sprite = ImageHelper.GetElementImage(updateCardDisplayEvent.Card.skillElement.FastElementString());
                }
                else
                {
                    activeACost.text = "";
                    activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
                }
            }
            else
            {
                activeAHolder.SetActive(false);
            }
            immaterialIndicator.SetActive(updateCardDisplayEvent.Card.innateSkills.Immaterial);
        }

        private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
        {
            if (!clearCardDisplayEvent.Id.Equals(displayerId))
            {
                return;
            }
            DisplayCard(new UpdateCardDisplayEvent(displayerId, CardDatabase.Instance.GetPlaceholderCard(displayerId.index)));
        }
    }
}
