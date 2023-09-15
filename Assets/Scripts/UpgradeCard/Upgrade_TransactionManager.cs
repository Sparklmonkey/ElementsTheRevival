using TMPro;
using UnityEngine;

public class UpgradeTransactionManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI coinCount;

    public void SetupTransactionManager()
    {
        coinCount.text = $"{PlayerData.Shared.electrum}";
    }

    public void ChangeCoinCount()
    {
        PlayerData.Shared.electrum -= 1500;
        coinCount.text = $"{PlayerData.Shared.electrum}";
    }
}
