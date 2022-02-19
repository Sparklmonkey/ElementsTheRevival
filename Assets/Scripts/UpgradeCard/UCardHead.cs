using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UCardHead : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Card cardToShow;
    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private Image cardElement;

    private Upgrade_InvetoryViewManager inventoryManager;
    public Card GetCard() => cardToShow;
    public void SetupCardHead(Card card, Upgrade_InvetoryViewManager inventoryManager)
    {
        cardName.text = card.name;
        cardElement.sprite = ImageHelper.GetCardBackGroundImage(card.element.FastElementString());
        cardToShow = card;
        this.inventoryManager = inventoryManager;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.DisplayCardAndUp(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.HideCardAndUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryManager.UpgradeCard(cardToShow);
    }
}
