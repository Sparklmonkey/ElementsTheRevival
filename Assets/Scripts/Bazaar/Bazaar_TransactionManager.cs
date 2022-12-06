using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bazaar_TransactionManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinCount;



    public void SetupTransactionManager(int coinCount)
    {
        this.coinCount.text = $"{coinCount}";
    }

    public void ChangeCoinCount(int amount, bool isAdd)
    {
        if (isAdd)
        {
            PlayerData.shared.hasBoughtCardBazaar = true;
            PlayerData.shared.electrum -= amount;
            coinCount.text = $"{PlayerData.shared.electrum}";
            return;
        }

        PlayerData.shared.hasSoldCardBazaar = true;
        PlayerData.shared.electrum += amount;
        coinCount.text = $"{PlayerData.shared.electrum}";
    }
}

