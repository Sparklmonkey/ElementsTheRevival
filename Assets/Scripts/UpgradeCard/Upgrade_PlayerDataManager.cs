using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePlayerDataManager : MonoBehaviour
{

    private UpgradeInvetoryViewManager _playerInventoryManager;

    [SerializeField]
    private List<Card> playerInventory;
    [SerializeField]
    private TextMeshProUGUI deckCount;
    public ErrorMessageManager confirmationPopUp;
    // Start is called before the first frame update
    void Start()
    {
        _playerInventoryManager = GetComponent<UpgradeInvetoryViewManager>();
        if (PlayerData.Shared == null)
        {
            _playerInventoryManager.SetupInitialCardView(playerInventory);
            PlayerData.Shared = new PlayerData();
            PlayerData.Shared.electrum = 965;
        }
        else
        {
            _playerInventoryManager.SetupInitialCardView(PlayerData.Shared.cardInventory.DeserializeCard());
        }

        deckCount.text = $"( {PlayerData.Shared.cardInventory.Count} )";
        GetComponent<UpgradeTransactionManager>().SetupTransactionManager();
    }

    private Card _cardToUpgrade;

    public void UpgradeCardInInventory(Card cardToUpgrade)
    {
        if (cardToUpgrade.iD.IsUpgraded())
        {
            Debug.Log("Already Upgraded");
            return;
        }

        if (PlayerData.Shared.electrum < 1500)
        {
            Debug.Log("Not Enough Gold");
            return;
        }

        this._cardToUpgrade = cardToUpgrade;
        confirmationPopUp.gameObject.SetActive(true);
        confirmationPopUp.SetupErrorMessage($"Are you sure you want to UPGRADE {cardToUpgrade.cardName}?");
    }

    public void ConfirmUpgrade()
    {
        int cardIndex = 0;

        for (int i = 0; i < PlayerData.Shared.cardInventory.Count; i++)
        {
            if (PlayerData.Shared.cardInventory[i] == _cardToUpgrade.iD)
            {
                cardIndex = i;
                break;
            }
        }
        PlayerData.Shared.cardInventory.RemoveAt(cardIndex);
        PlayerData.Shared.cardInventory.Add(_cardToUpgrade.iD.GetUppedRegular());
        _playerInventoryManager.SetupContentView(PlayerData.Shared.cardInventory.DeserializeCard());
        GetComponent<UpgradeTransactionManager>().ChangeCoinCount();
        PlayerData.SaveData();
        _cardToUpgrade = null;
        confirmationPopUp.gameObject.SetActive(false);
    }

    public void CancelUpgrade()
    {
        _cardToUpgrade = null;
        confirmationPopUp.gameObject.SetActive(false);
    }
}
