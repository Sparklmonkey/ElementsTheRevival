using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrade_PlayerDataManager : MonoBehaviour
{

    private Upgrade_InvetoryViewManager playerInventoryManager;

    [SerializeField]
    private List<Card> playerInventory;
    [SerializeField]
    private TextMeshProUGUI deckCount;
    public ErrorMessageManager confirmationPopUp;
    // Start is called before the first frame update
    void Start()
    {
        playerInventoryManager = GetComponent<Upgrade_InvetoryViewManager>();
        if (PlayerData.shared == null)
        {
            playerInventoryManager.SetupInitialCardView(playerInventory);
            PlayerData.shared = new PlayerData();
            PlayerData.shared.electrum = 965;
        }
        else
        {
            playerInventoryManager.SetupInitialCardView(PlayerData.shared.cardInventory.DeserializeCard());
        }

        deckCount.text = $"( {PlayerData.shared.cardInventory.Count} )";
        GetComponent<Upgrade_TransactionManager>().SetupTransactionManager();
    }

    private Card cardToUpgrade;

    public void UpgradeCardInInventory(Card cardToUpgrade)
    {
        if (cardToUpgrade.iD.IsUpgraded())
        {
            Debug.Log("Already Upgraded");
            return;
        }

        if (PlayerData.shared.electrum < 1500)
        {
            Debug.Log("Not Enough Gold");
            return;
        }

        this.cardToUpgrade = cardToUpgrade;
        confirmationPopUp.gameObject.SetActive(true);
        confirmationPopUp.SetupErrorMessage($"Are you sure you want to UPGRADE {cardToUpgrade.cardName}?");
    }

    public void ConfirmUpgrade()
    {
        int cardIndex = 0;

        for (int i = 0; i < PlayerData.shared.cardInventory.Count; i++)
        {
            if (PlayerData.shared.cardInventory[i] == cardToUpgrade.iD)
            {
                cardIndex = i;
                break;
            }
        }
        PlayerData.shared.cardInventory.RemoveAt(cardIndex);
        PlayerData.shared.cardInventory.Add(cardToUpgrade.iD.GetUppedRegular());
        playerInventoryManager.SetupContentView(PlayerData.shared.cardInventory.DeserializeCard());
        GetComponent<Upgrade_TransactionManager>().ChangeCoinCount();
        PlayerData.SaveData();
        cardToUpgrade = null;
        confirmationPopUp.gameObject.SetActive(false);
    }

    public void CancelUpgrade()
    {
        cardToUpgrade = null;
        confirmationPopUp.gameObject.SetActive(false);
    }
}
