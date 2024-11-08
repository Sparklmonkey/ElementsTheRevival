using System.Collections.Generic;
using UnityEngine;

public class UpgradeInvetoryViewManager : MonoBehaviour
{
    [SerializeField]
    private CardDisplay currentCardDisplay, upgradedCardDisplay;
    [SerializeField]
    private Transform inventoryView;
    [SerializeField]
    private GameObject cardHeadPrefab;
    private int _selectedElement = 12;
    private List<Card> _inventoryCardList;
    private List<UCardHead> _cardHeads;

    public void SetupInitialCardView(List<Card> cardList)
    {
        _inventoryCardList = cardList;
        _selectedElement = 12;
        UpdateCardFilter(14);
    }

    public void UpdateCardFilter(int element)
    {
        if (_selectedElement == element) { return; }
        _selectedElement = element;
        if (element == 14)
        {
            SetupContentView(_inventoryCardList);
            return;
        }

        var filter = (Element)_selectedElement;
        var cardsToShow = new List<Card>();
        foreach (var card in _inventoryCardList)
        {
            if (card.CostElement != filter) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow);

    }

    public void DisplayCardAndUp(Card card)
    {
        var upgradedCard = CardDatabase.Instance.GetCardFromId(card.Id.GetUppedRegular());

        currentCardDisplay.gameObject.SetActive(true);
        currentCardDisplay.SetupCardView(card);
        if (upgradedCard == null) { return; }
        upgradedCardDisplay.gameObject.SetActive(true);
        upgradedCardDisplay.SetupCardView(upgradedCard);
    }
    public void HideCardAndUp()
    {
        currentCardDisplay.gameObject.SetActive(false);
        upgradedCardDisplay.gameObject.SetActive(false);
    }

    public void UpgradeCard(Card cardToUpgrade)
    {
        GetComponent<UpgradePlayerDataManager>().UpgradeCardInInventory(cardToUpgrade);
    }

    public void SetupContentView(List<Card> cardList)
    {
        HideCardAndUp();
        ClearContentView();
        cardList.Sort((x, y) => string.Compare(x.Id, y.Id));
        foreach (var card in cardList)
        {
            var dMCardPrefab = _cardHeads.Find(x => x.GetCard().Id == card.Id);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                var cardHeadObject = Instantiate(cardHeadPrefab, inventoryView);
                cardHeadObject.GetComponent<UCardHead>().SetupCardHead(card, this);
                _cardHeads.Add(cardHeadObject.GetComponent<UCardHead>());
            }
        }
    }

    public void ClearContentView()
    {
        _cardHeads = new List<UCardHead>();
        var children = new List<UCardHead>(inventoryView.GetComponentsInChildren<UCardHead>());
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }
    }

}
