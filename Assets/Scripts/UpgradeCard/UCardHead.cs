using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UCardHead : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Card cardToShow;
    [SerializeField]
    private TextMeshProUGUI cardName, cardCount;
    [SerializeField]
    private Image cardElement, cardImage, uppedShine, rareIndicator;

    [SerializeField]
    private TMP_FontAsset underlayBlack, underlayWhite;
    private int _cardCountValue;
    private UpgradeInvetoryViewManager _inventoryManager;
    public Card GetCard() => cardToShow;
    public void SetupCardHead(Card card, UpgradeInvetoryViewManager inventoryManager)
    {
        cardName.text = card.CardName;

        if (card.Id.IsUpgraded())
        {
            cardName.font = underlayWhite;
            cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
        }
        else
        {
            cardName.font = underlayBlack;
            cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }
        cardElement.sprite = ImageHelper.GetCardBackGroundImage(card.CostElement.FastElementString());
        cardToShow = card;
        _cardCountValue = 1;
        cardCount.text = _cardCountValue.ToString();
        this._inventoryManager = inventoryManager;
        uppedShine.gameObject.SetActive(card.Id.IsUpgraded());
        rareIndicator.gameObject.SetActive(card.IsRare());
    }

    public void AddCard()
    {
        _cardCountValue++;
        cardCount.text = _cardCountValue.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryManager.DisplayCardAndUp(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inventoryManager.HideCardAndUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventoryManager.UpgradeCard(cardToShow);
    }
}
