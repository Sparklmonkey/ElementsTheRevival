using System;
using Battlefield.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardDisplay : CardFieldDisplay
{
    [SerializeField] private Image cardBackground, cardImage, cardElement, isHidden;
    [SerializeField] private TextMeshProUGUI cardName, cardCost;
    [SerializeField] private GameObject cardHolder;
    public TMP_FontAsset underlayBlack, underlayWhite;

    private EventBinding<ClearCardDisplayEvent> _clearCardDisplayBinding;
    private EventBinding<UpdateCardDisplayEvent> _updateCardDisplayBinding;

    private void OnDisable()
    {
        EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
        EventBus<UpdateCardDisplayEvent>.Unregister(_updateCardDisplayBinding);
    }

    private void OnEnable()
    {
        _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
        EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
        _updateCardDisplayBinding = new EventBinding<UpdateCardDisplayEvent>(DisplayCard);
        EventBus<UpdateCardDisplayEvent>.Register(_updateCardDisplayBinding);
    }

    private void DisplayCard(UpdateCardDisplayEvent updateCardDisplayEvent)
    {
        if (!updateCardDisplayEvent.Id.Equals(Id)) return;

        SetCard(updateCardDisplayEvent.Card);
        isHidden.color = ElementColours.GetInvisibleColor();
        cardHolder.SetActive(true);
        if (Id.owner.Equals(OwnerEnum.Opponent))
        {
            isHidden.color = ElementColours.GetWhiteColor();
            cardHolder.SetActive(false);
        }

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
            cardImage.sprite = !shouldShowMarkElement ? ImageHelper.GetPendulumImage(costElement.FastElementString(), markElement.FastElementString()) : ImageHelper.GetPendulumImage(markElement.FastElementString(), costElement.FastElementString());
        }
        else
        {
            cardImage.sprite = ImageHelper.GetCardImage(imageId);
        }
    }
    
    private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
    {
        if (!clearCardDisplayEvent.Id.Equals(Id)) return;
        EventBus<RemoveCardFromManagerEvent>.Raise(new RemoveCardFromManagerEvent(Id));
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventBus<UpdateHandDisplayEvent>.Raise(new UpdateHandDisplayEvent(Id.owner));
    }
}