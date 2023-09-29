using System.Collections.Generic;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckDisplayManager : MonoBehaviour
{
    [SerializeField]
    private List<Card> playerDeck, playerInventory, arenaInventory;
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
    private DmMarkManager markManager;
    public static bool IsArena;
    private GameObject _touchBlocker;
    private List<DmCardPrefab> _inventoryDmCard;

    private int _currentFilterSelection = 14;
    // Start is called before the first frame update
    void Start()
    {
        _inventoryDmCard = new List<DmCardPrefab>();
        //Sort By ID

        if (IsArena)
        {
            menuBtnText.text = "Arena T50";
            playerDeck = PlayerData.Shared.arenaT50Deck.DeserializeCard();
            playerInventory = arenaInventory = PlayerData.Shared.inventoryCards.DeserializeCard();
            playerInventory.AddRange(PlayerData.Shared.currentDeck.DeserializeCard());
            foreach (var item in playerDeck)
            {
                var index = playerInventory.FindIndex(x => x.iD == item.iD);
                playerInventory.Remove(item);
            }
            markManager.SetupMarkCard((int)PlayerData.Shared.arenaT50Mark);
        }
        else
        {
            menuBtnText.text = "Main Menu";
            playerDeck = PlayerData.Shared.currentDeck.DeserializeCard();
            playerInventory = PlayerData.Shared.inventoryCards.DeserializeCard();
            markManager.SetupMarkCard((int)PlayerData.Shared.markElement);
        }
        playerDeck.Sort((x, y) => string.Compare(x.iD, y.iD));
        playerInventory.Sort((x, y) => string.Compare(x.iD, y.iD));

        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInventory.Count} Cards ) ";
        foreach (Card deckCard in playerDeck)
        {
            GameObject cardHeadObject = Instantiate(cardHeadPrefab, deckContentView);
            cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(deckCard, this);
        }
        foreach (Card inventoryCard in playerInventory)
        {
            DmCardPrefab dMCardPrefab = _inventoryDmCard.Find(x => x.GetCard().cardName == inventoryCard.cardName);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(inventoryCard, this);
                _inventoryDmCard.Add(cardHeadObject.GetComponent<DmCardPrefab>());
            }
        }
    }

    public void SetupDeckPresetView(string deckcode)
    {
        deckPresetContentView.transform.parent.parent.gameObject.SetActive(false);
        List<DeckPresetHead> children = new(deckPresetContentView.GetComponentsInChildren<DeckPresetHead>());
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }

        foreach (var deckPreset in PlayerData.Shared.savedDecks)
        {
            var deck = deckPreset.Split(":");

            GameObject deckPresetObject = Instantiate(deckPresetPrefab, deckPresetContentView);
            string markId = deck[1][^3..];
            deckPresetObject.GetComponent<DeckPresetHead>().SetupCardHead(deck[0], CardDatabase.Instance.GetCardFromId(markId).costElement.FastElementString(), deck[1], this);
        }
    }

    public void FilterInventoryCardsByElement(int element)
    {
        _currentFilterSelection = element;
        _inventoryDmCard = new List<DmCardPrefab>();
        ClearInventoryView();
        playerInventory.Sort((x, y) => string.Compare(x.iD, y.iD));
        if (element == 14)
        {
            foreach (Card inventoryCard in playerInventory)
            {
                DmCardPrefab dMCardPrefab = _inventoryDmCard.Find(x => x.GetCard().cardName == inventoryCard.cardName);
                if (dMCardPrefab != null)
                {
                    dMCardPrefab.AddCard();
                }
                else
                {
                    GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                    cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(inventoryCard, this);
                    _inventoryDmCard.Add(cardHeadObject.GetComponent<DmCardPrefab>());
                }
            }
            return;
        }

        Element filter = (Element)element;

        List<Card> filteredList = playerInventory.FindAll(x => x.costElement == filter);

        if (filter.Equals(Element.Air))
        {
            filteredList.AddRange(playerInventory.FindAll(x => x.cardName == "Animate Weapon"));
        }

        if (filter.Equals(Element.Light))
        {
            filteredList.AddRange(playerInventory.FindAll(x => x.cardName == "Luciferin" || x.cardName == "Luciferase"));
        }

        foreach (Card inventoryCard in filteredList)
        {

            DmCardPrefab dMCardPrefab = _inventoryDmCard.Find(x => x.GetCard().cardName == inventoryCard.cardName);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(inventoryCard, this);
                _inventoryDmCard.Add(cardHeadObject.GetComponent<DmCardPrefab>());
            }

        }
    }

    private void ClearInventoryView()
    {
        List<DmCardPrefab> children = new List<DmCardPrefab>(inventoryContentView.GetComponentsInChildren<DmCardPrefab>());
        foreach (DmCardPrefab child in children)
        {
            Destroy(child.gameObject);
        }
    }
    private void ClearDeckView()
    {
        List<DmCardPrefab> children = new List<DmCardPrefab>(deckContentView.GetComponentsInChildren<DmCardPrefab>());
        foreach (DmCardPrefab child in children)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveAllDeckCards()
    {
        List<DmCardPrefab> children = new(deckContentView.GetComponentsInChildren<DmCardPrefab>());
        foreach (DmCardPrefab child in children)
        {
            playerInventory.Add(child.GetCard());
            ClearDeckView();
        }
        FilterInventoryCardsByElement(14);
        playerDeck.Clear();
        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInventory.Count} Cards ) ";
    }

    private void UpdateCardView()
    {
        _inventoryDmCard = new List<DmCardPrefab>();
        ClearDeckView();
        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInventory.Count} Cards ) ";
        playerDeck.Sort((x, y) => string.Compare(x.iD, y.iD));
        foreach (Card deckCard in playerDeck)
        {
            GameObject cardHeadObject = Instantiate(cardHeadPrefab, deckContentView);
            cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(deckCard, this);
        }
        FilterInventoryCardsByElement(_currentFilterSelection);
    }

    public void ChangeParentContentView(Transform transform)
    {
        Card card = transform.GetComponent<DmCardPrefab>().GetCard();
        if (transform.parent.name == "DeckContentView")
        {
            PlayerData.Shared.removedCardFromDeck = true;
            playerDeck.Remove(card);
            playerInventory.Add(card);
            UpdateCardView();
            return;
        }

        if (playerDeck.FindAll(x => x.iD == card.iD || x.iD == card.iD.GetUppedRegular()).Count == 6 && !card.cardType.Equals(CardType.Pillar))
        {
            errorMessage.SetupErrorMessage("No more than 6 copies of each card(except pillars)");
            return;
        }

        playerInventory.Remove(card);
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

    public void GetDeckCode()
    {
        deckCodePopUpObject.SetActive(true);
        string returnString = "";
        foreach (Card card in playerDeck)
        {
            returnString += $"{card.iD} ";
        }
        GetComponent<DeckCodeManager>().SetupFields(returnString, markManager.GetMarkSelected());
    }

    public void OpenDeckPreset(string deckCode)
    {
        RemoveAllDeckCards();
        List<string> idList = deckCode.DecompressDeckCode();
        foreach (var id in idList)
        {
            int cardIndex = playerInventory.FindIndex(x => x.iD == id);
            if (cardIndex == -1) { continue; }
            playerDeck.Add(playerInventory[cardIndex]);
            playerInventory.RemoveAt(cardIndex);
            if (CardDatabase.Instance.MarkIds.Contains(id))
            {
                markManager.SetupMarkCard((int)CardDatabase.Instance.GetCardFromId(id).costElement);
            }
        }
        UpdateCardView();
        deckCodePopUpObject.SetActive(false);
    }

    public void ImportLegacyDeckCode()
    {
        RemoveAllDeckCards();
        List<string> idList = new(deckCodeField.text.Split(" "));
        foreach (var id in idList)
        {
            int cardIndex = playerInventory.FindIndex(x => x.iD == id);
            if (cardIndex == -1) { continue; }
            playerDeck.Add(playerInventory[cardIndex]);
            playerInventory.RemoveAt(cardIndex);
            if (CardDatabase.Instance.MarkIds.Contains(id))
            {
                markManager.SetupMarkCard((int)CardDatabase.Instance.GetCardFromId(id).costElement);
            }
        }
        UpdateCardView();
        deckCodePopUpObject.SetActive(false);
    }

    public async void SaveDeck()
    {
        if (playerDeck.Count is >= 30 and <= 60)
        {
            if (IsArena)
            {
                PlayerData.Shared.arenaT50Deck = playerDeck.SerializeCard();
                PlayerData.Shared.arenaT50Mark = markManager.GetMarkSelected();
                PlayerData.Shared.inventoryCards = arenaInventory.SerializeCard();
            }
            else
            {
                PlayerData.Shared.currentDeck = playerDeck.SerializeCard();
                PlayerData.Shared.markElement = markManager.GetMarkSelected();
                PlayerData.Shared.inventoryCards = playerInventory.SerializeCard();
            }

            if (ApiManager.IsTrainer)
            {
                GetComponent<DashboardSceneManager>().LoadNewScene("Dashboard");
                return;
            }
            menuBtn.gameObject.SetActive(false);
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            await ApiManager.Instance.SaveGameData();

            _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            Destroy(_touchBlocker);
            menuBtn.gameObject.SetActive(true);
            if (IsArena)
            {
                IsArena = false;
                SceneTransitionManager.Instance.LoadScene("Top50");
            }
            else
            {
                IsArena = false;
                SceneTransitionManager.Instance.LoadScene("Dashboard");
            }
            return;
        }
        errorMessage.SetupErrorMessage("You deck has to have between 30 and 60 cards");
    }

    public async void GoToBazaar()
    {
        if (playerDeck.Count is >= 30 and <= 60)
        {
            if (IsArena)
            {
                PlayerData.Shared.arenaT50Deck = playerDeck.SerializeCard();
                PlayerData.Shared.arenaT50Mark = markManager.GetMarkSelected();
                PlayerData.Shared.inventoryCards = arenaInventory.SerializeCard();
            }
            else
            {
                PlayerData.Shared.currentDeck = playerDeck.SerializeCard();
                PlayerData.Shared.markElement = markManager.GetMarkSelected();
                PlayerData.Shared.inventoryCards = playerInventory.SerializeCard();
            }

            if (ApiManager.IsTrainer)
            {
                SceneTransitionManager.Instance.LoadScene("Bazaar");
                return;
            }
            menuBtn.gameObject.SetActive(false);
            _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            await ApiManager.Instance.SaveGameData();

            _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            Destroy(_touchBlocker);
            menuBtn.gameObject.SetActive(true);
            SceneTransitionManager.Instance.LoadScene("Bazaar");
            return;
        }
        errorMessage.SetupErrorMessage("You deck has to have between 30 and 60 cards");
    }
}
