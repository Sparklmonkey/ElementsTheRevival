using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DmCardPrefabNoTt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Card cardToShow;
    [SerializeField]
    private TextMeshProUGUI cardName, cardCount;
    [SerializeField]
    private Image cardElement, cardImage, uppedShine, rareIndicator;
    public bool isInventory;

    [SerializeField]
    private TMP_FontAsset underlayBlack, underlayWhite;
    private int _cardCountValue = 0;
    private InventoryManager _inventoryManager;
    public Card GetCard() => cardToShow;

    public void SetupCardHead(Card card, bool isInventory, InventoryManager inventoryManager)
    {
        this._inventoryManager = inventoryManager;
        this.isInventory = isInventory;
        cardName.text = card.cardName;

        if (card.iD.IsUpgraded())
        {
            cardName.font = underlayWhite;
            cardName.color = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
        }
        else
        {
            cardName.font = underlayBlack;
            cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

        var backGroundString = card.cardName == "Animate Weapon" ? "Air" :
                                card.cardName == "Luciferin" || card.cardName == "Luciferase" ? "Light" :
                                card.costElement.ToString();
        cardElement.sprite = ImageHelper.GetCardBackGroundImage(backGroundString);
        cardImage.sprite = ImageHelper.GetCardImage(card.imageID);
        cardToShow = card;
        _cardCountValue = 1;
        cardCount.text = _cardCountValue.ToString();
        if (!isInventory)
        {
            cardCount.gameObject.SetActive(false);
        }

        uppedShine.gameObject.SetActive(card.iD.IsUpgraded());
        rareIndicator.gameObject.SetActive(card.IsRare());
        //actionButton.onClick.AddListener(delegate { this.deckDisplayManager.ChangeParentContentView(transform); });
    }

    public void AddCard()
    {
        _cardCountValue++;
        cardCount.text = _cardCountValue.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryManager.ShowCardDisplay(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inventoryManager.HideCardDisplay();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventoryManager.ChangeCardOwner(this);
    }
}
