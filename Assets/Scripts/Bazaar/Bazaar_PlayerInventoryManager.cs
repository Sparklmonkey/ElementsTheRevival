using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public void GoToDeckManagement()
    {
        if (ApiManager.isTrainer)
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckManagement");
        }
        if (PlayerPrefs.GetInt("IsGuest") == 1)
        {
            PlayerData.SaveData();
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckManagement");
        }
        else
        {
            //bazaarBtn.interactable = false;
            touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            StartCoroutine(ApiManager.shared.SaveToApi(AccountBazaarSuccess, AccountBazaarSuccess));
        }
        return;

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

    private void AccountBazaarSuccess(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        //bazaarBtn.interactable = true;
        DeckDisplayManager.isArena = false;
        SceneManager.LoadScene("DeckManagement");
    }


    public void GoToDashboard()
    {
        if (ApiManager.isTrainer)
        {
            GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
        }
        if (PlayerPrefs.GetInt("IsGuest") == 1)
        {
            PlayerData.SaveData();
            GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
        }
        else
        {
            //bazaarBtn.interactable = false;
            touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            StartCoroutine(ApiManager.shared.SaveToApi(AccountDashboardSuccess, AccountDashboardSuccess));
        }
    }
    private void AccountDashboardSuccess(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        //bazaarBtn.interactable = true;
        SceneManager.LoadScene("Dashboard");
    }

}
