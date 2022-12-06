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
        DuelManager.enemy.StopAllCoroutines();
        DuelManager.player.StopAllCoroutines();

        BattleVars.shared.playerHP = DuelManager.player.healthManager.GetCurrentHealth();
        isGameOver = true;
        instance.StopAllCoroutines();
        Command.CommandQueue.Clear();
        Command.playingQueue = false;
        didWinStatic = didWin;
        gameOverTextStatic.gameObject.SetActive(true);
        continueButtonStatic.gameObject.SetActive(true);
        rayBlockerStatic.gameObject.SetActive(true);
        gameOverTextStatic.text = didWin ? "You Won!" : "You Lost";
        if(BattleVars.shared.isTest){return;}
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
            if (BattleVars.shared.isArena)
            {
                PlayerData.shared.arenaWins++;
            }
            ApiManager.shared.StartCoroutine(ApiManager.shared.UpdateGameStats(new GameStatRequest(BattleVars.shared.enemyAiData, true, BattleVars.shared.isArena), GameStatHandler));

            PlayerData.shared.electrum += BattleVars.shared.enemyAiData.costToPlay;
        }
        else
        {
            if (BattleVars.shared.isArena)
            {
                PlayerData.shared.arenaLosses++;
            }
            PlayerData.shared.gamesLost++;
            PlayerData.shared.playerScore -= BattleVars.shared.enemyAiData.scoreWin / 2;
            PlayerData.shared.playerScore = PlayerData.shared.playerScore < 0 ? 0 : PlayerData.shared.playerScore;
            ApiManager.shared.StartCoroutine(ApiManager.shared.UpdateGameStats(new GameStatRequest(BattleVars.shared.enemyAiData, false, BattleVars.shared.isArena), GameStatHandler));
            

        }

        PlayerData.SaveData();
    }

    public static void GameStatHandler(bool wasSuccess)
    {

    }

    public void MoveToSpinnerScreen()
    {
        PlayerData.SaveData();
        if(didWinStatic && !BattleVars.shared.isTest)
        {
            SceneManager.LoadScene("SpinnerScene");
        }
        else
        {
            BattleVars.shared.isTest = false;
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
