using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    public static DuelManager Instance { get; private set; }
    
    [SerializeField]
    private GameOverVisual gameOverVisual;
    [SerializeField]
    private MessageManager messageManager;
    private EventBinding<CardTappedEvent> _cardTappedBinding;
    
    private void OnDisable() {
        EventBus<CardTappedEvent>.Unregister(_cardTappedBinding);
    }

    private void OnEnable()
    {
        _cardTappedBinding = new EventBinding<CardTappedEvent>(CardTapped);
        EventBus<CardTappedEvent>.Register(_cardTappedBinding);
    }
    public void UpdateNightFallEclipse(bool isAdded, string skill)
    {
        player.CheckEclipseNightfall(isAdded, skill);
        enemy.CheckEclipseNightfall(isAdded, skill);
    }

    public static List<CardObject> OpponentShuffledDeck;
    public static int OpponentCardsTurn, PlayerCardsTurn;

    public void StopAllRunningRoutines()
    {
        player.StopAllCoroutines();
        enemy.StopAllCoroutines();
        aiController.StopAllCoroutines();
    }
    public void SetupHighlights(List<(ID, Card)> targets)
    {
        _validTargets = new();
        foreach (var target in targets)
        {
            _validTargets.Add(target.Item1, target.Item2);
            EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(true, target.Item1));
        }
    }

    public int GetPossibleDamage(bool isPlayer)
    {
        return isPlayer ? Instance.enemy.GetPossibleDamage() : Instance.player.GetPossibleDamage();
    }

    public PlayerManager player;
    public PlayerManager enemy;

    public Button endTurnButton;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI discardText;
    private static Dictionary<ID,Card> _validTargets;
    
    public static int FloodCount;

    [SerializeField]
    private GameObject floodImagePlayer, floodImageEnemy;

    [SerializeField] private EnemyController aiController;
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
    private void Update()
    {
        if (!allPlayersSetup) return;

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
        
        Instance = this;
        BattleVars.Shared.ElementalMastery = false;
        gameOverVisual.isGameOver = false;
        ActionManager.ActionList = new List<ElementAction>();
        endTurnButton.interactable = false;
    }

    private void Start()
    {
        _validTargets = new();
        FloodCount = 0;
        BattleVars.Shared.GameStartInTicks = DateTime.Now.Ticks;
        aiController.SetupController(enemy, gameOverVisual);
        enemyName.text = BattleVars.Shared.EnemyAiData.opponentName;
        StartCoroutine(SetupManagers());
    }

    private IEnumerator SetupManagers()
    {
        yield return coinFlip.StartCoroutine(coinFlip.WaitPlease(0.0001f, 1.0f));
        yield return Instance.enemy.StartCoroutine(enemy.SetupPlayerManager(OwnerEnum.Opponent));
        yield return Instance.player.StartCoroutine(player.SetupPlayerManager(OwnerEnum.Player));

        BattleVars.Shared.IsPlayerTurn = coinFlip.playerStarts;
        if (coinFlip.playerStarts)
        {
            Instance.player.StartTurn();
            messageManager.DisplayMessage("Your turn has started!");
            endTurnButton.interactable = true;
        }
    }

    public void EndTurn()
    {
        endTurnButton.interactable = false;
        ResetTargeting();
        BattleVars.Shared.IsSingularity = 0;
        EventBus<HideUsableDisplayEvent>.Raise(new HideUsableDisplayEvent());
        if (BattleVars.Shared.IsPlayerTurn)
        {
            if (Instance.player.playerHand.ShouldDiscard())
            {
                discardText.gameObject.SetActive(true);
                BattleVars.Shared.HasToDiscard = true;
                return;
            }
        }

        if (BattleVars.Shared.IsPlayerTurn)
        {
            player.StartCoroutine(player.EndTurnRoutine());
            player.UpdateCounterAndEffects();
            if (ShouldEndGame()) { return; }
            OpponentCardsTurn = 0;
        }
        else
        {
            if (ShouldEndGame()) { return; }
            PlayerCardsTurn = 0;
            player.StartTurn();
            endTurnButton.interactable = true;
            BattleVars.Shared.ChangePlayerTurn();
        }
    }

    private bool ShouldEndGame()
    {
        if (IsGameWinForPlayer())
        {
            gameOverVisual.ShowGameOverScreen(true);
            return true;
        }

        if (IsGameLossForPlayer())
        {
            gameOverVisual.ShowGameOverScreen(false);
            return true;
        }

        return false;
    }
    private bool IsGameWinForPlayer() => enemy.HealthManager.GetCurrentHealth() <= 0 || enemy.DeckManager.GetDeckCount() == 0;
    private bool IsGameLossForPlayer() => player.HealthManager.GetCurrentHealth() <= 0 || player.DeckManager.GetDeckCount() == 0;

    public void ResetTargeting()
    {
        targetingObject.SetActive(false);
        foreach (var item in _validTargets)
        {
            EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(false, item.Key));
        }
        EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(false, new ID(OwnerEnum.Player, FieldEnum.Player, 0)));
        EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(false, new ID(OwnerEnum.Opponent, FieldEnum.Player, 0)));
        _validTargets.Clear();
        BattleVars.Shared.AbilityCardOrigin = null;
        BattleVars.Shared.AbilityIDOrigin = null;
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
        var cardCount = 0;
        cardCount += Instance.player.playerPermanentManager.GetAllValidCards().FindAll(x => cardIds.Contains(x.iD)).Count;
        cardCount += Instance.enemy.playerPermanentManager.GetAllValidCards().FindAll(x => cardIds.Contains(x.iD)).Count;
        return cardCount;
    }

    private void CardTapped(CardTappedEvent cardTappedEvent)
    {
        if (!BattleVars.Shared.IsPlayerTurn) return;

        if (_validTargets.Count > 0)
        {
            HandleValidTargets(cardTappedEvent);
            return;
        }

        if (HandleOpponentCardTapped(cardTappedEvent)) return;

        if (HandleDiscard(cardTappedEvent)) return;

        if (PlayerPrefs.GetInt("QuickPlay") == 0)
        {
            player.QuickPlay(cardTappedEvent.TappedId, cardTappedEvent.TappedCard);
            return;
        }

        EventBus<SetupCardDisplayEvent>.Raise(new SetupCardDisplayEvent(cardTappedEvent.TappedId, cardTappedEvent.TappedCard, player.IsCardPlayable(cardTappedEvent.TappedCard)));
    }

    private void HandleValidTargets(CardTappedEvent cardTappedEvent)
    {
        if (BattleVars.Shared.IsSelectingTarget && _validTargets.TryGetValue(cardTappedEvent.TappedId, out var target))
        {
            player.ActivateAbility(cardTappedEvent.TappedId, target);
        }
        else
        {
            ResetTargeting();
        }
    }

    private bool HandleOpponentCardTapped(CardTappedEvent cardTappedEvent)
    {
        if (!cardTappedEvent.TappedId.owner.Equals(OwnerEnum.Opponent)) return false;
        if (cardTappedEvent.TappedId.field.Equals(FieldEnum.Hand)) return true; 

        if (enemy.playerCounters.invisibility > 0 && !enemy.cloakIndex.Contains(cardTappedEvent.TappedId)) return true;
        EventBus<SetupCardDisplayEvent>.Raise(new SetupCardDisplayEvent(cardTappedEvent.TappedId, cardTappedEvent.TappedCard, false));
        return true;

    }

    private bool HandleDiscard(CardTappedEvent cardTappedEvent)
    {
        if (!BattleVars.Shared.HasToDiscard || !cardTappedEvent.TappedId.field.Equals(FieldEnum.Hand)) return false;
        player.DiscardCard(cardTappedEvent.TappedId, cardTappedEvent.TappedCard);
        BattleVars.Shared.HasToDiscard = false;
        discardText.gameObject.SetActive(false);
        EndTurn();
        return true;

    }

    public void SetGameOver(bool isGameOver)
    {
        gameOverVisual.isGameOver = isGameOver;
    }
}
