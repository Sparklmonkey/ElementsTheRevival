using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DashAccountManagement : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField currentUsernameField, newUsernameField, currentPasswordField, newPasswordField, emailField;
    [SerializeField]
    private Button submitButton;
    [SerializeField]
    private TextMeshProUGUI submitButtonText;
    [SerializeField]
    private ErrorAnimated errorAnimated;
    private GameObject _touchBlocker;

    public void ClearPwdFields()
    {
        newPasswordField.text = "";
        currentPasswordField.text = "";
    }

    public async void UpdateInGameUserPassword()
    {
        if (ApiManager.IsTrainer) { return; }

        if (newPasswordField.text != "")
        {
            if (!newPasswordField.text.PasswordCheck())
            {
                errorAnimated.DisplayAnimatedError("The new Password does not meet the criteria.");
                return;
            }
        }

        if (currentUsernameField.text != PlayerData.Shared.Username)
        {
            if (!currentUsernameField.text.UsernameCheck())
            {
                errorAnimated.DisplayAnimatedError("The new Username does not meet the criteria.");
                return;
            }
        }
        submitButton.interactable = false;
        submitButtonText.text = "Submitting . . .";
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));

        var response = await ApiManager.Instance.UpdateUserData(new()
        {
            username = PlayerData.Shared.Username,
            newUsername = newUsernameField.text,
            password = currentPasswordField.text,
            newPassword = newPasswordField.text
        });

        submitButton.interactable = true;
        submitButtonText.text = "Submit";
        _touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(_touchBlocker);
        
        if (!response.wasSuccess)
        {
            errorAnimated.DisplayAnimatedError("Mmm something went wrong on our end. We will be looking into it. Please try again later.");
        }
    }

    public void UpdateFieldsWithInfo()
    {
        currentUsernameField.text = PlayerData.Shared.Username;
        submitButton.interactable = true;
        submitButtonText.text = "Submit";
        emailField.text = PlayerData.Shared.Email;
    }
}
