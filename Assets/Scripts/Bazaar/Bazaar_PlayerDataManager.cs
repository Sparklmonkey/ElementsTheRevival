using TMPro;
using UnityEngine;

public class BazaarPlayerDataManager : MonoBehaviour
{
    private BazaarPlayerInventoryManager _playerInventoryManager;
    private BazaarShopInventoryManager _shopInventoryManager;

    [SerializeField]
    private TextMeshProUGUI deckCount;
    [SerializeField]
    private ErrorMessageManager confirmationMessage;

    private Card _cardToChange;
    private bool _isAdd = false;

    // Start is called before the first frame update
    private void Start()
    {
        _playerInventoryManager = GetComponent<BazaarPlayerInventoryManager>();
        _shopInventoryManager = GetComponent<BazaarShopInventoryManager>();
        _playerInventoryManager.SetupPlayerInvetoryView(PlayerData.Shared.inventoryCards.DeserializeCard());

        _shopInventoryManager.SetupInitialCardView();
        deckCount.text = $"( {PlayerData.Shared.inventoryCards.Count} )";
        GetComponent<BazaarTransactionManager>().SetupTransactionManager(PlayerData.Shared.electrum);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public bool CanBuyCard(int buyPrice)
    {
        return PlayerData.Shared.electrum >= buyPrice;
    }

    public void ModifyPlayerCardInventory(Card card, bool isAdd)
    {
        _cardToChange = card;
        this._isAdd = isAdd;

        confirmationMessage.gameObject.SetActive(true);
        confirmationMessage.SetupErrorMessage($"Are you sure you want to {(isAdd ? "buy" : "sell")} {card.CardName}?");
    }

    public void ConfirmModification()
    {
        confirmationMessage.gameObject.SetActive(false);
        if (_isAdd)
        {
            GetComponent<BazaarTransactionManager>().ChangeCoinCount(_cardToChange.BuyPrice, true);
            PlayerData.Shared.inventoryCards.Add(_cardToChange.Id);
        }
        else
        {
            var index = 0;
            for (var i = 0; i < PlayerData.Shared.inventoryCards.Count; i++)
            {
                if (PlayerData.Shared.inventoryCards[i] == _cardToChange.Id)
                {
                    index = i;
                    break;
                }
            }
            GetComponent<BazaarTransactionManager>().ChangeCoinCount(_cardToChange.SellPrice, false);
            PlayerData.Shared.inventoryCards.RemoveAt(index);
        }
        PlayerData.SaveData();
        deckCount.text = $"( {PlayerData.Shared.inventoryCards.Count} )";
        _playerInventoryManager.SetupPlayerInvetoryView(PlayerData.Shared.inventoryCards.DeserializeCard());
    }

    public void CancelModification()
    {
        confirmationMessage.gameObject.SetActive(false);
        _cardToChange = null;
        _isAdd = false;
    }
}
