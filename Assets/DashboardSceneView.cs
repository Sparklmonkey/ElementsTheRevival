using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardSceneView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerOverallScore, playerSeasonScore, playerWins, playerLoses, electrumCount, oracleText, saveStatus, versionLabel;
    [SerializeField]
    private Button oracleButton, falseGobButton, achieveButton, redeemButton;

    [SerializeField] private GameObject redeemPopUp, popUpModal, popUpObject;
    [SerializeField] private Transform mainPanel;
    private static GameObject _touchBlocker;

    private DashboardSceneModel _model;
    private DashboardSceneViewModel _viewModel;
    private void Awake()
    {
        _viewModel = new DashboardSceneViewModel();
        SetupUI();
    }

    private void SetupUI()
    {
        playerOverallScore.text = _model.PlayerOverallScore.ToString();
        playerSeasonScore.text = _model.PlayerSeasonScore.ToString();
        playerWins.text = _model.PlayerWins.ToString();
        playerLoses.text = _model.PlayerLoses.ToString();
        electrumCount.text = _model.Electrum.ToString();
        versionLabel.text = _model.Version;
        oracleButton.interactable = false;
        falseGobButton.interactable = _model.IsFalseGodEnabled;

        // oracleButton.interactable = await ApiManager.Instance.CheckOraclePlay();
        oracleText.text = oracleButton.interactable ? "See what the Oracle has for you today!" : "You cannot visit the Oracle yet";
    }
}

public class DashboardSceneViewModel
{
    private readonly DashboardSceneModel _model;
    
}

public class DashboardSceneModel
{
    public int PlayerOverallScore => SessionManager.Instance.PlayerScore.overallScore;
    public int PlayerSeasonScore => SessionManager.Instance.PlayerScore.seasonalScore;
    public int PlayerWins => PlayerData.Shared.GamesWon;
    public int PlayerLoses => PlayerData.Shared.GamesLost;
    public int Electrum => PlayerData.Shared.Electrum;
    public string Version => Application.version;
    public bool IsOrablePlayable;
    public bool IsFalseGodEnabled => PlayerData.Shared.CurrentQuestIndex >= 7;
    public bool IsAchievementsEnabled => RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.Achievements);
}