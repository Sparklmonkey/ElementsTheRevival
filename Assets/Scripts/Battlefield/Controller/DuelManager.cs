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

    private DeathTriggerHandler _deathTriggerHandler;

    public void RegisterDeathTrigger(DeathTriggerHandler deathTrigger, bool isRegister)
    {
        if (isRegister)
        {
            _deathTriggerHandler += deathTrigger;
        }
        else
        {
            _deathTriggerHandler -= deathTrigger;
        }
    }

    public IEnumerator ActivateDeathTriggers()
    {
        yield return _deathTriggerHandler.Occured(this, new());
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
        foreach (Card card in Instance.player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5rp" || card.iD == "7q9") { return true; }
        }
        foreach (Card card in Instance.enemy.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5rp" || card.iD == "7q9") { return true; }
        }
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
            yield return player.StartCoroutine(player.GeneratePillarQuantaLogic());

            yield return player.StartCoroutine(player.UpdateCounterAndEffects());
            if (GameOverVisual.isGameOver) { yield break; }
            opponentCardsTurn = 0;
            Instance.enemy.StartCoroutine(botContoller.StartTurn());
            BattleVars.shared.turnCount++;
        }
        else
        {
            yield return enemy.StartCoroutine(enemy.GeneratePillarQuantaLogic());
            yield return enemy.StartCoroutine(enemy.UpdateCounterAndEffects());
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
            if (item.id.Owner.Equals(OwnerEnum.Player))
            {
                Instance.player.ShouldDisplayTarget(item.id, false);
                continue;
            }
            Instance.enemy.ShouldDisplayTarget(item.id, false);
        }
        Instance.enemy.playerDisplayer.ShouldShowTarget(false);
        Instance.player.playerDisplayer.ShouldShowTarget(false);
        validTargets.Clear();
        BattleVars.shared.abilityOrigin = null;
        BattleVars.shared.isSelectingTarget = false;
    }

    public static Card GetCard(ID iD)
    {
        if (iD.Owner.Equals(OwnerEnum.Player))
        {
            return Instance.player.GetCard(iD);
        }
        return Instance.enemy.GetCard(iD);
    }

    public static int GetAllQuantaOfElement(Element element)
    {
        if (BattleVars.shared.isPlayerTurn)
        {
            return Instance.player.GetAllQuantaOfElement(element);
        }
        return Instance.enemy.GetAllQuantaOfElement(element);
    }

    public static void ApplyCounter(CounterEnum freeze, int amount, ID target)
    {
        if (target.Owner.Equals(OwnerEnum.Opponent))
        {
            Instance.enemy.ModifyCreatureLogic(target, freeze, amount);
        }
        else
        {
            Instance.player.ModifyCreatureLogic(target, freeze, amount);
        }
    }

    public static PlayerManager GetOtherPlayer() => BattleVars.shared.isPlayerTurn ? Instance.enemy : Instance.player;

    public static List<IDCardPair> GetAllValidTargets() => validTargets;

    public static void RevealOpponentsHand()
    {
        if (!BattleVars.shared.isPlayerTurn) { return; }
        Instance.enemy.DisplayHand();
    }

    public static List<List<int>> AdrenalineDamageList = new List<List<int>> { new List<int> { 1, 1, 1, 1 }
                                                                            , new List<int> { 2, 2, 2, 2 }
                                                                            , new List<int> { 3, 3, 3, 3 }
                                                                            , new List<int> { 4, 3, 2 }
                                                                            , new List<int> { 5, 4, 2}
                                                                            , new List<int> {6, 4, 2 }
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
        foreach (Card card in Instance.player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { return true; }
        }
        foreach (Card card in Instance.enemy.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { return true; }
        }
        return false;
    }

    public static bool IsNightfallInPlay()
    {
        int count = 0;
        foreach (Card card in Instance.player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { count++; }
        }
        foreach (Card card in Instance.enemy.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { count++; }
        }
        return count > 1;
    }

    public static bool IsEclipseInPlay()
    {
        int count = 0;
        foreach (Card card in Instance.player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5uq" || card.iD == "7ta") { count++; }
        }
        foreach (Card card in Instance.enemy.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5uq" || card.iD == "7ta") { count++; }
        }
        return count > 1;
    }

    public void IdCardTapped(IDCardPair idCard)
    {
        if (!BattleVars.shared.isPlayerTurn) { return; }
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
