using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerScore, playerWins, playerLoses, electrumCount, oracleText, saveStatus;
    [SerializeField]
    private Button oracleButton, falseGobButton;
    private static GameObject touchBlocker;
    // Start is called before the first frame update
    void Start()
    {
        UpdateDashboard();
        InvokeRepeating(nameof(PeriodicSave), 0f, 300f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void PeriodicSave()
    {
        if (ApiManager.isTrainer)
        {
            return;
        }
        PlayerData.SaveData();
        if (PlayerPrefs.GetInt("IsGuest") == 1)
        {
            PlayerData.SaveData();
        }
        else
        {
            touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), transform.Find("Background/MainPanel"));
            StartCoroutine(ApiManager.shared.SaveToApi(AccountSuccess, AccountFailure));
        }
    }


    private void AccountSuccess(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        saveStatus.transform.parent.gameObject.SetActive(true);
        saveStatus.text = "Save Success";
        Invoke("HideSaveStatus", 3f);
    }

    private void HideSaveStatus()
    {
        saveStatus.transform.parent.gameObject.SetActive(false);
    }

    private void AccountFailure(AccountResponse accountResponse)
    {
        touchBlocker.GetComponentInChildren<ServicesSpinner>().StopAllCoroutines();
        Destroy(touchBlocker);
        saveStatus.transform.parent.gameObject.SetActive(true);
        saveStatus.text = "Save Failed";
        Invoke("HideSaveStatus", 3f);
    }
    public void UpdateDashboard()
    {
        playerScore.text = PlayerData.shared.playerScore.ToString();
        playerWins.text = PlayerData.shared.gamesWon.ToString();
        playerLoses.text = PlayerData.shared.gamesLost.ToString();
        electrumCount.text = PlayerData.shared.electrum.ToString();
        oracleButton.interactable = false;
        falseGobButton.interactable = PlayerData.shared.currentQuestIndex >= 7;
        if (PlayerPrefs.GetInt("IsGuest") == 1)
        {
            oracleButton.interactable = PlayerData.shared.dayLastOraclePlay.Day < System.DateTime.Today.Day;
        }
        else
        {
            oracleButton.interactable = !PlayerData.shared.playedOracleToday;
        }
        oracleText.text = oracleButton.interactable ? "See what the Oracle has for you today!" : "You cannot visit the Oracle yet";
    }
}
