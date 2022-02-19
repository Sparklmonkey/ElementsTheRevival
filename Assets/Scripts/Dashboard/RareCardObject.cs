using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RareCardObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image cardImage, backgroundColor;
    [SerializeField]
    private TextMeshProUGUI cardName;

    private CardDisplay cardDisplay;
    private Dash_QuestManager questManager;
    private Card rareCard;
    public void SetupSelection(Card rareCard, Dash_QuestManager questManager, CardDisplay cardDisplay)
    {
        this.rareCard = rareCard;
        this.cardDisplay = cardDisplay;
        this.questManager = questManager;
        cardImage.sprite = rareCard.cardImage;//ImageHelper.GetCardImage(rareCard.imageID);
        backgroundColor.sprite = ImageHelper.GetCardBackGroundImage(rareCard.element.FastElementString());
        cardName.text = rareCard.name;
    }

    public void SelectCard()
    {
        PlayerData.shared.cardInventory.Add(new CardObject(rareCard.name, rareCard.type.FastCardTypeString(), !rareCard.isUpgradable));
        PlayerData.SaveData();
        PlayerPrefs.SetFloat("ShouldShowRareCard", 2);
        questManager.SetupQuestPanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.SetupCardView(rareCard, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardDisplay.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerData.shared.cardInventory.Add(new CardObject(rareCard.name, rareCard.type.FastCardTypeString(), !rareCard.isUpgradable));
        PlayerPrefs.SetFloat("ShouldShowRareCard", 2);
        questManager.SetupQuestPanel();
    }
}
