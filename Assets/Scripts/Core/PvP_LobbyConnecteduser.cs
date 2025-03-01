using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PvPLobbyConnecteduser : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI youUsername, youScore, youWin, youLose, oppUsername, oppScore, oppWin, oppLose;

    private bool _shouldUpdateOpInfo;

    private PvpUserInfo _opponenInfo;
    public void SetupPvpScreen(PvpUserInfo opponent)
    {
        youUsername.text = PlayerData.Shared.Username;
        youScore.text = PlayerData.Shared.PlayerScore.ToString();
        youWin.text = PlayerData.Shared.GamesWon.ToString();
        youLose.text = PlayerData.Shared.GamesLost.ToString();

        if (opponent == null) { return; }
        oppUsername.text = opponent.Username;
        oppLose.text = opponent.Lost.ToString();
        oppWin.text = opponent.Win.ToString();
        oppScore.text = opponent.Score.ToString();
    }

    public void UpdateOpInfo(PvpUserInfo opponent)
    {
        _opponenInfo = opponent;
        _shouldUpdateOpInfo = true;
    }

    private void Update()
    {
        if (_shouldUpdateOpInfo)
        {
            SetupPvpScreen(_opponenInfo);
            _shouldUpdateOpInfo = false;
        }
        if (_shouldMoveToBattlefield)
        {
            SceneManager.LoadScene("PvPBattlefield");
            _shouldMoveToBattlefield = false;
        }
    }
    private bool _shouldMoveToBattlefield;
    public void MoveToPvpBattlefield()
    {
        _shouldMoveToBattlefield = true;
    }
}
