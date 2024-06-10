using System.Collections.Generic;
using Networking;
using TMPro;
using UnityEngine;

public class BazaarPlayerInventoryManager : InventoryManager
{
    [SerializeField]
    private TextMeshProUGUI cardCount;
    [SerializeField]
    private List<Card> testList;
    [SerializeField]
    private GameObject touchBlocker;
    private int _selectedElement;
    private List<Card> _cardList;

    public void SetupPlayerInvetoryView(List<Card> cardList)
    {
        this._cardList = cardList;
        cardList.Sort((x, y) => string.Compare(x.Id, y.Id));
        SetupContentView(cardList, true);
    }

    public async void GoToDeckManagement()
    {
        if (ApiManager.IsTrainer)
        {
            SceneTransitionManager.Instance.LoadScene("DeckManagement");
            return;
        }

        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        await ApiManager.Instance.SaveGameData();
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        DeckDisplayManager.IsArena = false;
        SceneTransitionManager.Instance.LoadScene("DeckManagement");
    }

    public void UpdateCardFilter(int element)
    {
        if (element == _selectedElement) { return; }

        if (element == 14)
        {
            SetupPlayerInvetoryView(PlayerData.Shared.GetInventory().DeserializeCard());
            return;
        }
        _selectedElement = element;
        var filter = (Element)_selectedElement;
        List<Card> cardsToShow = new();
        foreach (var card in _cardList)
        {
            if (card.CardElement != filter || card.BuyPrice == 0) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow, true);
    }

    public async void GoToDashboard()
    {
        if (ApiManager.IsTrainer)
        {
            SceneTransitionManager.Instance.LoadScene("Dashboard");
        }


        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        await ApiManager.Instance.SaveGameData();
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }

}
