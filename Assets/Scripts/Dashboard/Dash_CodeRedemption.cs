using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dash_CodeRedemption : MonoBehaviour
{
    private List<string> validCodes = new List<string> { "ElementsDiscordOEtG", "Legacy_ETG" };
    [SerializeField]
    private TextMeshProUGUI errorMessage;
    [SerializeField]
    private GameObject redeemObject;
    public void RedeemCode(TMP_InputField input)
    {
        string code = input.text;
        if (!validCodes.Contains(code))
        {
            errorMessage.text = "Invalid Code";
            return;
        }

        if (PlayerData.shared.redeemedCodes.Contains(code))
        {
            errorMessage.text = "Code Already Redeemed";
            return;
        }

        if (PlayerData.shared.redeemedCodes == null)
        {
            PlayerData.shared.redeemedCodes = new List<string>();
        }

        PlayerData.shared.redeemedCodes.Add(code);

        switch (code)
        {
            case "ElementsDiscordOEtG":
                PlayerData.shared.electrum += 750;
                break;
            case "Legacy_ETG":
                PlayerData.shared.electrum += 750;
                break;
            default:
                break;
        }
        GetComponent<DashboardPlayerData>().UpdateDashboard();
        redeemObject.SetActive(false);
    }
}
