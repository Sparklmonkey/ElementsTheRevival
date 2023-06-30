using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventUtility;

public class DuelManager : MonoBehaviour
{
    private static DuelManager _instance;
    public static DuelManager Instance { get { return _instance; } }

    public void UpdateNightFallEclipse(bool isAdded, string skill)
    {
        player.CheckEclipseNightfall(isAdded, skill);
        enemy.CheckEclipseNightfall(isAdded, skill);
    }

    public static List<CardObject> opponentShuffledDeck;
    public static int opponentCardsTurn, playerCardsTurn;


    public void ActivateDeathTriggers()
    {
        player.ActivateDeathTriggers();
        enemy.ActivateDeathTriggers();
    }

    public static void SetupHighlights(List<IDCardPair> targets)
    {
        validTargets = new();
        foreach (var target in targets)
        {
            validTargets.Add(target);
            if (target.id.Owner == OwnerEnum.Player)
            {
                Instance.player.ShowTargetHighlight(target);
                continue;
            }
            Instance.enemy.ShowTargetHighlight(target);
        }

    }
    public static bool IsSundialInPlay()
    {
        if (Instance.player.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "5rp" || x.card.iD == "7q9").Count > 0) { return true; }
        if (Instance.enemy.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "5rp" || x.card.iD == "7q9").Count > 0) { return true; }
        return false;
    }
    public static void SetOpponentDeck(List<CardObject> opponentShuffledDeck)
    {
        Debug.Log("Reveiced Opponent Deck");
        DuelManager.opponentShuffledDeck = opponentShuffledDeck;
    }

    public static int GetPossibleDamage(bool isPlayer)
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
    public static List<IDCardPair> validTargets = new();

    private static EnemyController botContoller;

    public static int floodCount = 0;

    [SerializeField]
    private GameObject floodImagePlayer, floodImageEnemy;

    [SerializeField]
    private CoinFlip coinFlip;
    public GameObject targetingObject;

    public void AddFloodCount(int amount)
    {
        floodCount += amount;
        floodImageEnemy.SetActive(floodCount >= 1);
        floodImagePlayer.SetActive(floodCount >= 1);
    }

    public static PlayerManager GetIDOwner(ID iD)
    {
        return iD.Owner.Equals(OwnerEnum.Player) ? Instance.player : Instance.enemy;
    }

    public static PlayerManager GetNotIDOwner(ID iD)
    {
        return iD.Owner.Equals(OwnerEnum.Player) ? Instance.enemy : Instance.player;
    }

    public static bool allPlayersSetup;
    public static bool canSetupOpDeck;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        BattleVars.shared.elementalMastery = false;
        GameOverVisual.isGameOver = false;
        ActionManager.actionList = new List<ElementAction>();
    }

    private void Start()
    {
        floodCount = 0;
        BattleVars.shared.gameStartInTicks = DateTime.Now.Ticks;
        botContoller = new EnemyController(enemy);
        enemyName.text = BattleVars.shared.enemyAiData.opponentName;
        StartCoroutine(SetupManagers());
    }

    public static void GameStart(bool isPlayerStart)
    {
        while (!allPlayersSetup) { }
        BattleVars.shared.isPlayerTurn = isPlayerStart;
        if (isPlayerStart)
        {
            Instance.player.StartTurn();
            MessageManager.shared.DisplayMessage("Your turn has started!");
        }
        else
        {
            Instance.enemy.StartCoroutine(botContoller.StartTurn());
            
        }
    }

    private IEnumerator SetupManagers()
    {
         yield return coinFlip.StartCoroutine(coinFlip.WaitPlease(0.0001f, 1.0f));
        yield return Instance.enemy.StartCoroutine(enemy.SetupPlayerManager(false));
        yield return Instance.player.StartCoroutine(player.SetupPlayerManager(true));

        BattleVars.shared.isPlayerTurn = coinFlip.playerStarts;
        if (coinFlip.playerStarts)
        {
            Instance.player.StartTurn();
            MessageManager.shared.DisplayMessage("Your turn has started!");
        }
        else
        {
            Instance.enemy.StartCoroutine(botContoller.StartTurn());

        }
    }

    public void PlayerEndTurn()
    {
        endTurnButton.interactable = false;
        BattleVars.shared.isSingularity = 0;
        ResetTargeting();
        Instance.player.StartCoroutine(EndTurn());
    }

    public IEnumerator EndTurn()
    {
        Instance.player.HideAllPlayableGlow();
        if (BattleVars.shared.isPlayerTurn)
        {
            if (Instance.player.GetHandCards().Count == 8)
            {
                discardText.gameObject.SetActive(true);
                BattleVars.shared.hasToDiscard = true;
                yield break;
            }
        }

        if (BattleVars.shared.isPlayerTurn)
        {
            player.EndTurnRoutine();

            player.UpdateCounterAndEffects();
            if (GameOverVisual.isGameOver) { yield break; }
            opponentCardsTurn = 0;
            Instance.enemy.StartCoroutine(botContoller.StartTurn());
            BattleVars.shared.turnCount++;
        }
        else
        {
            enemy.EndTurnRoutine();
            enemy.UpdateCounterAndEffects();
            if (GameOverVisual.isGameOver) { yield break; }
            playerCardsTurn = 0;
            player.StartTurn();
            endTurnButton.interactable = true;
            BattleVars.shared.spaceTapped = false;
        }
        BattleVars.shared.isPlayerTurn = !BattleVars.shared.isPlayerTurn;
    }

    public void ResetTargeting()
    {
        targetingObject.SetActive(false);
        foreach (IDCardPair item in validTargets)
        {
            item.IsTargeted(false);
        }
        Instance.enemy.playerDisplayer.ShouldShowTarget(false);
        Instance.player.playerDisplayer.ShouldShowTarget(false);
        validTargets.Clear();
        BattleVars.shared.abilityOrigin = null;
        BattleVars.shared.isSelectingTarget = false;
    }

    public static int GetAllQuantaOfElement(Element element)
    {
        if (BattleVars.shared.isPlayerTurn)
        {
            return Instance.player.GetAllQuantaOfElement(element);
        }
        return Instance.enemy.GetAllQuantaOfElement(element);
    }

    public static PlayerManager GetOtherPlayer() => BattleVars.shared.isPlayerTurn ? Instance.enemy : Instance.player;

    public static List<IDCardPair> GetAllValidTargets() => validTargets;

    public static void RevealOpponentsHand()
    {
        if (!BattleVars.shared.isPlayerTurn) { return; }
        Instance.enemy.DisplayHand();
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

    public static bool IsFloodInPlay()
    {
        if (Instance.player.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "5ih" || x.card.iD == "7h1").Count > 0) { return true; }
        if (Instance.enemy.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "5ih" || x.card.iD == "7h1").Count > 0) { return true; }
        return false;
    }

    public static int GetNightfallCount()
    {
        int nightfallCount = 0;
        nightfallCount += Instance.player.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "5uq").Count;
        nightfallCount += Instance.enemy.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "5uq").Count;
        return nightfallCount;
    }

    public static int GetEclipseCount()
    {
        int eclipseCount = 0;
        eclipseCount += Instance.player.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "7ta").Count;
        eclipseCount += Instance.enemy.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "7ta").Count;
        return eclipseCount;
    }

    public void IdCardTapped(IDCardPair idCard)
    {
        if (!BattleVars.shared.isPlayerTurn) { return; }

        if(validTargets.Count > 0)
        {
            if (BattleVars.shared.isSelectingTarget && validTargets.Contains(idCard))
            {
                player.ActivateAbility(idCard);
            }
        }

        if (!idCard.HasCard()) { return; }

        if (idCard.id.Owner.Equals(OwnerEnum.Opponent))
        {
            if (idCard.id.Field.Equals(FieldEnum.Hand)) { return; }

            if (enemy.playerCounters.invisibility > 0 && !enemy.cloakIndex.Contains(idCard.id)) { return; }
            player.SetupCardDisplay(idCard);
            return;
        }

        if (BattleVars.shared.hasToDiscard)
        {
            if (idCard.id.Field.Equals(FieldEnum.Hand))
            {
                player.DiscardCard(idCard);
                BattleVars.shared.hasToDiscard = false;
                discardText.gameObject.SetActive(false);
                StartCoroutine(EndTurn());
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
}
