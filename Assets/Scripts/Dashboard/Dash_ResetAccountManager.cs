using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dash_ResetAccountManager : MonoBehaviour
{
    [SerializeField]
    private Error_Animated errorMessageManager;

    public async void ConfirmResetText(TMP_InputField inputField)
    {
        if (ApiManager.isTrainer)
        {
            return;
        }
        if (inputField.text == "RESET")
        {
            PlayerData.shared.ResetAccount();
            await ApiManager.shared.SaveDataToUnity();
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            errorMessageManager.DisplayAnimatedError("Input text is not correct. Try Again.");
        }
    }
}
