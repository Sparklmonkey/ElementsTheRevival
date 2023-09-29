using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    private static DuelManager _instance;
    public static DuelManager Instance { get { return _instance; } }
    
    [SerializeField]
    private GameOverVisual gameOverVisual;

    public void UpdateNightFallEclipse(bool isAdded, string skill)
    {
        player.CheckEclipseNightfall(isAdded, skill);
        enemy.CheckEclipseNightfall(isAdded, skill);
    }

    public static List<CardObject> OpponentShuffledDeck;
    public static int OpponentCardsTurn, PlayerCardsTurn;


    public void ActivateDeathTriggers()
    {
        player.ActivateDeathTriggers();
        enemy.ActivateDeathTriggers();
    }

    public void SetupHighlights(List<IDCardPair> targets)
    {
        _validTargets = new();
        foreach (var target in targets)
        {
            _validTargets.Add(target);
            if (target.id.owner == OwnerEnum.Player)
            {
                Instance.player.ShowTargetHighlight(target);
                continue;
            }
            Instance.enemy.ShowTargetHighlight(target);
        }
    }

    public int GetPossibleDamage(bool isPlayer)
    {
        if (isPlayer)
        {
            return Instance.enemy.GetPossibleDamage();
        }
        return Instance.player.GetPossibleDamage();
    }

    public PlayerManager player;
    public PlayerManager enemy;

    public Button endTurnButton;
    public TMPro.TextMeshProUGUI enemyName;
    public TMPro.TextMeshProUGUI discardText;
    private static List<IDCardPair> _validTargets;

    private static EnemyController _botContoller;

    public static int FloodCount;

    [SerializeField]
    private GameObject floodImagePlayer, floodImageEnemy;

    [SerializeField]
    private CoinFlip coinFlip;
    public GameObject targetingObject;

    public void AddFloodCount(int amount)
    {
        FloodCount += amount;
        floodImageEnemy.SetActive(FloodCount >= 1);
        floodImagePlayer.SetActive(FloodCount >= 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!allPlayersSetup)
        {
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Space) || !endTurnButton.interactable) return;
        BattleVars.Shared.SpaceTapped = true;
        EndTurn();
    }

    public PlayerManager GetIDOwner(ID iD)
    {
        return iD.owner.Equals(OwnerEnum.Player) ? player : enemy;
    }

    public PlayerManager GetNotIDOwner(ID iD)
    {
        return iD.owner.Equals(OwnerEnum.Player) ? enemy : player;
    }

    public bool allPlayersSetup;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        BattleVars.Shared.ElementalMastery = false;
        gameOverVisual.IsGameOver = false;
        ActionManager.ActionList = new List<ElementAction>();
        endTurnButton.interactable = false;
    }

    private void Start()
    {
        _validTargets = new();
        FloodCount = 0;
        BattleVars.Shared.GameStartInTicks = DateTime.Now.Ticks;
        _botContoller = new EnemyController(enemy);
        enemyName.text = BattleVars.Shared.EnemyAiData.opponentName;
        StartCoroutine(SetupManagers());
    }

    private IEnumerator SetupManagers()
    {
        yield return coinFlip.StartCoroutine(coinFlip.WaitPlease(0.0001f, 1.0f));
        yield return Instance.enemy.StartCoroutine(enemy.SetupPlayerManager(false));
        yield return Instance.player.StartCoroutine(player.SetupPlayerManager(true));

        BattleVars.Shared.IsPlayerTurn = coinFlip.playerStarts;
        if (coinFlip.playerStarts)
        {
            Instance.player.StartTurn();
            MessageManager.Shared.DisplayMessage("Your turn has started!");
            endTurnButton.interactable = true;
        }
        else
        {
            Instance.enemy.StartCoroutine(_botContoller.StartTurn());

        }
    }

    public void EndTurn()
    {
        endTurnButton.interactable = false;
        ResetTargeting();
        BattleVars.Shared.IsSingularity = 0;
        Instance.player.HideAllPlayableGlow();
        if (BattleVars.Shared.IsPlayerTurn)
        {
            if (Instance.player.GetHandCards().Count == 8)
            {
                discardText.gameObject.SetActive(true);
                BattleVars.Shared.HasToDiscard = true;
                return;
            }
        }

        if (IsGameWinForPlayer())
        {
            gameOverVisual.ShowGameOverScreen(true);
            return;
        }

        if (IsGameLossForPlayer())
        {
            gameOverVisual.ShowGameOverScreen(false);
            return;
        }

        if (BattleVars.Shared.IsPlayerTurn)
        {
            player.EndTurnRoutine();

            player.UpdateCounterAndEffects();
            if (gameOverVisual.IsGameOver) { return; }
            OpponentCardsTurn = 0;
            Instance.enemy.StartCoroutine(_botContoller.StartTurn());
            BattleVars.Shared.TurnCount++;
        }
        else
        {
            enemy.EndTurnRoutine();
            enemy.UpdateCounterAndEffects();
            if (gameOverVisual.IsGameOver) { return; }
            PlayerCardsTurn = 0;
            player.StartTurn();
            endTurnButton.interactable = true;
            BattleVars.Shared.SpaceTapped = false;
        }
        BattleVars.Shared.IsPlayerTurn = !BattleVars.Shared.IsPlayerTurn;
    }

    private bool IsGameWinForPlayer()
    {
        return enemy.HealthManager.GetCurrentHealth() <= 0 || enemy.DeckManager.GetDeckCount() == 0;
    }
    private bool IsGameLossForPlayer()
    {
        return player.HealthManager.GetCurrentHealth() <= 0 || player.DeckManager.GetDeckCount() == 0;
    }

    public void ResetTargeting()
    {
        targetingObject.SetActive(false);
        foreach (IDCardPair item in _validTargets)
        {
            item.IsTargeted(false);
        }
        Instance.enemy.playerDisplayer.ShouldShowTarget(false);
        Instance.player.playerDisplayer.ShouldShowTarget(false);
        _validTargets.Clear();
        BattleVars.Shared.AbilityOrigin = null;
        BattleVars.Shared.IsSelectingTarget = false;
    }

    public static List<List<int>> AdrenalineDamageList = new()              { new List<int> { 1, 1, 1, 1 }
                                                                            , new List<int> { 2, 2, 2, 2 }
                                                                            , new List<int> { 3, 3, 3, 3 }
                                                                            , new List<int> { 4, 3, 2 }
                                                                            , new List<int> { 5, 4, 2}
                                                                            , new List<int> { 6, 4, 2 }
                                                                            , new List<int> { 7, 5, 3 }
                                                                            , new List<int> { 8, 6, 3 }
                                                                            , new List<int> { 9, 3}
                                                                            , new List<int> { 10, 4 }
                                                                            , new List<int> { 11, 4}
                                                                            , new List<int> { 12, 4 }
                                                                            , new List<int> { 13,5}
                                                                            , new List<int> { 14,5 }
                                                                            , new List<int> { 15,5 } };

    public int GetCardCount(List<string> cardIds)
    {
        int cardCount = 0;
        cardCount += Instance.player.playerPermanentManager.GetAllValidCardIds().FindAll(x => cardIds.Contains(x.card.iD)).Count;
        cardCount += Instance.enemy.playerPermanentManager.GetAllValidCardIds().FindAll(x => cardIds.Contains(x.card.iD)).Count;
        return cardCount;
    }

    public void IdCardTapped(IDCardPair idCard)
    {
        if (!BattleVars.Shared.IsPlayerTurn) { return; }

        if (_validTargets.Count > 0)
        {
            if (BattleVars.Shared.IsSelectingTarget && _validTargets.Contains(idCard))
            {
                player.ActivateAbility(idCard);
            }
            else
            {
                ResetTargeting();
            }

            return;
        }

        if (!idCard.HasCard()) { return; }

        if (idCard.id.owner.Equals(OwnerEnum.Opponent))
        {
            if (idCard.id.field.Equals(FieldEnum.Hand)) { return; }

            if (enemy.playerCounters.invisibility > 0 && !enemy.cloakIndex.Contains(idCard.id)) { return; }
            player.SetupCardDisplay(idCard);
            return;
        }

        if (BattleVars.Shared.HasToDiscard)
        {
            if (idCard.id.field.Equals(FieldEnum.Hand))
            {
                player.DiscardCard(idCard);
                BattleVars.Shared.HasToDiscard = false;
                discardText.gameObject.SetActive(false);
                EndTurn();
                return;
            }
        }

        if (PlayerPrefs.GetInt("QuickPlay") == 0)
        {
            player.QuickPlay(idCard);
            return;
        }
        player.SetupCardDisplay(idCard);
    }

    public void SetGameOver(bool isGameOver)
    {
        gameOverVisual.IsGameOver = isGameOver;
    }
}
