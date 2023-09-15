using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameOverVisual : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI gameOverText;
    [SerializeField]
    public Button continueButton;
    [SerializeField]
    public Image rayBlocker;

    private static GameObject _touchBlocker;
    private static GameOverVisual _instance;
    private static TextMeshProUGUI _gameOverTextStatic;
    private static Button _continueButtonStatic;
    private static Image _rayBlockerStatic;
    private static bool _didWinStatic;
    public static bool IsGameOver = false;

    public async static void ShowGameOverScreen(bool didWin)
    {
        DuelManager.Instance.enemy.StopAllCoroutines();
        DuelManager.Instance.player.StopAllCoroutines();

        BattleVars.Shared.PlayerHp = DuelManager.Instance.player.HealthManager.GetCurrentHealth();
        IsGameOver = true;
        _didWinStatic = didWin;
        _gameOverTextStatic.gameObject.SetActive(true);
        _rayBlockerStatic.gameObject.SetActive(true);
        _gameOverTextStatic.text = didWin ? "You Won!" : "You Lost";
        if (BattleVars.Shared.IsTest) { return; }
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), GameObject.Find("GameOverManager").transform);
        if (didWin)
        {
            if (DuelManager.Instance.player.HealthManager.IsMaxHealth())
            {
                BattleVars.Shared.ElementalMastery = true;
            }
            PlayerData.Shared.gamesWon++;

            if (BattleVars.Shared.EnemyAiData.spins == 0)
            {
                PlayerData.Shared.hasDefeatedLevel0 = true;
            }

            if (BattleVars.Shared.EnemyAiData.spins == 1)
            {
                PlayerData.Shared.hasDefeatedLevel1 = true;
            }

            if (BattleVars.Shared.EnemyAiData.spins == 2)
            {
                PlayerData.Shared.hasDefeatedLevel2 = true;
            }
            if (BattleVars.Shared.IsArena)
            {
                PlayerData.Shared.arenaWins++;
            }
            GameStats.Shared.UpdateValues(new GameStatRequest(BattleVars.Shared.EnemyAiData, true, BattleVars.Shared.IsArena));
            await ApiManager.Instance.SaveGameStats();
            PlayerData.Shared.electrum += BattleVars.Shared.EnemyAiData.costToPlay;
        }
        else
        {
            if (BattleVars.Shared.IsArena)
            {
                PlayerData.Shared.arenaLosses++;
            }
            PlayerData.Shared.gamesLost++;
            PlayerData.Shared.playerScore -= BattleVars.Shared.EnemyAiData.scoreWin / 2;
            PlayerData.Shared.playerScore = PlayerData.Shared.playerScore < 0 ? 0 : PlayerData.Shared.playerScore;
            GameStats.Shared.UpdateValues(new GameStatRequest(BattleVars.Shared.EnemyAiData, true, BattleVars.Shared.IsArena));
            await ApiManager.Instance.SaveGameStats();
        }

        Destroy(_touchBlocker);
        _continueButtonStatic.gameObject.SetActive(true);
        PlayerData.SaveData();
    }


    public void MoveToSpinnerScreen()
    {
        PlayerData.SaveData();
        if (_didWinStatic && !BattleVars.Shared.IsTest)
        {
            SceneTransitionManager.Instance.LoadScene("SpinnerScene");
        }
        else
        {
            BattleVars.Shared.IsTest = false;
            SceneTransitionManager.Instance.LoadScene("Dashboard");
        }
    }

    private void Start()
    {
        _gameOverTextStatic = gameOverText;
        _continueButtonStatic = continueButton;
        _rayBlockerStatic = rayBlocker;
        _instance = this;
    }
}
