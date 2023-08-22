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
        if (ApiManager.isTrainer)
        {
            return;
        }
        if (inputField.text == "DELETE")
        {
            int objectId = PlayerData.shared.id;
            PlayerData.shared.ResetAccount();
            PlayerData.shared.id = objectId;
            GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
            //if (PlayerPrefs.GetInt("IsGuest") == 1)
            //{
            //    PlayerData.SaveData();
            //    GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
            //}
            //else
            //{
            //    //StartCoroutine(ApiManager.shared.SaveToApi(SuccessHandler, FailureHandler));
            //    touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
            //    Destroy(touchBlocker);
            //    GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
            //    touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            //}
        }
        else
        {
            errorMessageManager.DisplayAnimatedError("Input text is not correct. Try Again.");
        }
    }

    private void SuccessHandler(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        GetComponent<DashboardSceneManager>().LoadNewScene("DeckSelector");
    }

    private void FailureHandler(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        errorMessageManager.DisplayAnimatedError("There was an unexpected error. Try Again.");
    }
}
