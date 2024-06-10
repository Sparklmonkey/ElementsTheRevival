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
        cardImage.sprite = rareCard.cardImage;
        backgroundColor.sprite = ImageHelper.GetCardBackGroundImage(rareCard.CostElement.FastElementString());
        cardName.text = rareCard.CardName;
    }

    public void SelectCard()
    {
        var invent = PlayerData.Shared.GetInventory();
        invent.Add(_rareCard.Id);
        PlayerData.Shared.SetInventory(invent);
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
        var invent = PlayerData.Shared.GetInventory();
        invent.Add(_rareCard.Id);
        PlayerData.Shared.SetInventory(invent);
        PlayerPrefs.SetFloat("ShouldShowRareCard", 2);
        _questManager.SetupQuestPanel();
    }
}
