using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        cardImage.sprite = ImageHelper.GetCardImage(rareCard.imageID);
        backgroundColor.sprite = ImageHelper.GetCardBackGroundImage(rareCard.costElement.FastElementString());
        cardName.text = rareCard.cardName;
    }

    public void SelectCard()
    {
        PlayerData.shared.cardInventory.Add(rareCard.iD);
        PlayerData.SaveData();
        PlayerPrefs.SetFloat("ShouldShowRareCard", 2);
        questManager.SetupQuestPanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.SetupCardView(rareCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardDisplay.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerData.shared.cardInventory.Add(rareCard.iD);
        PlayerPrefs.SetFloat("ShouldShowRareCard", 2);
        questManager.SetupQuestPanel();
    }
}
