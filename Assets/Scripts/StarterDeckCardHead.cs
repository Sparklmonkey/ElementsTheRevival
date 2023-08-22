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
    private CardDisplay cardDisplay;
    public void SetupCardHead(Card card, CardDisplay cardDisplay)
    {
        this.cardDisplay = cardDisplay;
        cardName.text = card.cardName;
        cardName.font = underlayBlack;
        cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        cardElement.sprite = ImageHelper.GetCardBackGroundImage(card.costElement.ToString());
        cardImage.sprite = ImageHelper.GetCardImage(card.imageID);
        cardToShow = card;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.SetupCardView(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardDisplay.gameObject.SetActive(false);
    }

}

