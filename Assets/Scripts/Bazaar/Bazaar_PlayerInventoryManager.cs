using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bazaar_PlayerInventoryManager : InventoryManager
{
    [SerializeField]
    private TextMeshProUGUI cardCount;
    [SerializeField]
    private List<Card> testList;
    [SerializeField]
    private GameObject touchBlocker;
    private int selectedElement;
    private List<Card> cardList;

    public void SetupPlayerInvetoryView(List<Card> cardList)
    {
        this.cardList = cardList;
        cardList.Sort((x, y) => string.Compare(x.iD, y.iD));
        SetupContentView(cardList, true);
    }

    public async void GoToDeckManagement()
    {
        if (ApiManager.isTrainer)
        {
            SceneTransitionManager.Instance.LoadScene("DeckManagement");
            return;
        }

        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        await ApiManager.Instance.SaveDataToUnity();
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        DeckDisplayManager.isArena = false;
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }

    public void UpdateCardFilter(int element)
    {
        if (element == selectedElement) { return; }

        if(element == 14)
        {
            SetupPlayerInvetoryView(PlayerData.shared.cardInventory.DeserializeCard());
            return;
        }
        selectedElement = element;
        Element filter = (Element)selectedElement;
        List<Card> cardsToShow = new();
        foreach (Card card in cardList)
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
            if (card.costElement != filter || card.BuyPrice == 0) { continue; }
            cardsToShow.Add(card);
        }
        SetupContentView(cardsToShow, true);
    }

    public async void GoToDashboard()
    {
        if (ApiManager.isTrainer)
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
        }


        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
        await ApiManager.Instance.SaveDataToUnity();
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }

}
