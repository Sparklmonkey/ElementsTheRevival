using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RedeemCardObject : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI cardName;
    public Image cardImage, cardHeadBackground;
    private Card cardOnDisplay;

    public Dash_CodeRedemption manager;

    public void SetupObject(Card card, Dash_CodeRedemption manager)
    {
        cardOnDisplay = card;
        this.manager = manager;
        cardName.text = card.cardName;
        cardImage.sprite = ImageHelper.GetCardImage(card.imageID);
        cardHeadBackground.sprite = ImageHelper.GetCardHeadBackground(card.costElement.FastElementString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.DisplayCardDetail(cardOnDisplay);
    }
}
