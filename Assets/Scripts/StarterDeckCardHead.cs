using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarterDeckCardHead : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Card cardToShow;
    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private Image cardElement, cardImage;

    [SerializeField]
    private TMP_FontAsset underlayBlack;
    private CardDisplay _cardDisplay;
    public void SetupCardHead(Card card, CardDisplay cardDisplay)
    {
        _cardDisplay = cardDisplay;
        cardName.text = card.CardName;
        cardName.font = underlayBlack;
        cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        cardElement.sprite = ImageHelper.GetCardBackGroundImage(card.CardElement.ToString());
        cardImage.sprite = card.cardImage;
        cardToShow = card;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardDisplay.gameObject.SetActive(true);
        _cardDisplay.SetupCardView(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardDisplay.gameObject.SetActive(false);
    }

}

