using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{

    public class PermanentInPlay : CardDisplayer
    {
        [SerializeField]
        private Image cardImage, activeAElement;
        [SerializeField]
        private TextMeshProUGUI stackCount, activeAName, activeACost;
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
        
        public void DisplayCard(UpdateCardDisplayEvent updateCardDisplayEvent)
        {
            if (!updateCardDisplayEvent.Id.Equals(displayerId))
            {
                return;
            }
            List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
            if (permanentsWithCountdown.Contains(updateCardDisplayEvent.Card.iD))
            {
                stackCount.text = $"{updateCardDisplayEvent.Card.TurnsInPlay}";
            }
            else
            {
                stackCount.text = updateCardDisplayEvent.Stack > 1 ? $"{updateCardDisplayEvent.Stack}X" : "";
            }
            maskImage.gameObject.SetActive(true);
            immaterialIndicator.SetActive(updateCardDisplayEvent.Card.innateSkills.Immaterial);
            var isPlayer = displayerId.owner.Equals(OwnerEnum.Player);
            if (updateCardDisplayEvent.Card.cardName.Contains("Pendulum"))
            {
                var pendulumElement = updateCardDisplayEvent.Card.costElement;
                var markElement = isPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
                if (updateCardDisplayEvent.Card.costElement == updateCardDisplayEvent.Card.skillElement)
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(pendulumElement.FastElementString(), markElement.FastElementString());
                }
                else
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(markElement.FastElementString(), pendulumElement.FastElementString());
                }
            }
            else
            {
                cardImage.sprite = ImageHelper.GetCardImage(updateCardDisplayEvent.Card.imageID);
            }
            activeAHolder.SetActive(false);
            if (updateCardDisplayEvent.Card.skill != "")
            {
                activeAHolder.SetActive(true);
                activeAName.text = updateCardDisplayEvent.Card.skill;
                if (updateCardDisplayEvent.Card.skillCost > 0)
                {
                    activeACost.text = updateCardDisplayEvent.Card.skillCost.ToString();
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
            return;
        }
        
        public void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
        {
            if (!clearCardDisplayEvent.Id.Equals(displayerId))
            {
                return;
            }
            if (clearCardDisplayEvent.Stack == 0)
            {
                ClearDisplay();
                return;
            }
            
            List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
            if (permanentsWithCountdown.Contains(clearCardDisplayEvent.Card?.iD))
            {
                stackCount.text = $"{clearCardDisplayEvent.Card?.TurnsInPlay}";
            }
            else
            {
                stackCount.text = clearCardDisplayEvent.Stack > 1 ? $"{clearCardDisplayEvent.Stack}X" : "";
            }
        }
    }
}