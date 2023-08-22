using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PvP_LobbyConnecteduser : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI youUsername, youScore, youWin, youLose, oppUsername, oppScore, oppWin, oppLose;

    private bool shouldUpdateOpInfo;

    private PvpUserInfo opponenInfo;
    public void SetupPvpScreen(PvpUserInfo opponent)
    {
        youUsername.text = PlayerData.shared.userName;
        youScore.text = PlayerData.shared.playerScore.ToString();
        youWin.text = PlayerData.shared.gamesWon.ToString();
        youLose.text = PlayerData.shared.gamesLost.ToString();

        if(opponent == null) { return; }
        oppUsername.text = opponent.Username;
        oppLose.text = opponent.Lost.ToString();
        oppWin.text = opponent.Win.ToString();
        oppScore.text = opponent.Score.ToString();
    }

    public void UpdateOpInfo(PvpUserInfo opponent)
    {
        opponenInfo = opponent;
        shouldUpdateOpInfo = true;
    }

    private void Update()
    {
        if (shouldUpdateOpInfo)
        {
            SetupPvpScreen(opponenInfo);
            shouldUpdateOpInfo = false;
        }
        if (shouldMoveToBattlefield)
        {
            SceneManager.LoadScene("PvPBattlefield");
            shouldMoveToBattlefield = false;
        }
    }
    private bool shouldMoveToBattlefield;
    public void MoveToPvpBattlefield()
    {
        shouldMoveToBattlefield = true;
    }
}
