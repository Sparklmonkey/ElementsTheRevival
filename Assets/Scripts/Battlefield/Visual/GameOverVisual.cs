using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverVisual : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI gameOverText;
    [SerializeField]
    public Button continueButton;
    [SerializeField]
    public Image rayBlocker;

    private static GameOverVisual instance;
    private static TextMeshProUGUI gameOverTextStatic;
    private static Button continueButtonStatic;
    private static Image rayBlockerStatic;
    private static bool didWinStatic;
    public static bool isGameOver = false;

    public static void ShowGameOverScreen(bool didWin)
    {
        if(isGameOver) { return; }
        isGameOver = true;
        instance.StopAllCoroutines();
        Command.CommandQueue.Clear();
        Command.playingQueue = false;
        didWinStatic = didWin;
        gameOverTextStatic.gameObject.SetActive(true);
        continueButtonStatic.gameObject.SetActive(true);
        rayBlockerStatic.gameObject.SetActive(true);
        gameOverTextStatic.text = didWin ? "You Won!" : "You Lost";
        if (didWin)
        {
            if(DuelManager.player.healthManager.IsMaxHealth())
            {
                BattleVars.shared.elementalMastery = true;
            }
            PlayerData.shared.gamesWon++;

            if (BattleVars.shared.enemyAiData.spins == 0)
            {
                PlayerData.shared.hasDefeatedLevel0 = true;
            }

            if (BattleVars.shared.enemyAiData.spins == 1)
            {
                PlayerData.shared.hasDefeatedLevel1 = true;
            }

            if (BattleVars.shared.enemyAiData.spins == 2)
            {
                PlayerData.shared.hasDefeatedLevel2 = true;
            }

            PlayerData.shared.playerScore += BattleVars.shared.enemyAiData.scoreWin;
        }
        else
        {
            PlayerData.shared.gamesLost++;
            PlayerData.shared.playerScore -= BattleVars.shared.enemyAiData.scoreWin / 2;
            PlayerData.shared.playerScore = PlayerData.shared.playerScore < 0 ? 0 : PlayerData.shared.playerScore;

        }

        PlayerData.SaveData();
    }

    public void MoveToSpinnerScreen()
    {
        PlayerData.SaveData();
        if(didWinStatic)
        {
            SceneManager.LoadScene("SpinnerScene");
        }
        else
        {
            SceneManager.LoadScene("Dashboard");
        }
    }

    private void Start()
    {
        gameOverTextStatic = gameOverText;
        continueButtonStatic = continueButton;
        rayBlockerStatic = rayBlocker;
        instance = this;
    }
}
