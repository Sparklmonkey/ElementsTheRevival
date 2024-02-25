using Networking;
using TMPro;
using UnityEngine;

public class DashResetAccountManager : MonoBehaviour
{
    [SerializeField]
    private ErrorAnimated errorMessageManager;

    public async void ConfirmResetText(TMP_InputField inputField)
    {
        if (ApiManager.IsTrainer) return;
        if (inputField.text == "DELETE")
        {
            var response = await ApiManager.Instance.ResetSaveData();
            PlayerData.Shared = response.savedData;
            SceneTransitionManager.Instance.LoadScene("DeckSelector");
        }
        else
        {
            errorMessageManager.DisplayAnimatedError("Input text is not correct. Try Again.");
        }
    }
}
