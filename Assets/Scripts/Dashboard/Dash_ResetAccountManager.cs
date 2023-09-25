using TMPro;
using UnityEngine;

public class DashResetAccountManager : MonoBehaviour
{
    [SerializeField]
    private ErrorAnimated errorMessageManager;

    public async void ConfirmResetText(TMP_InputField inputField)
    {
        if (ApiManager.IsTrainer)
        {
            return;
        }
        if (inputField.text == "RESET")
        {
            PlayerData.Shared.ResetAccount();
            await ApiManager.Instance.SaveGameData();
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
        }
        else
        {
            errorMessageManager.DisplayAnimatedError("Input text is not correct. Try Again.");
        }
    }
}
