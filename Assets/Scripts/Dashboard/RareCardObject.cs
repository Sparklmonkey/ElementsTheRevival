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

    private CardDisplay _cardDisplay;
    private DashQuestManager _questManager;
    private Card _rareCard;
    public void SetupSelection(Card rareCard, DashQuestManager questManager, CardDisplay cardDisplay)
    {
        this._rareCard = rareCard;
        this._cardDisplay = cardDisplay;
        this._questManager = questManager;
        cardImage.sprite = ImageHelper.GetCardImage(rareCard.imageID);
        backgroundColor.sprite = ImageHelper.GetCardBackGroundImage(rareCard.costElement.FastElementString());
        cardName.text = rareCard.cardName;
    }

    public void SelectCard()
    {
        PlayerData.Shared.inventoryCards.Add(_rareCard.iD);
        PlayerData.SaveData();
        PlayerPrefs.SetFloat("ShouldShowRareCard", 2);
        _questManager.SetupQuestPanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardDisplay.gameObject.SetActive(true);
        _cardDisplay.SetupCardView(_rareCard);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardDisplay.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerData.Shared.inventoryCards.Add(_rareCard.iD);
        PlayerPrefs.SetFloat("ShouldShowRareCard", 2);
        _questManager.SetupQuestPanel();
    }
}
