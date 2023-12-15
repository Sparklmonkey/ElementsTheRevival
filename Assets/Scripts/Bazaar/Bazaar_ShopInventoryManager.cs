using System.Collections.Generic;

public class BazaarShopInventoryManager : InventoryManager
{

    private int _selectedElement = 12;
    private List<Card> _inventoryCardList;

    public void SetupInitialCardView()
    {
        _inventoryCardList = CardDatabase.Instance.GetAllBazaarCards();
        UpdateCardFilter(0);
    }

    public void UpdateCardFilter(int element)
    {
        if (element == _selectedElement) { return; }


        _selectedElement = element;
        var filter = (Element)_selectedElement;
        var cardsToShow = new List<Card>();
        foreach (var card in _inventoryCardList)
        {
            if (card.cardName == "Animate Weapon")
            {
                if (filter.Equals(Element.Air))
                {
                    cardsToShow.Add(card);
                }
                continue;
            }
            if (card.cardName == "Luciferin" || card.cardName == "Luciferase")
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
