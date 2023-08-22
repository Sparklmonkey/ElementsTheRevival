using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrade_TransactionManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI coinCount;

    public void SetupTransactionManager()
    {
        coinCount.text = $"{PlayerData.shared.electrum}";
    }

    public void ChangeCoinCount()
    {
        PlayerData.shared.electrum -= 1500;
        coinCount.text = $"{PlayerData.shared.electrum}";
    }
}
