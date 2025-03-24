using TMPro;
using UnityEngine;

public class UpgradeTransactionManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI coinCount;

    public void SetupTransactionManager()
    {
        coinCount.text = $"{PlayerData.Shared.Electrum}";
    }

    public void ChangeCoinCount()
    {
        PlayerData.Shared.Electrum -= 1500;
        coinCount.text = $"{PlayerData.Shared.Electrum}";
    }
}
