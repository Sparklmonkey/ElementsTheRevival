using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bazaar_PlayerDataManager : MonoBehaviour
{
    private Bazaar_PlayerInventoryManager playerInventoryManager;
    private Bazaar_ShopInventoryManager shopInventoryManager;

    [SerializeField]
    private List<Card> playerInvetoryTest, shopInventoryList;
    [SerializeField]
    private TextMeshProUGUI deckCount;
    // Start is called before the first frame update
    void Start()
    {
        playerInventoryManager = GetComponent<Bazaar_PlayerInventoryManager>();
        shopInventoryManager = GetComponent<Bazaar_ShopInventoryManager>();

            playerInventoryManager.SetupPlayerInvetoryView(PlayerData.shared.cardInventory.DeserializeCard());
        

        shopInventoryManager.SetupInitialCardView(shopInventoryList);
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
        CardObject cardObject = new CardObject(card.name, card.type.FastCardTypeString(), !card.isUpgradable);
        if (isAdd)
        {
            PlayerData.shared.cardInventory.Add(cardObject);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < PlayerData.shared.cardInventory.Count; i++)
            {
                if (PlayerData.shared.cardInventory[i].cardName == card.name)
                {
                    index = i;
                    break;
                }
            }
            PlayerData.shared.cardInventory.RemoveAt(index);
        }
        PlayerData.SaveData();
        deckCount.text = $"( {PlayerData.shared.cardInventory.Count} )";
    }
}
