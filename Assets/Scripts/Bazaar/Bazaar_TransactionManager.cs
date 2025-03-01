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
            PlayerData.Shared.HasBoughtCardBazaar = true;
            PlayerData.Shared.Electrum -= amount;
            coinCount.text = $"{PlayerData.Shared.Electrum}";
            return;
        }

        PlayerData.Shared.HasSoldCardBazaar = true;
        PlayerData.Shared.Electrum += amount;
        coinCount.text = $"{PlayerData.Shared.Electrum}";
    }
}

