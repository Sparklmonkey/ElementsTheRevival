using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_InvetoryViewManager : MonoBehaviour
{
    [SerializeField]
    private CardDisplay currentCardDisplay, upgradedCardDisplay;
    [SerializeField]
    private Transform inventoryView;
    [SerializeField]
    private GameObject cardHeadPrefab;
    private int selectedElement = 12;
    private List<Card> inventoryCardList;
    private List<UCardHead> cardHeads;

    public void SetupInitialCardView(List<Card> cardList)
    {
        inventoryCardList = cardList;
        selectedElement = 12;
        UpdateCardFilter(14);
    }

    public void UpdateCardFilter(int element)
    {
        if (selectedElement == element) { return; }
        selectedElement = element;
        if (element == 14)
        {
            SetupContentView(inventoryCardList);
            return;
        }

        Element filter = (Element)selectedElement;
        List<Card> cardsToShow = new List<Card>();
        foreach (Card card in inventoryCardList)
        {
            if (card.costElement != filter) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow);

    }

    public void DisplayCardAndUp(Card card)
    {
        Card upgradedCard = CardDatabase.GetCardFromId(card.iD.GetUppedRegular());

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
        GetComponent<Upgrade_PlayerDataManager>().UpgradeCardInInventory(cardToUpgrade);
    }

    public void SetupContentView(List<Card> cardList)
    {
        HideCardAndUp();
        ClearContentView();
        cardList.Sort((x, y) => string.Compare(x.iD, y.iD));
        foreach (Card card in cardList)
        {
            UCardHead dMCardPrefab = cardHeads.Find(x => x.GetCard().cardName == card.cardName);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryView);
                cardHeadObject.GetComponent<UCardHead>().SetupCardHead(card, this);
                cardHeads.Add(cardHeadObject.GetComponent<UCardHead>());
            }
        }
    }

    public void ClearContentView()
    {
        cardHeads = new List<UCardHead>();
        List<UCardHead> children = new List<UCardHead>(inventoryView.GetComponentsInChildren<UCardHead>());
        foreach (UCardHead child in children)
        {
            Destroy(child.gameObject);
        }
    }

}
