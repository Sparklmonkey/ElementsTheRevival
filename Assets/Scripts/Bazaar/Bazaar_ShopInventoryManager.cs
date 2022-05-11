using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazaar_ShopInventoryManager : InventoryManager
{

    private int selectedElement = 12;
    private List<Card> inventoryCardList;

    public void SetupInitialCardView(List<Card> cardList)
    {
        inventoryCardList = CardDatabase.GetAllCards();
        UpdateCardFilter(0);
    }

    public void UpdateCardFilter(int element)
    {
        if(element == selectedElement) { return; }


        selectedElement = element;
        Element filter = (Element)selectedElement;
        List<Card> cardsToShow = new List<Card>();
        foreach (Card card in inventoryCardList)
        {
            if (card.name == "Animate Weapon" && !filter.Equals(Element.Air)) { continue; }
            if (card.name == "Luciferin" || card.name == "Luciferase" && !filter.Equals(Element.Light)) { continue; }
            if (card.element != filter || card.buyPrice == 0) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow, false);
    }
}
