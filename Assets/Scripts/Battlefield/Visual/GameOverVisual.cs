using System;
using Core;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GameOverVisual : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI gameOverText;
    [SerializeField]
    public Button continueButton;
    [SerializeField]
    public Image rayBlocker;
    private bool _didPlayerWin = false;

    private GameObject _touchBlocker;
    public bool isGameOver = false;

    private EventBinding<GameEndEvent> _gameEndBinding;
    
    private void OnDisable() {
        EventBus<GameEndEvent>.Unregister(_gameEndBinding);
    }

    private void OnEnable()
    {
        _gameEndBinding = new EventBinding<GameEndEvent>(EndGame);
        EventBus<GameEndEvent>.Register(_gameEndBinding);
    }

    private void EndGame(GameEndEvent gameEndEvent)
    {
        if (isGameOver) return;
        ShowGameOverScreen(gameEndEvent.Owner.Equals(OwnerEnum.Player));
    }
    
    private async void ShowGameOverScreen(bool didWin)
    {
        DuelManager.Instance.StopAllRunningRoutines();
        EventBus<GameEndEvent>.Unregister(_gameEndBinding);

        BattleVars.Shared.PlayerHp = DuelManager.Instance.player.HealthManager.GetCurrentHealth();
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        rayBlocker.gameObject.SetActive(true);
        gameOverText.text = didWin ? "You Won!" : "You Lost";
        _didPlayerWin = didWin;
        if (BattleVars.Shared.IsTest) { return; }
        _touchBlocker = Instantiate(Resources.Load<GameObject>("Prefabs/TouchBlocker"), GameObject.Find("GameOverManager").transform);
        // _touchBlocker.transform.SetSiblingIndex(0);
        if (_didPlayerWin)
        {
            if (DuelManager.Instance.player.HealthManager.IsMaxHealth())
            {
                BattleVars.Shared.ElementalMastery = true;
            }
            PlayerData.Shared.gamesWon++;

            switch (BattleVars.Shared.EnemyAiData.spins)
            {
                case 0:
                    PlayerData.Shared.hasDefeatedLevel0 = true;
                    break;
                case 1:
                    PlayerData.Shared.hasDefeatedLevel1 = true;
                    break;
                case 2:
                    PlayerData.Shared.hasDefeatedLevel2 = true;
                    break;
            }

            if (BattleVars.Shared.IsArena)
            {
                PlayerData.Shared.arenaWins++;
            }
            
            await ApiManager.Instance.SaveGameStats(new (BattleVars.Shared.EnemyAiData, true, BattleVars.Shared.IsArena));
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
            var newScore = await ApiManager.Instance.UpdateScore(-BattleVars.Shared.EnemyAiData.scoreWin / 2);
            SessionManager.Instance.PlayerScore = newScore;
            await ApiManager.Instance.SaveGameStats(new (BattleVars.Shared.EnemyAiData, true, BattleVars.Shared.IsArena));
        }   

        Destroy(_touchBlocker);
        continueButton.gameObject.SetActive(true);
        continueButton.interactable = true;
        PlayerData.SaveData();
    }


    public void MoveToSpinnerScreen()
    {
        EventBusUtil.ClearAllBuses();
        PlayerData.SaveData();
        if (_didPlayerWin && !BattleVars.Shared.IsTest)
        {
            SceneTransitionManager.Instance.LoadScene("SpinnerScene");
        }
        else
        {
            BattleVars.Shared.IsTest = false;
            SceneTransitionManager.Instance.LoadScene("Dashboard");
        }
    }
}
