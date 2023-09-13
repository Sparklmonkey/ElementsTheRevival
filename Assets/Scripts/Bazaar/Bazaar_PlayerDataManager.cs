using TMPro;
using UnityEngine;

public class Bazaar_PlayerDataManager : MonoBehaviour
{
    private Bazaar_PlayerInventoryManager playerInventoryManager;
    private Bazaar_ShopInventoryManager shopInventoryManager;

    [SerializeField]
    private TextMeshProUGUI deckCount;
    [SerializeField]
    private ErrorMessageManager confirmationMessage;

    private Card cardToChange;
    private bool isAdd = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInventoryManager = GetComponent<Bazaar_PlayerInventoryManager>();
        shopInventoryManager = GetComponent<Bazaar_ShopInventoryManager>();
        playerInventoryManager.SetupPlayerInvetoryView(PlayerData.shared.cardInventory.DeserializeCard());

        shopInventoryManager.SetupInitialCardView();
        deckCount.text = $"( {PlayerData.shared.cardInventory.Count} )";
        GetComponent<Bazaar_TransactionManager>().SetupTransactionManager(PlayerData.shared.electrum);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CanBuyCard(int buyPrice)
    {
        return PlayerData.shared.electrum >= buyPrice;
    }

    public void ModifyPlayerCardInventory(Card card, bool isAdd)
    {
        cardToChange = card;
        this.isAdd = isAdd;

        confirmationMessage.gameObject.SetActive(true);
        confirmationMessage.SetupErrorMessage($"Are you sure you want to {(isAdd ? "buy" : "sell")} {card.cardName}?");
    }

    public void ConfirmModification()
    {
        confirmationMessage.gameObject.SetActive(false);
        if (isAdd)
        {
            GetComponent<Bazaar_TransactionManager>().ChangeCoinCount(cardToChange.BuyPrice, true);
            PlayerData.shared.cardInventory.Add(cardToChange.iD);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < PlayerData.shared.cardInventory.Count; i++)
            {
                if (PlayerData.shared.cardInventory[i] == cardToChange.iD)
                {
                    index = i;
                    break;
                }
            }
            GetComponent<Bazaar_TransactionManager>().ChangeCoinCount(cardToChange.SellPrice, false);
            PlayerData.shared.cardInventory.RemoveAt(index);
        }
        PlayerData.SaveData();
        deckCount.text = $"( {PlayerData.shared.cardInventory.Count} )";
        playerInventoryManager.SetupPlayerInvetoryView(PlayerData.shared.cardInventory.DeserializeCard());
    }

    public void CancelModification()
    {
        confirmationMessage.gameObject.SetActive(false);
        cardToChange = null;
        isAdd = false;
    }
}
