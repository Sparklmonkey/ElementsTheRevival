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

    private static EnemyAi enemyAi;
    private static ArenaResponse arenaResponse;
    // Start is called before the first frame update
    async void Start()
    {
        playerScore.text = PlayerData.shared.playerScore.ToString();
        playerWin.text = PlayerData.shared.arenaWins.ToString();
        playerLoss.text = PlayerData.shared.arenaLosses.ToString();
        BattleVars.shared.ResetBattleVars();
        enemyAi = Resources.Load<EnemyAi>("EnemyAi/Arena/Random");
        playerMark.sprite = ImageHelper.GetElementImage(PlayerData.shared.arenaT50Mark.ToString());
        if (arenaResponse == null)
        {
            await ApiManager.Instance.GetT50Opponent(ArenaResponseHandler);
        }
        else
        {
            ArenaResponseHandler(arenaResponse);
        }
    }

    public void StartGame()
    {

        if (PlayerData.shared.arenaT50Deck.Count < 30)
        {
            responseText.text = "Please set a deck to use in Arena T50. \n You can do so by tapping the 'Modify Deck' button";
        }
        else
        {
            BattleVars.shared.isArena = true;
            BattleVars.shared.enemyAiData = enemyAi;
            arenaResponse = null;
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
        oppName.text = arenaResponse.userName;
        enemyAi.opponentName = arenaResponse.userName;
        enemyAi.mark = (Element)arenaResponse.arenaT50Mark;
        enemyAi.deck = string.Join(" ", arenaResponse.arenaT50Deck);
        oppWin.text = arenaResponse.arenaWins.ToString();
        oppScore.text = arenaResponse.playerScore.ToString();
        oppLoss.text = arenaResponse.arenaLoses.ToString();
        oppRank.text = arenaResponse.arenaRank.ToString();
        ArenaDataManager.arenaResponse = arenaResponse;
        startGameBtn.SetActive(true);
        responseText.text = $"Your opponent is {arenaResponse.userName}";
    }

    public void ReturnToMenu()
    {
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }
    public void GoToDeckManager()
    {
        DeckDisplayManager.isArena = true;
        SceneTransitionManager.Instance.LoadScene("DeckManagement");
    }
}
