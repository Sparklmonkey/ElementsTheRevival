using System;
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
    private void Start()
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
                var index = playerInventory.FindIndex(x => x.Id == item.Id);
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
        playerDeck.Sort((x, y) => string.CompareOrdinal(x.Id, y.Id));
        playerInventory.Sort((x, y) => string.CompareOrdinal(x.Id, y.Id));

        deckCount.text = $"( {playerDeck.Count} Cards ) ";
        inventoryCount.text = $"( {playerInventory.Count} Cards ) ";
        foreach (var deckCard in playerDeck)
        {
            if (deckCard.Id is "999")
            {
                PlayerData.Shared.currentDeck.Remove(deckCard.Id);
                continue;
            }
            var cardHeadObject = Instantiate(cardHeadPrefab, deckContentView);
            cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(deckCard, this);
        }
        foreach (var inventoryCard in playerInventory)
        {
            if (inventoryCard.Id is "999")
            {
                PlayerData.Shared.inventoryCards.Remove(inventoryCard.Id);
                continue;
            }
            var dMCardPrefab = _inventoryDmCard.Find(x => x.GetCard().Id == inventoryCard.Id);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                var cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
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

            var deckPresetObject = Instantiate(deckPresetPrefab, deckPresetContentView);
            var markId = deck[1][^3..];
            deckPresetObject.GetComponent<DeckPresetHead>().SetupCardHead(deck[0], CardDatabase.Instance.GetCardFromId(markId).CostElement.FastElementString(), deck[1], this);
        }
    }

    public void FilterInventoryCardsByElement(int element)
    {
        _currentFilterSelection = element;
        _inventoryDmCard = new List<DmCardPrefab>();
        ClearInventoryView();
        playerInventory.Sort((x, y) => string.Compare(x.Id, y.Id));
        if (element == 14)
        {
            foreach (var inventoryCard in playerInventory)
            {
                var dMCardPrefab = _inventoryDmCard.Find(x => x.GetCard().Id == inventoryCard.Id);
                if (dMCardPrefab != null)
                {
                    dMCardPrefab.AddCard();
                }
                else
                {
                    var cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                    cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(inventoryCard, this);
                    _inventoryDmCard.Add(cardHeadObject.GetComponent<DmCardPrefab>());
                }
            }
            return;
        }

        var filter = (Element)element;

        var filteredList = playerInventory.FindAll(x => x.CardElement == filter);

        foreach (var inventoryCard in filteredList)
        {

            var dMCardPrefab = _inventoryDmCard.Find(x => x.GetCard().Id == inventoryCard.Id);
            if (dMCardPrefab != null)
            {
                dMCardPrefab.AddCard();
            }
            else
            {
                var cardHeadObject = Instantiate(cardHeadPrefab, inventoryContentView);
                cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(inventoryCard, this);
                _inventoryDmCard.Add(cardHeadObject.GetComponent<DmCardPrefab>());
            }

        }
    }

    private void ClearInventoryView()
    {
        var children = new List<DmCardPrefab>(inventoryContentView.GetComponentsInChildren<DmCardPrefab>());
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }
    }
    private void ClearDeckView()
    {
        var children = new List<DmCardPrefab>(deckContentView.GetComponentsInChildren<DmCardPrefab>());
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveAllDeckCards()
    {
        List<DmCardPrefab> children = new(deckContentView.GetComponentsInChildren<DmCardPrefab>());
        foreach (var child in children)
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
        playerDeck.Sort((x, y) => string.Compare(x.Id, y.Id));
        foreach (var deckCard in playerDeck)
        {
            var cardHeadObject = Instantiate(cardHeadPrefab, deckContentView);
            cardHeadObject.GetComponent<DmCardPrefab>().SetupCardHead(deckCard, this);
        }
        FilterInventoryCardsByElement(_currentFilterSelection);
    }

    public void ChangeParentContentView(Transform transform)
    {
        var card = transform.GetComponent<DmCardPrefab>().GetCard();
        if (transform.parent.name == "DeckContentView")
        {
            PlayerData.Shared.removedCardFromDeck = true;
            playerDeck.Remove(card);
            playerInventory.Add(card);
            UpdateCardView();
            return;
        }

        if (playerDeck.FindAll(x => x.Id == card.Id || x.Id == card.Id.GetUppedRegular()).Count == 6 && !card.Type.Equals(CardType.Pillar))
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
        var returnString = "";
        foreach (var card in playerDeck)
        {
            returnString += $"{card.Id} ";
        }
        GetComponent<DeckCodeManager>().SetupFields(returnString, markManager.GetMarkSelected());
    }

    public void OpenDeckPreset(string deckCode)
    {
        RemoveAllDeckCards();
        var idList = deckCode.DecompressDeckCode();
        foreach (var id in idList)
        {
            var cardIndex = playerInventory.FindIndex(x => x.Id == id);
            if (cardIndex == -1) { continue; }
            playerDeck.Add(playerInventory[cardIndex]);
            playerInventory.RemoveAt(cardIndex);
            if (CardDatabase.Instance.MarkIds.Contains(id))
            {
                markManager.SetupMarkCard((int)CardDatabase.Instance.GetCardFromId(id).CostElement);
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
            var cardIndex = playerInventory.FindIndex(x => x.Id == id);
            if (cardIndex == -1) { continue; }
            playerDeck.Add(playerInventory[cardIndex]);
            playerInventory.RemoveAt(cardIndex);
            if (CardDatabase.Instance.MarkIds.Contains(id))
            {
                markManager.SetupMarkCard((int)CardDatabase.Instance.GetCardFromId(id).CostElement);
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
                SceneTransitionManager.Instance.LoadScene("Dashboard");
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
