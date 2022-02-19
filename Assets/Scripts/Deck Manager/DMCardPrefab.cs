using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DMCardPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Card cardToShow;
    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private Image cardElement;
    [SerializeField]
    private Button actionButton;

    private DeckDisplayManager deckDisplayManager;

    public Card GetCard() => cardToShow;
    public void SetupCardHead(Card card, DeckDisplayManager deckDisplayManager)
    {
        this.deckDisplayManager = deckDisplayManager;
        cardName.text = card.name;

        string backGroundString = card.name == "Animate Weapon" ? "Air" :
                                card.name == "Luciferin" || card.name == "Luciferase" ? "Light" :
                                card.element.ToString();
        cardElement.sprite = ImageHelper.GetCardHeadBackground(backGroundString);
        cardToShow = card;
        //actionButton.onClick.AddListener(delegate { this.deckDisplayManager.ChangeParentContentView(transform); });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        deckDisplayManager.ShowCardDisplay(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        deckDisplayManager.HideCardDisplay();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        deckDisplayManager.ChangeParentContentView(this.transform);
    }

}
