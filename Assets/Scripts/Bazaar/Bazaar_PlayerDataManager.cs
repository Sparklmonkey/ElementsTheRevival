using Core;
using TMPro;
using UnityEngine;

public class BazaarPlayerDataManager : MonoBehaviour
{
    private BazaarPlayerInventoryManager _playerInventoryManager;
    private BazaarShopInventoryManager _shopInventoryManager;

    [SerializeField]
    private TextMeshProUGUI deckCount;
    [SerializeField]
    private Bazaar_ConfirmationMessage confirmationMessage;

    private Card _cardToChange;
    private bool _isAdd = false;

    // Start is called before the first frame update
    private void Start()
    {
        _playerInventoryManager = GetComponent<BazaarPlayerInventoryManager>();
        _shopInventoryManager = GetComponent<BazaarShopInventoryManager>();
        _playerInventoryManager.SetupPlayerInvetoryView(PlayerData.Shared.GetInventory().DeserializeCard());

        _shopInventoryManager.SetupInitialCardView();
        deckCount.text = $"( {PlayerData.Shared.GetInventory().Count} )";
        GetComponent<BazaarTransactionManager>().SetupTransactionManager(PlayerData.Shared.Electrum);
    }

    public bool CanBuyCard(int buyPrice)
    {
        return PlayerData.Shared.Electrum >= buyPrice;
    }

    public void ModifyPlayerCardInventory(Card card, bool isAdd)
    {
        _cardToChange = card;
        _isAdd = isAdd;
        if(SessionManager.Instance.ShouldHideConfirm) 
        {
            ConfirmModification();
            return;
        }
        confirmationMessage.gameObject.SetActive(true);
        confirmationMessage.SetupErrorMessage($"Are you sure you want to {(isAdd ? "buy" : "sell")} {card.CardName}?");
    }

    public void ConfirmModification()
    {
        SessionManager.Instance.ShouldHideConfirm = confirmationMessage.hideConfirmation.isOn;
        confirmationMessage.gameObject.SetActive(false);
        if (_isAdd)
        {
            GetComponent<BazaarTransactionManager>().ChangeCoinCount(_cardToChange.BuyPrice, true);
            var invent = PlayerData.Shared.GetInventory();
            invent.Add(_cardToChange.Id);
            PlayerData.Shared.SetInventory(invent);
        }
        else
        {
            var invent = PlayerData.Shared.GetInventory();
            var index = 0;
            for (var i = 0; i < invent.Count; i++)
            {
                if (invent[i] == _cardToChange.Id)
                {
                    index = i;
                    break;
                }
            }
            GetComponent<BazaarTransactionManager>().ChangeCoinCount(_cardToChange.SellPrice, false);
            invent.RemoveAt(index);
            PlayerData.Shared.SetInventory(invent);
        }
        PlayerData.SaveData();
        deckCount.text = $"( {PlayerData.Shared.GetInventory().Count} )";
        _playerInventoryManager.SetupPlayerInvetoryView(PlayerData.Shared.GetInventory().DeserializeCard());
    }

    public void CancelModification()
    {
        confirmationMessage.gameObject.SetActive(false);
        _cardToChange = null;
        _isAdd = false;
    }
}
