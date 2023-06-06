using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazaar_ShopInventoryManager : InventoryManager
{

    private int selectedElement = 12;
    private List<Card> inventoryCardList;

    public void SetupInitialCardView(List<Card> cardList)
    {
        inventoryCardList = CardDatabase.Instance.GetAllBazaarCards();
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
            if (card.cardName == "Animate Weapon")
            {
                if (filter.Equals(Element.Air))
                {
                    cardsToShow.Add(card);
                }
                continue;
            }
            if ((card.cardName == "Luciferin" || card.cardName == "Luciferase"))
            {
                if (filter.Equals(Element.Light))
                {
                    cardsToShow.Add(card);
                }
                continue;
            }
            if (card.costElement != filter) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow, false);
    }
}
