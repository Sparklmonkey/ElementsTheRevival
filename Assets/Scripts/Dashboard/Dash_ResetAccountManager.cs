using TMPro;
using UnityEngine;

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
            await ApiManager.Instance.SaveDataToUnity();
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            errorMessageManager.DisplayAnimatedError("Input text is not correct. Try Again.");
        }
    }
}
