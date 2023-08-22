using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dash_AccountManagement : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameField, emailField, currentPasswordField, newPasswordField, confirmNewPasswordField;
    [SerializeField]
    private Button submitButton;
    [SerializeField]
    private TextMeshProUGUI submitButtonText;
    [SerializeField]
    private Error_Animated error_Animated;
    private GameObject touchBlocker;

    public void ShouldEnableButton()
    {
        if (currentPasswordField.text == "" || currentPasswordField.text == null) { return; }
        if (!VerifyEmailAddress(emailField.text)) { return; }
        if (newPasswordField.text != confirmNewPasswordField.text) { return; }
        if (usernameField.text == "" || usernameField.text == null) { return; }

        submitButton.interactable = true;
        submitButtonText.text = "Submit";
    }

    public void ClearPwdFields()
    {
        newPasswordField.text = "";
        confirmNewPasswordField.text = "";
        currentPasswordField.text = "";
    }

    public void SubmitNewAccountChanges()
    {
        if (ApiManager.isTrainer) { return; }
        AccountRequest accountRequest = new AccountRequest
        {
            OldPassword = currentPasswordField.text,
            Username = usernameField.text,
            NewEmailAddress = emailField.text,
            NewPassword = newPasswordField.text,
            SavedData = PlayerData.shared
        };
        submitButton.interactable = false;
        submitButtonText.text = "Submitting . . .";

        StartCoroutine(ApiManager.shared.UpdateUserAccount(accountRequest, AccountUpdateSuccess, AccountUpdateFailure));
        touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
    }


    public void AccountUpdateSuccess(AccountResponse accountResponse)
    {
        usernameField.text = accountResponse.username;
        emailField.text = accountResponse.emailAddress;

        submitButton.interactable = false;
        submitButtonText.text = "Submit";
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
    }
    public void AccountUpdateFailure(AccountResponse accountResponse)
    {
        switch (accountResponse.errorMessage)
        {
            case ErrorCases.UserNameInUse:
                error_Animated.DisplayAnimatedError("Username is already in use. Please try again.");
                break;
            case ErrorCases.IncorrectPassword:
                error_Animated.DisplayAnimatedError("The current password you provided is incorrect. Please try again.");
                break;
            default:
                error_Animated.DisplayAnimatedError("Mmm something went wrong on our end. We will be looking into it. Please try again later.");
                break;
        }
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
    }

    public void UpdateFieldsWithInfo()
    {
        usernameField.text = ApiManager.shared.GetEmail();
        emailField.text = usernameField.text == "" ? "Not yet set!" : usernameField.text;
        submitButton.interactable = false;
        submitButtonText.text = "Submit";
    }

    private bool VerifyEmailAddress(string address)
    {
        string[] atCharacter;
        string[] dotCharacter;
        atCharacter = address.Split("@"[0]);
        if (atCharacter.Length == 2)
        {
            dotCharacter = atCharacter[1].Split("."[0]);
            if (dotCharacter.Length >= 2)
            {
                if (dotCharacter[dotCharacter.Length - 1].Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
