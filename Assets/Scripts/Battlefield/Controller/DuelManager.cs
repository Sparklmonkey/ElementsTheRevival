using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Helpers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public delegate bool IsGameOver();
public class DuelManager : MonoBehaviour
{
    public static DuelManager Instance { get; private set; }
    
    [SerializeField]
    private GameOverVisual gameOverVisual;
    [SerializeField]
    private MessageManager messageManager;
    private EventBinding<CardTappedEvent> _cardTappedBinding;
    private EventBinding<AddTargetToListEvent> _addTargetToListBinding;
    
    private void OnDisable() {
        EventBus<CardTappedEvent>.Unregister(_cardTappedBinding);
        EventBus<AddTargetToListEvent>.Unregister(_addTargetToListBinding);
    }

    private void OnEnable()
    {
        _cardTappedBinding = new EventBinding<CardTappedEvent>(CardTapped);
        EventBus<CardTappedEvent>.Register(_cardTappedBinding);
        _addTargetToListBinding = new EventBinding<AddTargetToListEvent>(AddTargetToList);
        EventBus<AddTargetToListEvent>.Register(_addTargetToListBinding);
    }
    public void UpdateNightFallEclipse(bool isAdded, bool isNightFall)
    {
        player.CheckEclipseNightfall(isAdded, isNightFall);
        enemy.CheckEclipseNightfall(isAdded, isNightFall);
    }

    public static List<CardObject> OpponentShuffledDeck;
    public static int OpponentCardsTurn, PlayerCardsTurn;

    public void StopAllRunningRoutines()
    {
        player.StopAllCoroutines();
        enemy.StopAllCoroutines();
        aiController.StopAllCoroutines();
    }
    public int GetPossibleDamage(bool isPlayer)
    {
        var creatureDamage = isPlayer ? enemy.GetPossibleDamage() : player.GetPossibleDamage();
        var poisonDamage = isPlayer ? player.playerCounters.poison : enemy.playerCounters.poison;
        return creatureDamage + poisonDamage;
    }

    public PlayerManager player;
    public PlayerManager enemy;

    public Button endTurnButton;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI discardText;
    public Dictionary<ID, Card> ValidTargets { get; private set; }
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
        return iD.IsOwnedBy(OwnerEnum.Player) ? player : enemy;
    }

    public PlayerManager GetNotIDOwner(ID iD)
    {
        return iD.IsOwnedBy(OwnerEnum.Player) ? enemy : player;
    }

    public bool allPlayersSetup;

    private void Awake()
    {
        Instance = this;
        BattleVars.Shared.ElementalMastery = false;
        gameOverVisual.isGameOver = false;
        endTurnButton.interactable = false;
    }

    private void Start()
    {
        ValidTargets = new();
        FloodCount = 0;
        BattleVars.Shared.GameStartInTicks = DateTime.Now;
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
            player.StartCoroutine(player.EndTurnRoutine(ShouldEndGame));
            OpponentCardsTurn = 0;
        }
        else
        {
            PlayerCardsTurn = 0;
            player.StartTurn();
            endTurnButton.interactable = true;
            BattleVars.Shared.ChangePlayerTurn();
        }
    }

    public bool ShouldEndGame()
    {
        if (IsGameWinForPlayer())
        {
            EventBus<GameEndEvent>.Raise(new GameEndEvent(OwnerEnum.Player));
            return true;
        }

        if (IsGameLossForPlayer())
        {
            EventBus<GameEndEvent>.Raise(new GameEndEvent(OwnerEnum.Opponent));
            return true;
        }

        return false;
    }
    private bool IsGameWinForPlayer() => enemy.HealthManager.GetCurrentHealth() <= 0 || enemy.DeckManager.GetDeckCount() == 0;
    private bool IsGameLossForPlayer() => player.HealthManager.GetCurrentHealth() <= 0 || player.DeckManager.GetDeckCount() == 0;

    public void ResetTargeting()
    {
        targetingObject.SetActive(false);
        EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(null));
        ValidTargets.Clear();
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
        if(player.playerPermanentManager.GetAllValidCards().FirstOrDefault(x => cardIds.Contains(x.Id)) is not null)
        {
            cardCount += player.playerPermanentManager.GetAllValidCards().FindAll(x => cardIds.Contains(x.Id)).Count;
        }
        if(enemy.playerPermanentManager.GetAllValidCards().FirstOrDefault(x => cardIds.Contains(x.Id)) is not null)
        {
            cardCount += enemy.playerPermanentManager.GetAllValidCards().FindAll(x => cardIds.Contains(x.Id)).Count;
        }
        return cardCount;
    }

    private void AddTargetToList(AddTargetToListEvent addTargetToListEvent)
    {
        if (ValidTargets.ContainsKey(addTargetToListEvent.TargetId)) return;
        ValidTargets.Add(addTargetToListEvent.TargetId, addTargetToListEvent.TargetCard);
    }

    private void CardTapped(CardTappedEvent cardTappedEvent)
    {
        if (!BattleVars.Shared.IsPlayerTurn) return;
        if (!BattleVars.Shared.CanInteract) return;

        if (ValidTargets.Count > 0)
        {
            HandleValidTargets(cardTappedEvent);
            return;
        }

        if (HandleOpponentCardTapped(cardTappedEvent)) return;

        if (HandleDiscard(cardTappedEvent)) return;

        var isPlayable = player.IsCardPlayable(cardTappedEvent.TappedCard) && player.playerCounters.silence <= 0;
        var isAbilityUsable = player.IsAbilityUsable(cardTappedEvent.TappedCard);
        if (PlayerPrefs.GetInt("QuickPlay") == 0)
        {
            //Check if card is in hand and is NOT a spell
             if (cardTappedEvent.TappedId.IsFromHand() 
                && !cardTappedEvent.TappedCard.Type.Equals(CardType.Spell)
                && isPlayable)
            {
                EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(cardTappedEvent.TappedCard, cardTappedEvent.TappedId));
                return;
            }
            
             //Check if card is in hand and IS a spell
             if (cardTappedEvent.TappedCard.Type.Equals(CardType.Spell) && isPlayable)
             {
                 BattleVars.Shared.AbilityIDOrigin = cardTappedEvent.TappedId;
                 BattleVars.Shared.AbilityCardOrigin = cardTappedEvent.TappedCard;
                 if (!SkillManager.Instance.ShouldAskForTarget(cardTappedEvent.TappedCard))
                 {
                     EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(cardTappedEvent.TappedId, cardTappedEvent.TappedCard));
                 }
                 else
                 {
                     BattleVars.Shared.IsSelectingTarget = true;
                     EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(Instance.player, cardTappedEvent.TappedCard));
                 }
                 return;
             }
            
            if (isAbilityUsable && !cardTappedEvent.TappedId.IsFromHand())
            {
                BattleVars.Shared.AbilityIDOrigin = cardTappedEvent.TappedId;
                BattleVars.Shared.AbilityCardOrigin = cardTappedEvent.TappedCard;
                if (!SkillManager.Instance.ShouldAskForTarget(cardTappedEvent.TappedCard))
                {
                    EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(cardTappedEvent.TappedId, cardTappedEvent.TappedCard));
                }
                else
                {
                    BattleVars.Shared.IsSelectingTarget = true;
                    EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(Instance.player, cardTappedEvent.TappedCard));
                }

                return;
            }
        }

        EventBus<SetupCardDisplayEvent>.Raise(new SetupCardDisplayEvent(cardTappedEvent.TappedId, cardTappedEvent.TappedCard, isPlayable, isAbilityUsable));
    }

    private void HandleValidTargets(CardTappedEvent cardTappedEvent)
    {
        if (BattleVars.Shared.IsSelectingTarget && ValidTargets.ContainsKey(cardTappedEvent.TappedId))
        {
            ValidTargets.TryGetValue(cardTappedEvent.TappedId, out var target);
            EventBus<ActivateSpellOrAbilityEvent>.Raise(new ActivateSpellOrAbilityEvent(cardTappedEvent.TappedId, target));
        }
        else
        {
            ResetTargeting();
        }
    }

    private bool HandleOpponentCardTapped(CardTappedEvent cardTappedEvent)
    {
        if (!cardTappedEvent.TappedId.IsOwnedBy(OwnerEnum.Opponent)) return false;
        if (cardTappedEvent.TappedId.field.Equals(FieldEnum.Hand)) return true; 

        if (enemy.IsPlayerInvisible()) return true;
        EventBus<SetupCardDisplayEvent>.Raise(new SetupCardDisplayEvent(cardTappedEvent.TappedId, cardTappedEvent.TappedCard, false, false));
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
