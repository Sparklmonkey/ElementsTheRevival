using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dash_AccountManagement : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameField, currentPasswordField, newPasswordField;
    [SerializeField]
    private Button submitButton, deleteAccountButton;
    [SerializeField]
    private TextMeshProUGUI submitButtonText;
    [SerializeField]
    private Error_Animated error_Animated;
    private GameObject touchBlocker;

    private void OnEnable()
    {
        if (ApiManager.Instance.isUnityUser)
        {
            currentPasswordField.gameObject.SetActive(false);
            newPasswordField.gameObject.SetActive(false);
            deleteAccountButton.gameObject.SetActive(true);
        }
        else
        {
            currentPasswordField.gameObject.SetActive(true);
            newPasswordField.gameObject.SetActive(true);
            deleteAccountButton.gameObject.SetActive(false);
        }
    }

    public void ShouldEnableButton()
    {
        if (currentPasswordField.text == "" || currentPasswordField.text == null) { return; }
        if (usernameField.text == "" || usernameField.text == null) { return; }

        submitButton.interactable = true;
        submitButtonText.text = "Submit";
    }

    public void ClearPwdFields()
    {
        newPasswordField.text = "";
        currentPasswordField.text = "";
    }

    public async void UpdateInGameUserPassword()
    {
        if (ApiManager.isTrainer) { return; }

        if(newPasswordField.text != "")
        {
            if (!newPasswordField.text.PasswordCheck())
            {
                error_Animated.DisplayAnimatedError("The new Password does not meet the criteria.");
                return;
            }
        }

        if(usernameField.text != PlayerData.shared.userName)
        {
            if(!usernameField.text.UsernameCheck())
            {
                error_Animated.DisplayAnimatedError("The new Username does not meet the criteria.");
                return;
            }
        }
        submitButton.interactable = false;
        submitButtonText.text = "Submitting . . .";
        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));

        var response = await ApiManager.Instance.UpdateUserData(usernameField.text, currentPasswordField.text, newPasswordField.text);

        submitButton.interactable = true;
        submitButtonText.text = "Submit";
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);

        if (!response)
        {
            error_Animated.DisplayAnimatedError("Mmm something went wrong on our end. We will be looking into it. Please try again later.");
        }
    }

    public void UpdateFieldsWithInfo()
    {
        usernameField.text = PlayerData.shared.userName;
        submitButton.interactable = false;
        submitButtonText.text = "Submit";
    }

    public void ShowUnityPrivacy()
    {
        Application.OpenURL("https://docs.unity.com/ugs/en-us/manual/authentication/manual/mailto:unity-player-login-privacy@unity3d.com");
    }

    public void DeleteUnityAccount()
    {
        Application.OpenURL("https://player-account.unity.com/");
    }
}
