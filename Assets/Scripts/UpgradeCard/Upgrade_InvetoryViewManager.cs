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
            if (card.element != filter) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow);

    }

    public void DisplayCardAndUp(Card card)
    {
        Card upgradedCard = card.upgradedVersion;

        currentCardDisplay.gameObject.SetActive(true);
        currentCardDisplay.SetupCardView(card, null);
        if (upgradedCard == null) { return; }
        upgradedCardDisplay.gameObject.SetActive(true);
        upgradedCardDisplay.SetupCardView(upgradedCard, null);
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
        ClearContentView();
        foreach (Card card in cardList)
        {
            GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryView);
            cardHeadObject.GetComponent<UCardHead>().SetupCardHead(card, this);
        }
    }

    public void ClearContentView()
    {
        List<UCardHead> children = new List<UCardHead>(inventoryView.GetComponentsInChildren<UCardHead>());
        foreach (UCardHead child in children)
        {
            Destroy(child.gameObject);
        }
    }

}
