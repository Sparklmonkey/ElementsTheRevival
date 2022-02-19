using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DeckDisplayManager : MonoBehaviour
{
    [SerializeField]
    private List<Card> playerDeck, playerInverntory;

    [SerializeField]
    private TextMeshProUGUI deckCount, inventoryCount;

    [SerializeField]
    private GameObject cardHeadPrefab;

    [SerializeField]
    private Transform deckContentView, inventoryContentView;
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField]
    private ErrorMessageManager errorMessage;
    [SerializeField]
    private DM_MarkManager markManager;

    private GameObject touchBlocker;

    private int currentFilterSelection = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerDeck = PlayerData.shared.currentDeck.DeserializeCard();
        playerInverntory = PlayerData.shared.cardInventory.DeserializeCard();
        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInverntory.Count} Cards ) ";
        markManager.SetupMarkCard((int)PlayerData.shared.markElement);
        foreach (Card deckCard in playerDeck)
        {
            GameObject cardHeadObject = Instantiate(cardHeadPrefab, deckContentView);
            cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(deckCard, this);
        }
        foreach (Card inventoryCard in playerInverntory)
        {
            GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
            cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(inventoryCard, this);
        }
    }

    public void FilterInventoryCardsByElement(int element)
    {
        if (currentFilterSelection == element) { return; }
        currentFilterSelection = element;
        ClearInventoryView();
        if(element == 14)
        {
            foreach (Card inventoryCard in playerInverntory)
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(inventoryCard, this);
            }
            return;
        }

        Element filter = (Element)element;
        foreach (Card inventoryCard in playerInverntory)
        {
            if (inventoryCard.name == "Animate Weapon" && !filter.Equals(Element.Air)) { continue; }
            if (inventoryCard.name == "Luciferin" || inventoryCard.name == "Luciferase" && !filter.Equals(Element.Light)) { continue; }
            if (!inventoryCard.element.Equals(filter)) { continue; }
            GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
            cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(inventoryCard, this);
        }
    }

    private void ClearInventoryView()
    {
        List<DMCardPrefab> children = new List<DMCardPrefab>(inventoryContentView.GetComponentsInChildren<DMCardPrefab>());
        foreach (DMCardPrefab child in children)
        {
            Destroy(child.gameObject);
        }
    }
    private void ClearDeckView()
    {
        List<DMCardPrefab> children = new List<DMCardPrefab>(deckContentView.GetComponentsInChildren<DMCardPrefab>());
        foreach (DMCardPrefab child in children)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveAllDeckCards()
    {
        List<DMCardPrefab> children = new List<DMCardPrefab> (deckContentView.GetComponentsInChildren<DMCardPrefab>());
        foreach (DMCardPrefab child in children)
        {
            playerInverntory.Add(child.GetCard());
            ClearDeckView();
        }
        FilterInventoryCardsByElement(14);
        playerDeck.Clear();
        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInverntory.Count} Cards ) ";
    }

    public void ChangeParentContentView(Transform transform)
    {
        Card card = transform.GetComponent<DMCardPrefab>().GetCard();
        if (transform.parent.name == "DeckContentView")
        {
            PlayerData.shared.removedCardFromDeck = true;
            playerDeck.Remove(card);
            playerInverntory.Add(card);
            transform.parent = inventoryContentView;
            deckCount.text = $"( {playerDeck.Count} Cards ) ";
            inventoryCount.text = $"( {playerInverntory.Count} Cards ) ";
            return;
        }

        if(playerDeck.FindAll(x => x.name == card.name).Count == 6 && !card.type.Equals(CardType.Pillar))
        {
            errorMessage.SetupErrorMessage("No more than 6 copies of each card(except pillars)");
            return;
        }

        playerInverntory.Remove(card);
        playerDeck.Add(card);
        transform.parent = deckContentView;
        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInverntory.Count} Cards ) ";
    }


    public void ShowCardDisplay(Card card)
    {
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.SetupCardView(card, null);
    }

    public void HideCardDisplay()
    {
        cardDisplay.gameObject.SetActive(false);
    }

    private void AccountSuccess(AccountResponse accountResponse)
    {
        Destroy(touchBlocker.gameObject);
        SceneManager.LoadScene("Dashboard");
    }
    private void AccountFailure(AccountResponse accountResponse)
    {
        Destroy(touchBlocker.gameObject);
        SceneManager.LoadScene("Dashboard");
    }

    public void SaveDeck()
    {
        if(playerDeck.Count >= 30 && playerDeck.Count <= 60)
        {
            PlayerData.shared.currentDeck = playerDeck.SerializeCard();
            PlayerData.shared.cardInventory = playerInverntory.SerializeCard();
            PlayerData.shared.markElement = markManager.GetMarkSelected();
            if (PlayerPrefs.GetString("IsOnline") == "True")
            {
                if (PlayerPrefs.GetInt("IsGuest") == 1)
                {
                    PlayerData.SaveData();
                    GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
                }
                else
                {
                    touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
                    StartCoroutine(ApiManager.shared.SaveToApi(AccountSuccess, AccountFailure));
                }
                return;
            }
            PlayerData.SaveData();
            SceneManager.LoadScene("Dashboard");
            return;
        }
        errorMessage.SetupErrorMessage("You deck has to have between 30 and 60 cards");
    }
    public void GoToBazaar()
    {
        if (playerDeck.Count >= 30 && playerDeck.Count <= 60)
        {
            PlayerData.shared.currentDeck = playerDeck.SerializeCard();
            PlayerData.shared.cardInventory = playerInverntory.SerializeCard();
            PlayerData.shared.markElement = markManager.GetMarkSelected();
            if (PlayerPrefs.GetString("IsOnline") == "True")
            {
                if (PlayerPrefs.GetInt("IsGuest") == 1)
                {
                    PlayerData.SaveData();
                    GetComponent<DashboardSceneManager>().LoadNewScene("Bazaar");
                }
                else
                {
                    touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
                    StartCoroutine(ApiManager.shared.SaveToApi(AccountBazaarSuccess, AccountBazaarFailure));
                }
                return;
            }
            PlayerData.SaveData();
            SceneManager.LoadScene("Bazaar");
            return;
        }
        errorMessage.SetupErrorMessage("You deck has to have between 30 and 60 cards");
    }
    private void AccountBazaarSuccess(AccountResponse accountResponse)
    {
        Destroy(touchBlocker.gameObject);
        SceneManager.LoadScene("Bazaar");
    }
    private void AccountBazaarFailure(AccountResponse accountResponse)
    {
        Destroy(touchBlocker.gameObject);
        SceneManager.LoadScene("Bazaar");
    }
}
