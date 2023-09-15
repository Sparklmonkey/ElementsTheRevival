using TMPro;
using UnityEngine;

public class BazaarTransactionManager : MonoBehaviour
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
            PlayerData.Shared.hasBoughtCardBazaar = true;
            PlayerData.Shared.electrum -= amount;
            coinCount.text = $"{PlayerData.Shared.electrum}";
            return;
        }

        PlayerData.Shared.hasSoldCardBazaar = true;
        PlayerData.Shared.electrum += amount;
        coinCount.text = $"{PlayerData.Shared.electrum}";
    }
}

