using System;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameManager : MonoBehaviour
{
    private GameObject _touchBlocker;
    [SerializeField] private TextMeshProUGUI gameTime, gameTurns, coinsLost, coinsLeft;
    [SerializeField] private Button mainMenuButton;

    public async void SetupSurrenderScreen()
    {
        DuelManager.Instance.SetGameOver(true);
        if (BattleVars.Shared.IsArena)
        {
            PlayerData.Shared.arenaLosses++;
        }

        coinsLost.text = $"You lost: {BattleVars.Shared.EnemyAiData.costToPlay.ToString()}";
        coinsLeft.text = $"Electrum coins left: {PlayerData.Shared.electrum.ToString()}";
        var gameTimeInSeconds = (DateTime.Now - BattleVars.Shared.GameStartInTicks).TotalSeconds;
        gameTime.text = $"Game Time: {BattleVars.Shared.TurnCount} seconds";
        gameTurns.text = $"Game Length: {(int)gameTimeInSeconds} turns";
        
        
        PlayerData.Shared.gamesLost++;
        PlayerData.Shared.playerScore -= BattleVars.Shared.EnemyAiData.scoreWin / 2;
        PlayerData.Shared.playerScore = PlayerData.Shared.playerScore < 0 ? 0 : PlayerData.Shared.playerScore;
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"),
            GameObject.Find("QuitGameScreen").transform);
        await ApiManager.Instance.SaveGameStats(new (BattleVars.Shared.EnemyAiData, true,
            BattleVars.Shared.IsArena));
        Destroy(_touchBlocker);
        mainMenuButton.interactable = true;
    }


    public void MoveToDashboardScene()
    {
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }
}
