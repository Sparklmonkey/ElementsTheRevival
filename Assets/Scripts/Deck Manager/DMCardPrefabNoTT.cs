using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DMCardPrefabNoTT : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Card cardToShow;
    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private Image cardElement;
    public bool isInventory;

    private InventoryManager inventoryManager;
    public Card GetCard() => cardToShow;
    public void SetupCardHead(Card card, bool isInventory, InventoryManager inventoryManager)
    {
        cardName.text = card.name;

        string backGroundString = card.name == "Animate Weapon" ? "Air" :
                                card.name == "Luciferin" || card.name == "Luciferase" ? "Light" :
                                card.element.ToString();
        cardElement.sprite = ImageHelper.GetCardHeadBackground(backGroundString);
        cardToShow = card;
        this.isInventory = isInventory;
        this.inventoryManager = inventoryManager;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.ShowCardDisplay(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.HideCardDisplay();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryManager.ChangeCardOwner(this);
    }
}
