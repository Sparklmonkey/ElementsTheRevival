using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DashboardPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerScore, playerWins, playerLoses, electrumCount, oracleText, saveStatus;
    [SerializeField]
    private Button oracleButton, falseGobButton;
    private static GameObject touchBlocker;

    void Start()
    {
        UpdateDashboard();
        InvokeRepeating(nameof(PeriodicSave), 0f, 300f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public async void PeriodicSave()
    {
        if (ApiManager.isTrainer)
        {
            return;
        }
        await ApiManager.shared.SaveDataToUnity();

        saveStatus.transform.parent.gameObject.SetActive(true);
        saveStatus.text = "Save Success";
        Invoke(nameof(HideSaveStatus), 3f);
    }

    public void LogoutUser()
    {
        ApiManager.shared.LogoutUser();
        SceneManager.LoadScene("LoginScreen");
    }

    private void HideSaveStatus()
    {
        saveStatus.transform.parent.gameObject.SetActive(false);
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
