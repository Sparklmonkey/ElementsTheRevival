using System;
using Battlefield.Abstract;
using Core.Helpers;
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
    private EventBinding<UpdatePrecogEvent> _updatePreCogEventBinding;

    private bool isPrecogCard;
    private void OnDisable()
    {
        EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
        EventBus<UpdateCardDisplayEvent>.Unregister(_updateCardDisplayBinding);
        EventBus<UpdatePrecogEvent>.Unregister(_updatePreCogEventBinding);
    }

    private void OnEnable()
    {
        _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
        EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
        _updateCardDisplayBinding = new EventBinding<UpdateCardDisplayEvent>(DisplayCard);
        EventBus<UpdateCardDisplayEvent>.Register(_updateCardDisplayBinding);
        _updatePreCogEventBinding = new EventBinding<UpdatePrecogEvent>(UpdateForPrecog);
        EventBus<UpdatePrecogEvent>.Register(_updatePreCogEventBinding);
    }

    private void UpdateForPrecog(UpdatePrecogEvent  updatePrecogEvent)
    {
        if (!updatePrecogEvent.Id.Equals(Id)) return;
        isPrecogCard = true;
        isHidden.color = ElementColours.GetInvisibleColor();
        
        SetCardImage(Card.cardImage, Card.CardName.Contains("Pendulum"),
            Card.CostElement == Card.SkillElement,
            Card.CostElement);
        cardName.text = Card.CardName;
        cardName.font = Card.Id.IsUpgraded() ? underlayWhite : underlayBlack;
        cardName.color = Card.Id.IsUpgraded()
            ? ElementColours.GetBlackColor()
            : ElementColours.GetWhiteColor();

        cardCost.text = Card.Cost.ToString();
        cardElement.sprite = ImageHelper.GetElementImage(Card.CostElement.ToString());

        cardElement.color = ElementColours.GetWhiteColor();


        cardBackground.sprite = ImageHelper.GetCardBackGroundImage(Card.CardElement.ToString());

        if (Card.Cost == 0)
        {
            cardCost.text = "";
            cardElement.color = ElementColours.GetInvisibleColor();
        }

        if (Card.CostElement.Equals(Element.Other))
        {
            cardElement.color = ElementColours.GetInvisibleColor();
        }

        cardHolder.SetActive(true);
    }
    
    private void DisplayCard(UpdateCardDisplayEvent updateCardDisplayEvent)
    {
        if (!updateCardDisplayEvent.Id.Equals(Id)) return;

        SetCard(updateCardDisplayEvent.Card);
        isHidden.color = ElementColours.GetInvisibleColor();
        cardHolder.SetActive(true);
        if (Id.IsOwnedBy(OwnerEnum.Opponent))
        {
            isHidden.color = ElementColours.GetWhiteColor();
            cardHolder.SetActive(false);
        }

        cardName.text = updateCardDisplayEvent.Card.CardName;
        cardName.font = updateCardDisplayEvent.Card.Id.IsUpgraded() ? underlayWhite : underlayBlack;
        cardName.color = updateCardDisplayEvent.Card.Id.IsUpgraded()
            ? ElementColours.GetBlackColor()
            : ElementColours.GetWhiteColor();

        cardCost.text = updateCardDisplayEvent.Card.Cost.ToString();
        cardElement.sprite = ImageHelper.GetElementImage(updateCardDisplayEvent.Card.CostElement.ToString());

        cardElement.color = ElementColours.GetWhiteColor();


        cardBackground.sprite = ImageHelper.GetCardBackGroundImage(updateCardDisplayEvent.Card.CardElement.ToString());

        if (updateCardDisplayEvent.Card.Cost == 0)
        {
            cardCost.text = "";
            cardElement.color = ElementColours.GetInvisibleColor();
        }

        if (updateCardDisplayEvent.Card.CostElement.Equals(Element.Other))
        {
            cardElement.color = ElementColours.GetInvisibleColor();
        }

        SetCardImage(updateCardDisplayEvent.Card.cardImage, updateCardDisplayEvent.Card.CardName.Contains("Pendulum"),
            updateCardDisplayEvent.Card.CostElement == updateCardDisplayEvent.Card.SkillElement,
            updateCardDisplayEvent.Card.CostElement);
    }

    private void SetCardImage(Sprite sprite, bool isPendulum, bool shouldShowMarkElement, Element costElement)
    {
        if (isPendulum)
        {
            var markElement = PlayerData.Shared.markElement;
            cardImage.sprite = !shouldShowMarkElement ? ImageHelper.GetPendulumImage(costElement.FastElementString(), markElement.FastElementString()) : ImageHelper.GetPendulumImage(markElement.FastElementString(), costElement.FastElementString());
        }
        else
        {
            cardImage.sprite = sprite;
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