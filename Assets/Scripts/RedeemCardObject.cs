    using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RedeemCardObject : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI cardName;
    public Image cardImage, cardHeadBackground;
    private Card _cardOnDisplay;

    public DashCodeRedemption manager;

    public void SetupObject(Card card, DashCodeRedemption delegateManager)
    {
        _cardOnDisplay = card;
        manager = delegateManager;
        cardName.text = card.CardName;
        cardImage.sprite = card.cardImage;
        cardHeadBackground.sprite = ImageHelper.GetCardHeadBackground(card.CostElement.FastElementString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.DisplayCardDetail(_cardOnDisplay);
    }
}
