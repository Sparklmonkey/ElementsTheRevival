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
    private TextMeshProUGUI cardName, cardCount;
    [SerializeField]
    private Image cardElement, cardImage, uppedShine, rareIndicator;

    [SerializeField]
    private TMP_FontAsset underlayBlack, underlayWhite;
    private int cardCountValue;
    private Upgrade_InvetoryViewManager inventoryManager;
    public Card GetCard() => cardToShow;
    public void SetupCardHead(Card card, Upgrade_InvetoryViewManager inventoryManager)
    {
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
        cardElement.sprite = ImageHelper.GetCardBackGroundImage(card.costElement.FastElementString());
        cardToShow = card;
        cardCountValue = 1;
        cardCount.text = cardCountValue.ToString();
        this.inventoryManager = inventoryManager;
        uppedShine.gameObject.SetActive(card.iD.IsUpgraded());
        rareIndicator.gameObject.SetActive(card.IsRare());
    }

    public void AddCard()
    {
        cardCountValue++;
        cardCount.text = cardCountValue.ToString();
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
