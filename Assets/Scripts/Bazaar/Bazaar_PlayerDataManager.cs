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
    void Start()
    {
        _playerInventoryManager = GetComponent<BazaarPlayerInventoryManager>();
        _shopInventoryManager = GetComponent<BazaarShopInventoryManager>();
        _playerInventoryManager.SetupPlayerInvetoryView(PlayerData.Shared.cardInventory.DeserializeCard());

        _shopInventoryManager.SetupInitialCardView();
        deckCount.text = $"( {PlayerData.Shared.cardInventory.Count} )";
        GetComponent<BazaarTransactionManager>().SetupTransactionManager(PlayerData.Shared.electrum);
    }

    // Update is called once per frame
    void Update()
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
        confirmationMessage.SetupErrorMessage($"Are you sure you want to {(isAdd ? "buy" : "sell")} {card.cardName}?");
    }

    public void ConfirmModification()
    {
        confirmationMessage.gameObject.SetActive(false);
        if (_isAdd)
        {
            GetComponent<BazaarTransactionManager>().ChangeCoinCount(_cardToChange.BuyPrice, true);
            PlayerData.Shared.cardInventory.Add(_cardToChange.iD);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < PlayerData.Shared.cardInventory.Count; i++)
            {
                if (PlayerData.Shared.cardInventory[i] == _cardToChange.iD)
                {
                    index = i;
                    break;
                }
            }
            GetComponent<BazaarTransactionManager>().ChangeCoinCount(_cardToChange.SellPrice, false);
            PlayerData.Shared.cardInventory.RemoveAt(index);
        }
        PlayerData.SaveData();
        deckCount.text = $"( {PlayerData.Shared.cardInventory.Count} )";
        _playerInventoryManager.SetupPlayerInvetoryView(PlayerData.Shared.cardInventory.DeserializeCard());
    }

    public void CancelModification()
    {
        confirmationMessage.gameObject.SetActive(false);
        _cardToChange = null;
        _isAdd = false;
    }
}
