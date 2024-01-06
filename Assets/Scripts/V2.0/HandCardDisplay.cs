using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardDisplay : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image cardBackground, cardImage, cardElement, isHidden;
    [SerializeField] private TextMeshProUGUI cardName, cardCost;
    [SerializeField] private Image validTargetGlow;
    [SerializeField] private Image isUsableGlow;
    public TMP_FontAsset underlayBlack, underlayWhite;
    private Card _card;
    private ID _id;

    private EventBinding<ClearCardDisplayEvent> _clearCardDisplayBinding;
    private EventBinding<UpdateCardDisplayEvent> _updateCardDisplayBinding;
    private EventBinding<ShouldShowUsableEvent> _shouldShowUsableBinding;
    private EventBinding<HideUsableDisplayEvent> _hideUsableDisplayBinding;

    private void OnDisable()
    {
        EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
        EventBus<UpdateCardDisplayEvent>.Unregister(_updateCardDisplayBinding);
        EventBus<ShouldShowUsableEvent>.Unregister(_shouldShowUsableBinding);
        EventBus<HideUsableDisplayEvent>.Unregister(_hideUsableDisplayBinding);
    }

    private void OnEnable()
    {
        _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
        EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
        _updateCardDisplayBinding = new EventBinding<UpdateCardDisplayEvent>(DisplayCard);
        EventBus<UpdateCardDisplayEvent>.Register(_updateCardDisplayBinding);

        _shouldShowUsableBinding = new EventBinding<ShouldShowUsableEvent>(ShouldShowUsableGlow);
        EventBus<ShouldShowUsableEvent>.Register(_shouldShowUsableBinding);
        
        _hideUsableDisplayBinding = new EventBinding<HideUsableDisplayEvent>(HideUsableGlow);
        EventBus<HideUsableDisplayEvent>.Register(_hideUsableDisplayBinding);
    }

    public void SetupId(ID newId)
    {
        _id = newId;
    }

    private void DisplayCard(UpdateCardDisplayEvent updateCardDisplayEvent)
    {
        if (!updateCardDisplayEvent.Id.Equals(_id))
        {
            return;
        }

        _card = updateCardDisplayEvent.Card;
        isHidden.color = ElementColours.GetInvisibleColor();

        cardName.text = updateCardDisplayEvent.Card.cardName;
        cardName.font = updateCardDisplayEvent.Card.iD.IsUpgraded() ? underlayWhite : underlayBlack;
        cardName.color = updateCardDisplayEvent.Card.iD.IsUpgraded()
            ? ElementColours.GetBlackColor()
            : ElementColours.GetWhiteColor();

        cardCost.text = updateCardDisplayEvent.Card.cost.ToString();
        cardElement.sprite = ImageHelper.GetElementImage(updateCardDisplayEvent.Card.costElement.ToString());

        cardElement.color = ElementColours.GetWhiteColor();


        if (CardDatabase.Instance.CardNameToBackGroundString.TryGetValue(updateCardDisplayEvent.Card.cardName,
                out var backGroundString))
        {
            cardBackground.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        }
        else
        {
            cardBackground.sprite =
                ImageHelper.GetCardBackGroundImage(updateCardDisplayEvent.Card.costElement.ToString());
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

        SetCardImage(updateCardDisplayEvent.Card.imageID, updateCardDisplayEvent.Card.cardName.Contains("Pendulum"),
            updateCardDisplayEvent.Card.costElement == updateCardDisplayEvent.Card.skillElement,
            updateCardDisplayEvent.Card.costElement);
    }

    private void SetCardImage(string imageId, bool isPendulum, bool shouldShowMarkElement, Element costElement)
    {
        if (isPendulum)
        {
            var markElement = PlayerData.Shared.markElement;
            if (!shouldShowMarkElement)
            {
                cardImage.sprite =
                    ImageHelper.GetPendulumImage(costElement.FastElementString(), markElement.FastElementString());
            }
            else
            {
                cardImage.sprite =
                    ImageHelper.GetPendulumImage(markElement.FastElementString(), costElement.FastElementString());
            }
        }
        else
        {
            cardImage.sprite = ImageHelper.GetCardImage(imageId);
        }
    }
    
    private void ShouldShowUsableGlow(ShouldShowUsableEvent shouldShowUsableEvent)
    {
        if (!shouldShowUsableEvent.Owner.Equals(_id.owner))
        {
            return;
        }

        validTargetGlow.color = shouldShowUsableEvent.QuantaCheck(_card.costElement, _card.cost)
            ? new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
            : new Color(0, 0, 0, 0);
    }

    private void HideUsableGlow(HideUsableDisplayEvent hideUsableDisplayEvent)
    {
        if (_id.owner.Equals(OwnerEnum.Opponent)) return;
        validTargetGlow.color = new Color(0, 0, 0, 0);
    }

    private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
    {
        if (!clearCardDisplayEvent.Id.Equals(_id)) return;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventBus<UpdateHandDisplayEvent>.Raise(new UpdateHandDisplayEvent(_id.owner));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventBus<CardTappedEvent>.Raise(new CardTappedEvent(_id, _card));
    }

    private void PlayCardFromShortcut(HandShortcutEvent handShortcutEvent)
    {
        if (!_id.Equals(handShortcutEvent.Id)) return;
        
        EventBus<CardTappedEvent>.Raise(new CardTappedEvent(_id, _card));
    }

    public Card GetCard() => _card;
    public ID GetId() => _id;
}