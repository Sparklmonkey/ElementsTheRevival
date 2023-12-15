using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.Duel.Visual
{
    public class PlayerCardInHand : CardDisplayer
    {
        public bool belongsToPlayer = true;
        [SerializeField]
        private Image cardBackground, cardImage, cardElement, isHidden;
        [SerializeField]
        private TextMeshProUGUI cardName, cardCost;
        
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
            
            maskImage.gameObject.SetActive(true);
            if (!belongsToPlayer)
            {
                if (isHidden)
                {
                    isHidden.sprite = ImageHelper.GetCardBackImage();
                    isHidden.color = ElementColours.GetWhiteColor();
                    cardName.text = " ";
                    return;
                }
                isHidden.sprite = ImageHelper.GetCardImage(updateCardDisplayEvent.Card.imageID);
                return;
            }
            isHidden.color = ElementColours.GetInvisibleColor();

            cardName.text = updateCardDisplayEvent.Card.cardName;
            cardName.font = updateCardDisplayEvent.Card.iD.IsUpgraded() ? underlayWhite : underlayBlack;
            cardName.color = updateCardDisplayEvent.Card.iD.IsUpgraded() ? ElementColours.GetBlackColor() : ElementColours.GetWhiteColor();

            cardCost.text = updateCardDisplayEvent.Card.cost.ToString();
            cardElement.sprite = ImageHelper.GetElementImage(updateCardDisplayEvent.Card.costElement.ToString());

            cardElement.color = ElementColours.GetWhiteColor();


            if (CardDatabase.Instance.CardNameToBackGroundString.TryGetValue(updateCardDisplayEvent.Card.cardName, out var backGroundString))
            {
                cardBackground.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
            }
            else
            {
                cardBackground.sprite = ImageHelper.GetCardBackGroundImage(updateCardDisplayEvent.Card.costElement.ToString());
            }

            if (updateCardDisplayEvent.Card.cost == 0)
            {
                cardCost.text = "";
                cardElement.color = ElementColours.GetInvisibleColor();
            }

            if (updateCardDisplayEvent.Card.costElement.Equals(Element.Other))
            {
                cardElement.color = ElementColours.GetInvisibleColor();
            }

            SetCardImage(updateCardDisplayEvent.Card.imageID, updateCardDisplayEvent.Card.cardName.Contains("Pendulum"), updateCardDisplayEvent.Card.costElement == updateCardDisplayEvent.Card.skillElement, updateCardDisplayEvent.Card.costElement);

        }

        private void SetCardImage(string imageId, bool isPendulum, bool shouldShowMarkElement, Element costElement)
        {
            if (isPendulum)
            {
                var markElement = belongsToPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
                if (!shouldShowMarkElement)
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(costElement.FastElementString(), markElement.FastElementString());
                }
                else
                {
                    cardImage.sprite = ImageHelper.GetPendulumImage(markElement.FastElementString(), costElement.FastElementString());
                }
            }
            else
            {
                cardImage.sprite = ImageHelper.GetCardImage(imageId);
            }
        }

        private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
        {
            if (!clearCardDisplayEvent.Id.Equals(displayerId))
            {
                return;
            }
            ClearDisplay();
        }
    }

}