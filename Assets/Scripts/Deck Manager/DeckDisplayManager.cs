using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckDisplayManager : MonoBehaviour
{
    [SerializeField]
    private List<Card> playerDeck, playerInverntory;

    [SerializeField]
    private TextMeshProUGUI deckCount, inventoryCount, menuBtnText;
    [SerializeField]
    private Button menuBtn, bazaarBtn;

    [SerializeField]
    private GameObject cardHeadPrefab, deckCodePopUpObject, deckPresetPrefab;

    [SerializeField]
    private TMP_InputField deckCodeField;
    [SerializeField]
    private Transform deckContentView, inventoryContentView, deckPresetContentView;
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField]
    private ErrorMessageManager errorMessage;
    [SerializeField]
    private DeckManagerViewPlacement dmViewPlacement;
    [SerializeField]
    private DM_MarkManager markManager;
    public static bool isArena;
    private GameObject touchBlocker;
    private List<DMCardPrefab> inventoryDMCard;

    private int currentFilterSelection = 14;
    // Start is called before the first frame update
    void Start()
    {
        inventoryDMCard = new List<DMCardPrefab>();
        //Sort By ID

        if (isArena)
        {
            menuBtnText.text = "Arena T50";
            playerDeck = PlayerData.shared.arenaT50Deck.DeserializeCard();
            playerInverntory = PlayerData.shared.cardInventory.DeserializeCard();
            playerInverntory.AddRange(PlayerData.shared.currentDeck.DeserializeCard());
            foreach (var item in playerDeck)
            {
                playerInverntory.Remove(item);
            }
            markManager.SetupMarkCard((int)PlayerData.shared.arenaT50Mark);
        }
        else
        {
            menuBtnText.text = "Main Menu";
            playerDeck = PlayerData.shared.currentDeck.DeserializeCard();
            playerInverntory = PlayerData.shared.cardInventory.DeserializeCard();
            markManager.SetupMarkCard((int)PlayerData.shared.markElement);
        }
        playerDeck.Sort((x, y) => string.Compare(x.iD, y.iD));
        playerInverntory.Sort((x, y) => string.Compare(x.iD, y.iD));

        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInverntory.Count} Cards ) ";
        foreach (Card deckCard in playerDeck)
        {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, deckContentView);
                cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(deckCard, this);
        }
        foreach (Card inventoryCard in playerInverntory)
        {
            DMCardPrefab dMCardPrefab = inventoryDMCard.Find(x => x.GetCard().cardName == inventoryCard.cardName);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(inventoryCard, this);
                inventoryDMCard.Add(cardHeadObject.GetComponent<DMCardPrefab>());
            }
        }
    }

    public void SetupDeckPresetView(string deckcode)
    {
        deckPresetContentView.transform.parent.parent.gameObject.SetActive(false);
        List<DeckPresetHead> children = new (deckPresetContentView.GetComponentsInChildren<DeckPresetHead>());
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }

        foreach (var deckPreset in PlayerData.shared.savedDecks)
        {
            var deck = deckPreset.Split(":");

            GameObject deckPresetObject = Instantiate(deckPresetPrefab, deckPresetContentView);
            string markId = deck[1][^3..];
            deckPresetObject.GetComponent<DeckPresetHead>().SetupCardHead(deck[0], CardDatabase.Instance.GetCardFromId(markId).costElement.FastElementString(), deck[1], this);
        }
    }

    public void FilterInventoryCardsByElement(int element)
    {
        currentFilterSelection = element;
        inventoryDMCard = new List<DMCardPrefab>();
        ClearInventoryView();
        playerInverntory.Sort((x, y) => string.Compare(x.iD, y.iD));
        if (element == 14)
        {
            foreach (Card inventoryCard in playerInverntory)
            {
                DMCardPrefab dMCardPrefab = inventoryDMCard.Find(x => x.GetCard().cardName == inventoryCard.cardName);
                if (dMCardPrefab != null)
                {
                    dMCardPrefab.AddCard();
                }
                else
                {
                    GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                    cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(inventoryCard, this);
                    inventoryDMCard.Add(cardHeadObject.GetComponent<DMCardPrefab>());
                }
            }
            return;
        }

        Element filter = (Element)element;

        List<Card> filteredList = playerInverntory.FindAll(x => x.costElement == filter);

        if (filter.Equals(Element.Air))
        {
            filteredList.AddRange(playerInverntory.FindAll(x => x.cardName == "Animate Weapon"));
        }

        if (filter.Equals(Element.Light))
        {
            filteredList.AddRange(playerInverntory.FindAll(x => x.cardName == "Luciferin" || x.cardName == "Luciferase"));
        }

        foreach (Card inventoryCard in filteredList)
        {

            DMCardPrefab dMCardPrefab = inventoryDMCard.Find(x => x.GetCard().cardName == inventoryCard.cardName);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(inventoryCard, this);
                inventoryDMCard.Add(cardHeadObject.GetComponent<DMCardPrefab>());
            }
            
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
        List<DMCardPrefab> children = new List<DMCardPrefab>(deckContentView.GetComponentsInChildren<DMCardPrefab>());
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

    private void UpdateCardView()
    {
        inventoryDMCard = new List<DMCardPrefab>();
        ClearDeckView();
        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInverntory.Count} Cards ) ";
        playerDeck.Sort((x, y) => string.Compare(x.iD, y.iD));
        foreach (Card deckCard in playerDeck)
        {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, deckContentView);
                cardHeadObject.GetComponent<DMCardPrefab>().SetupCardHead(deckCard, this);
        }
        FilterInventoryCardsByElement(currentFilterSelection);
    }

    public void ChangeParentContentView(Transform transform)
    {
        Card card = transform.GetComponent<DMCardPrefab>().GetCard();
        if (transform.parent.name == "DeckContentView")
        {
            PlayerData.shared.removedCardFromDeck = true;
            playerDeck.Remove(card);
            playerInverntory.Add(card);
            UpdateCardView();
            return;
        }

        if (playerDeck.FindAll(x => x.iD == card.iD || x.iD == card.iD.GetUppedRegular()).Count == 6 && !card.cardType.Equals(CardType.Pillar))
        {
            errorMessage.SetupErrorMessage("No more than 6 copies of each card(except pillars)");
            return;
        }

        playerInverntory.Remove(card);
        playerDeck.Add(card);
        UpdateCardView();
    }


    public void ShowCardDisplay(Card card)
    {
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.SetupCardView(card);
    }

    public void HideCardDisplay()
    {
        cardDisplay.gameObject.SetActive(false);
    }

    private void AccountSuccess(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        menuBtn.gameObject.SetActive(true);
        if (isArena)
        {
            isArena = false;
            SceneManager.LoadScene("Top50");
        }
        else
        {
            isArena = false;
            SceneManager.LoadScene("Dashboard");
        }
    }
    private void AccountFailure(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        menuBtn.gameObject.SetActive(true);
        isArena = false;
        if (isArena)
        {
            SceneManager.LoadScene("Top50");
        }
        else
        {
            SceneManager.LoadScene("Dashboard");
        }
    }

    public void GetDeckCode()
    {
        deckCodePopUpObject.SetActive(true);
        string returnString = "";
        foreach (Card card in playerDeck)
        {
            returnString += $"{card.iD} ";
        }
        returnString += $"{CardDatabase.Instance.markIds[(int)markManager.GetMarkSelected()]}";
        deckCodeField.text = returnString;
    }

    public void OpenDeckPreset(string deckCode)
    {
        RemoveAllDeckCards();
        List<string> idList = deckCode.DecompressDeckCode();
        foreach (var id in idList)
        {
            int cardIndex = playerInverntory.FindIndex(x => x.iD == id);
            if (cardIndex == -1) { continue; }
            playerDeck.Add(playerInverntory[cardIndex]);
            playerInverntory.RemoveAt(cardIndex);
            if (CardDatabase.Instance.markIds.Contains(id))
            {
                markManager.SetupMarkCard((int)CardDatabase.Instance.GetCardFromId(id).costElement);
            }
        }
        UpdateCardView();
        deckCodePopUpObject.SetActive(false);
    }

    public void ImportDeckCode()
    {
        RemoveAllDeckCards();
        List<string> idList = new List<string>(deckCodeField.text.Split(" "));
        foreach (var id in idList)
        {
            int cardIndex = playerInverntory.FindIndex(x => x.iD == id);
            if(cardIndex == -1) { continue; }
            playerDeck.Add(playerInverntory[cardIndex]);
            playerInverntory.RemoveAt(cardIndex);
            if (CardDatabase.Instance.markIds.Contains(id))
            {
                markManager.SetupMarkCard((int)CardDatabase.Instance.GetCardFromId(id).costElement);
            }
        }
        UpdateCardView();
        deckCodePopUpObject.SetActive(false);
    }

    public void SaveDeck()
    {
        if (playerDeck.Count >= 30 && playerDeck.Count <= 60)
        {
            if (isArena)
            {
                PlayerData.shared.arenaT50Deck = playerDeck.SerializeCard();
                PlayerData.shared.arenaT50Mark = markManager.GetMarkSelected();
            }
            else
            {
                PlayerData.shared.currentDeck = playerDeck.SerializeCard();
                PlayerData.shared.markElement = markManager.GetMarkSelected();
                PlayerData.shared.cardInventory = playerInverntory.SerializeCard();
            }

            if (ApiManager.isTrainer)
            {
                GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
                return;
            }
            if (PlayerPrefs.GetInt("IsGuest") == 1)
            {
                PlayerData.SaveData();
                GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
            }
            else
            {
                menuBtn.gameObject.SetActive(false);
                touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
                StartCoroutine(ApiManager.shared.SaveToApi(AccountSuccess, AccountFailure));
            }
            return;
        }
        errorMessage.SetupErrorMessage("You deck has to have between 30 and 60 cards");
    }

    public void GoToBazaar()
    {
        if (playerDeck.Count >= 30 && playerDeck.Count <= 60)
        {
            if (isArena)
            {
                PlayerData.shared.arenaT50Deck = playerDeck.SerializeCard();
                PlayerData.shared.arenaT50Mark = markManager.GetMarkSelected();
            }
            else
            {
                PlayerData.shared.currentDeck = playerDeck.SerializeCard();
                PlayerData.shared.markElement = markManager.GetMarkSelected();
                PlayerData.shared.cardInventory = playerInverntory.SerializeCard();
            }

            if (ApiManager.isTrainer)
            {
                GetComponent<DashboardSceneManager>().LoadNewScene("Bazaar");
                return;
            }
            if (PlayerPrefs.GetInt("IsGuest") == 1)
            {
                PlayerData.SaveData();
                GetComponent<DashboardSceneManager>().LoadNewScene("Bazaar");
            }
            else
            {
                bazaarBtn.gameObject.SetActive(false);
                touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
                isArena = false;
                StartCoroutine(ApiManager.shared.SaveToApi(AccountBazaarSuccess, AccountBazaarFailure));
            }
            return;
        }
        errorMessage.SetupErrorMessage("You deck has to have between 30 and 60 cards");
    }

    private void AccountBazaarSuccess(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        bazaarBtn.gameObject.SetActive(true);
        SceneManager.LoadScene("Bazaar");
    }
    private void AccountBazaarFailure(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        bazaarBtn.gameObject.SetActive(true);
        SceneManager.LoadScene("Bazaar");
    }
}