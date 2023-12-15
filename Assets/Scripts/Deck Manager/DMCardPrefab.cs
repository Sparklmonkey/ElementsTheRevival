using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DmCardPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Card cardToShow;
    [SerializeField]
    private TextMeshProUGUI cardName, cardCount;
    [SerializeField]
    private Image cardElement, cardImage, uppedShine, rareIndicator;
    [SerializeField]
    private Button actionButton;

    [SerializeField]
    private TMP_FontAsset underlayBlack, underlayWhite;

    private DeckDisplayManager _deckDisplayManager;
    private int _cardCountValue = 0;
    public Card GetCard() => cardToShow;
    public void SetupCardHead(Card card, DeckDisplayManager deckDisplayManager)
    {
        this._deckDisplayManager = deckDisplayManager;
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
        uppedShine.gameObject.SetActive(card.iD.IsUpgraded());
        rareIndicator.gameObject.SetActive(card.IsRare());
        //actionButton.onClick.AddListener(delegate { this.deckDisplayManager.ChangeParentContentView(transform); });
    }

    public void AddCard()
    {
        _cardCountValue++;
        cardCount.text = _cardCountValue.ToString();
    }

    public void RemoveCard()
    {
        _cardCountValue--;
        cardCount.text = _cardCountValue.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _deckDisplayManager.ShowCardDisplay(cardToShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _deckDisplayManager.HideCardDisplay();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _deckDisplayManager.ChangeParentContentView(this.transform);
    }

}
