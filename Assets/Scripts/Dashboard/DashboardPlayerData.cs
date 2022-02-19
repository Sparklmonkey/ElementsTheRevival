using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerScore, playerWins, playerLoses, electrumCount;
    [SerializeField]
    private Button oracleButton, falseGobButton;
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
        PlayerData.SaveData();
        if (PlayerPrefs.GetInt("IsGuest") == 1)
        {
            PlayerData.SaveData();
        }
        else
        {
            StartCoroutine(ApiManager.shared.SaveToApi(AccountSuccess, AccountFailure));
        }
    }


    private static void AccountSuccess(AccountResponse accountResponse)
    {
        // Maybe Do Something??
    }
    private static void AccountFailure(AccountResponse accountResponse)
    {
        // Maybe Do Something??
    }
    public void UpdateDashboard()
    {
        playerScore.text = PlayerData.shared.playerScore.ToString();
        playerWins.text = PlayerData.shared.gamesWon.ToString();
        playerLoses.text = PlayerData.shared.gamesLost.ToString();
        electrumCount.text = PlayerData.shared.electrum.ToString();
        oracleButton.interactable = false;
        falseGobButton.interactable = PlayerData.shared.currentQuestIndex > 7;
        void OracleUpdate() => oracleButton.interactable = !PlayerData.shared.playedOracleToday;
        StartCoroutine(ApiManager.shared.UpdateOracleDay(OracleUpdate));
    }
}
