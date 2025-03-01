using System;
using Core;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerOverallScore, playerSeasonScore, playerWins, playerLoses, electrumCount, oracleText, saveStatus, versionLabel;
    [SerializeField]
    private Button oracleButton, falseGobButton, achieveButton, redeemButton;

    [SerializeField] private GameObject redeemPopUp, popUpModal, popUpObject;
    [SerializeField] private Transform mainPanel;
    private static GameObject _touchBlocker;

    private void Start()
    {
        UpdateDashboard();
        InvokeRepeating(nameof(PeriodicSave), 0f, 780f);
        // GetAchievementsResponse();
    //     achieveButton.enabled = RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.Achievements);
    //     redeemButton.enabled = RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.CodeRedeem);
    }

    private async void GetAchievementsResponse()
    {
        if (PlayerPrefs.GetInt("IsGuest") == 1 || ApiManager.IsTrainer)
        {
            achieveButton.enabled = false;
            redeemButton.enabled = false;
            return;
        }
        var achievements = await ApiManager.Instance.GetPlayersAchievements();
        SessionManager.Instance.Achievements = achievements.achievements;
    }

    public void DisplayRedeemCodePopUp()
    {
        if (RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.CodeRedeem))
        {
            redeemPopUp.SetActive(true);
            return;
        }
        popUpObject = Instantiate(popUpModal, mainPanel);
        popUpObject.GetComponent<PopUpModal>().SetupModal("Dashboard", "RemoteConfigFeatureNotEnabledTitle", 
            "RemoteConfigFeatureNotEnabledButtonTitle", DismissPopUp);
    }

    public async void GoToGameAchievements()
    {
        Debug.Log(RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.Achievements));
        if (RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.Achievements))
        {
            await SessionManager.Instance.GetPlayerAchievements();
            SceneTransitionManager.Instance.LoadScene("AchievementNexus");
            return;
        }

        popUpObject = Instantiate(popUpModal, mainPanel);
        popUpObject.GetComponent<PopUpModal>().SetupModal("Dashboard", "RemoteConfigFeatureNotEnabledTitle", 
            "RemoteConfigFeatureNotEnabledButtonTitle", DismissPopUp);
    }

    private void DismissPopUp()
    {
        Destroy(popUpObject);
    }
    
    private void OnDisable()
    {
        CancelInvoke();
    }

    public async void PeriodicSave()
    {
        if (ApiManager.IsTrainer) return;
        await ApiManager.Instance.SaveGameData();

        saveStatus.transform.parent.gameObject.SetActive(true);
        saveStatus.text = "Save Success";
        Invoke(nameof(HideSaveStatus), 3f);
    }

    public async void LogoutUser()
    {
        await ApiManager.Instance.LogoutUser();
        SceneTransitionManager.Instance.LoadScene("LoginScreen");
    }

    private void HideSaveStatus()
    {
        saveStatus.transform.parent.gameObject.SetActive(false);
    }

    public async void UpdateDashboard()
    {
        versionLabel.text = $"Version {Application.version}";
        playerOverallScore.text = SessionManager.Instance.PlayerScore.overallScore.ToString();
        playerSeasonScore.text = SessionManager.Instance.PlayerScore.seasonalScore.ToString();
        playerWins.text = PlayerData.Shared.GamesWon.ToString();
        playerLoses.text = PlayerData.Shared.GamesLost.ToString();
        electrumCount.text = PlayerData.Shared.Electrum.ToString();
        oracleButton.interactable = false;
        falseGobButton.interactable = PlayerData.Shared.CurrentQuestIndex >= 7;

        oracleButton.interactable = await ApiManager.Instance.CheckOraclePlay();
        oracleText.text = oracleButton.interactable ? "See what the Oracle has for you today!" : "You cannot visit the Oracle yet";
    }
}
