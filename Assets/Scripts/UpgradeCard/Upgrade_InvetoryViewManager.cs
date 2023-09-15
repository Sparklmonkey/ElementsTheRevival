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

        Element filter = (Element)_selectedElement;
        List<Card> cardsToShow = new List<Card>();
        foreach (Card card in _inventoryCardList)
        {
            if (card.costElement != filter) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow);

    }

    public void DisplayCardAndUp(Card card)
    {
        Card upgradedCard = CardDatabase.Instance.GetCardFromId(card.iD.GetUppedRegular());

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
        cardList.Sort((x, y) => string.Compare(x.iD, y.iD));
        foreach (Card card in cardList)
        {
            UCardHead dMCardPrefab = _cardHeads.Find(x => x.GetCard().cardName == card.cardName);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryView);
                cardHeadObject.GetComponent<UCardHead>().SetupCardHead(card, this);
                _cardHeads.Add(cardHeadObject.GetComponent<UCardHead>());
            }
        }
    }

    public void ClearContentView()
    {
        _cardHeads = new List<UCardHead>();
        List<UCardHead> children = new List<UCardHead>(inventoryView.GetComponentsInChildren<UCardHead>());
        foreach (UCardHead child in children)
        {
            Destroy(child.gameObject);
        }
    }

}
