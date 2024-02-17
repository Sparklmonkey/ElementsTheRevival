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
            if (card.CardElement != filter) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow, false);
    }
}
