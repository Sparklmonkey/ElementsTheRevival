using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaDataManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI oppName, oppScore, oppWin, oppLoss, oppRank, playerScore, playerWin, playerLoss, responseText;
    [SerializeField]
    private Image playerMark, oppMark;
    [SerializeField]
    private GameObject oppInfo, startGameBtn;

    private static EnemyAi _enemyAi;
    private static ArenaResponse _arenaResponse;
    // Start is called before the first frame update
    private async void Start()
    {
        playerScore.text = PlayerData.Shared.playerScore.ToString();
        playerWin.text = PlayerData.Shared.arenaWins.ToString();
        playerLoss.text = PlayerData.Shared.arenaLosses.ToString();
        BattleVars.Shared.ResetBattleVars();
        _enemyAi = Resources.Load<EnemyAi>("EnemyAi/Arena/Random");
        playerMark.sprite = ImageHelper.GetElementImage(PlayerData.Shared.arenaT50Mark.ToString());
        if (_arenaResponse == null)
        {
            _arenaResponse = await ApiManager.Instance.GetT50Opponent();
        }
        ArenaResponseHandler(_arenaResponse);
    }

    public void StartGame()
    {

        if (PlayerData.Shared.arenaT50Deck.Count < 30)
        {
            responseText.text = "Please set a deck to use in Arena T50. \n You can do so by tapping the 'Modify Deck' button";
        }
        else
        {
            BattleVars.Shared.IsArena = true;
            BattleVars.Shared.EnemyAiData = _enemyAi;
            _arenaResponse = null;
            SceneTransitionManager.Instance.LoadScene("Battlefield");
        }
    }

    private void ArenaResponseHandler(ArenaResponse arenaResponse)
    {
        if (arenaResponse.arenaT50Deck.Count == 0)
        {
            responseText.text = "No opponent found, try again later";
            return;
        }

        oppInfo.SetActive(true);
        oppMark.sprite = ImageHelper.GetElementImage(((Element)arenaResponse.arenaT50Mark).FastElementString());
        oppName.text = arenaResponse.username;
        _enemyAi.opponentName = arenaResponse.username;
        _enemyAi.mark = (Element)arenaResponse.arenaT50Mark;
        _enemyAi.deck = string.Join(" ", arenaResponse.arenaT50Deck);
        oppWin.text = arenaResponse.arenaWins.ToString();
        oppScore.text = arenaResponse.playerScore.ToString();
        oppLoss.text = arenaResponse.arenaLoses.ToString();
        oppRank.text = arenaResponse.arenaRank.ToString();
        _arenaResponse = arenaResponse;
        startGameBtn.SetActive(true);
        responseText.text = $"Your opponent is {arenaResponse.username}";
    }

    public void ReturnToMenu()
    {
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }
    public void GoToDeckManager()
    {
        DeckDisplayManager.IsArena = true;
        SceneTransitionManager.Instance.LoadScene("DeckManagement");
    }
}
