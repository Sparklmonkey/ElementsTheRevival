using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerScore, playerWins, playerLoses, electrumCount, oracleText, saveStatus;
    [SerializeField]
    private Button oracleButton, falseGobButton;
    private static GameObject _touchBlocker;

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
        if (ApiManager.IsTrainer)
        {
            return;
        }
        await ApiManager.Instance.SaveDataToUnity();

        saveStatus.transform.parent.gameObject.SetActive(true);
        saveStatus.text = "Save Success";
        Invoke(nameof(HideSaveStatus), 3f);
    }

    public void LogoutUser()
    {
        ApiManager.Instance.LogoutUser();
        SceneTransitionManager.Instance.LoadScene("LoginScreen");
    }

    private void HideSaveStatus()
    {
        saveStatus.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateDashboard()
    {
        playerScore.text = PlayerData.Shared.playerScore.ToString();
        playerWins.text = PlayerData.Shared.gamesWon.ToString();
        playerLoses.text = PlayerData.Shared.gamesLost.ToString();
        electrumCount.text = PlayerData.Shared.electrum.ToString();
        oracleButton.interactable = false;
        falseGobButton.interactable = PlayerData.Shared.currentQuestIndex >= 7;
        if (PlayerPrefs.GetInt("IsGuest") == 1)
        {
            oracleButton.interactable = PlayerData.Shared.DayLastOraclePlay.Day < System.DateTime.Today.Day;
        }
        else
        {
            oracleButton.interactable = !PlayerData.Shared.playedOracleToday;
        }
        oracleText.text = oracleButton.interactable ? "See what the Oracle has for you today!" : "You cannot visit the Oracle yet";
    }
}
