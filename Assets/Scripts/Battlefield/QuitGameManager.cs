using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameTime, gameTurns, coinsLost, coinsLeft;
    
    public void SetupSurrenderScreen()
    {
        GameOverVisual.isGameOver = true;
        PlayerData.shared.playerScore -= BattleVars.shared.enemyAiData.scoreWin / 2;
        PlayerData.shared.gamesLost++;
        PlayerData.shared.playerScore = PlayerData.shared.playerScore < 0 ? 0 : PlayerData.shared.playerScore;
        double gameTimeInSeconds = TimeSpan.FromTicks(DateTime.Now.Ticks - BattleVars.shared.gameStartInTicks).TotalSeconds;
        gameTime.text = $"Game time: {(int)gameTimeInSeconds}  seconds";
        gameTurns.text = $"Game length: {BattleVars.shared.turnCount}  turns";

        coinsLost.text = $"You lost: {BattleVars.shared.enemyAiData.costToPlay}";
        coinsLeft.text = $"Electrum coins left: {PlayerData.shared.electrum}";
    }


    public void MoveToDashboardScene()
    {
        SceneManager.LoadScene("Dashboard");
    }
}
