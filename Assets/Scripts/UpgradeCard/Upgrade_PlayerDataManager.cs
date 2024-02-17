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
    private void Start()
    {
        _playerInventoryManager = GetComponent<UpgradeInvetoryViewManager>();
        _playerInventoryManager.SetupInitialCardView(PlayerData.Shared.inventoryCards.DeserializeCard());

        deckCount.text = $"( {PlayerData.Shared.inventoryCards.Count} )";
        GetComponent<UpgradeTransactionManager>().SetupTransactionManager();
    }

    private Card _cardToUpgrade;

    public void UpgradeCardInInventory(Card cardToUpgrade)
    {
        if (cardToUpgrade.Id.IsUpgraded())
        {
            Debug.Log("Already Upgraded");
            return;
        }

        if (PlayerData.Shared.electrum < 1500)
        {
            Debug.Log("Not Enough Gold");
            return;
        }

        _cardToUpgrade = cardToUpgrade;
        confirmationPopUp.gameObject.SetActive(true);
        confirmationPopUp.SetupErrorMessage($"Are you sure you want to UPGRADE {cardToUpgrade.CardName}?");
    }

    public void ConfirmUpgrade()
    {
        var cardIndex = 0;

        for (var i = 0; i < PlayerData.Shared.inventoryCards.Count; i++)
        {
            if (PlayerData.Shared.inventoryCards[i] != _cardToUpgrade.Id) continue;
            cardIndex = i;
            break;
        }
        PlayerData.Shared.inventoryCards.RemoveAt(cardIndex);
        PlayerData.Shared.inventoryCards.Add(_cardToUpgrade.Id.GetUppedRegular());
        _playerInventoryManager.SetupContentView(PlayerData.Shared.inventoryCards.DeserializeCard());
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
