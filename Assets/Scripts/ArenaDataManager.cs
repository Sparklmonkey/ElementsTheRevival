using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if(arenaResponse == null)
        {
            await ApiManager.shared.GetT50Opponent(ArenaResponseHandler);
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
            SceneManager.LoadScene("Battlefield");
        }
    }
    public void TestSelf()
    {
        if (PlayerData.shared.arenaT50Deck.Count < 30)
        {
            responseText.text = "Please set a deck to use in Arena T50. \n You can do so by tapping the 'Modify Deck' button";
        }
        else
        {
            BattleVars.shared.isArena = true;
            BattleVars.shared.isTest = true;
            BattleVars.shared.enemyAiData = enemyAi;
            BattleVars.shared.enemyAiData.opponentName = PlayerData.shared.userName;
            BattleVars.shared.enemyAiData.deck = string.Join(" ", PlayerData.shared.arenaT50Deck);
            BattleVars.shared.enemyAiData.mark = PlayerData.shared.arenaT50Mark;
            arenaResponse = null;
            SceneManager.LoadScene("Battlefield");
        }
    }

    private void ArenaResponseHandler(ArenaResponse arenaResponse)
    {
        if(arenaResponse.opponentDeck.Count == 0)
        {
            responseText.text = "No opponent found, try again later";
            return;
        }


        oppInfo.SetActive(true);
        oppMark.sprite = ImageHelper.GetElementImage(((Element)arenaResponse.deckMark).FastElementString());
        oppName.text = arenaResponse.username;
        enemyAi.opponentName = arenaResponse.username;
        enemyAi.mark = (Element)arenaResponse.deckMark;
        enemyAi.deck = string.Join(" ", arenaResponse.opponentDeck);
        oppWin.text = arenaResponse.arenaWins.ToString();
        oppScore.text = arenaResponse.playerScore.ToString();
        oppLoss.text = arenaResponse.arenaLoses.ToString();
        oppRank.text = arenaResponse.arenaRank.ToString();
        ArenaDataManager.arenaResponse = arenaResponse;
        startGameBtn.SetActive(true);
        responseText.text = $"Your opponent is {arenaResponse.username}";
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Dashboard");
    }

    public void GoToDeckManager()
    {
        DeckDisplayManager.isArena = true;
        SceneManager.LoadScene("DeckManagement");
    }
}
