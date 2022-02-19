using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dash_ResetAccountManager : MonoBehaviour
{

    [SerializeField]
    private Error_Animated errorMessageManager;
    private GameObject touchBlocker;

    public void ConfirmResetText(TMP_InputField inputField)
    {
        if(inputField.text == "DELETE")
        {
            string objectId = PlayerData.shared.id;
            PlayerData.shared = new PlayerData();
            PlayerData.shared.id = objectId;
            if (PlayerPrefs.GetInt("IsGuest") == 1)
            {
                PlayerData.SaveData();
                GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
            }
            else
            {
                StartCoroutine(ApiManager.shared.SaveToApi(SuccessHandler, FailureHandler));
                touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            }
        }
        else
        {
            errorMessageManager.DisplayAnimatedError("Input text is not correct. Try Again.");
        }
    }

    private void SuccessHandler(AccountResponse accountResponse)
    {
        Destroy(touchBlocker.gameObject);
        GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
    }

    private void FailureHandler(AccountResponse accountResponse)
    {
        Destroy(touchBlocker.gameObject);
        errorMessageManager.DisplayAnimatedError("There was an unexpected error. Try Again.");
    }
}
