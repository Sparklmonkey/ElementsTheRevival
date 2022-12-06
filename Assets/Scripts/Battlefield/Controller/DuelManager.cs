using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    public static List<CardObject> opponentShuffledDeck;
    public static int opponentCardsTurn, playerCardsTurn;

    public static bool IsSundialInPlay()
    {
        foreach (Card card in player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5rp" || card.iD == "7q9") { return true; }
        }
        foreach (Card card in enemy.playerPermanentManager.GetAllCards())
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
            return enemy.GetPossibleDamage();
        }
        return player.GetPossibleDamage();
    }

    public static PlayerManager player;
    public static PlayerManager enemy;
    public static TMPro.TextMeshProUGUI discardTextStatic;
    public static Button endTurnButtonStatic;

    public Button endTurnButton;
    public TMPro.TextMeshProUGUI enemyName;
    public GameObject playerObject;
    public GameObject enemyObject;
    public TMPro.TextMeshProUGUI discardText;
    public static List<ID> validTargets = new List<ID>();

    private static EnemyController botContoller;

    public static int floodCount = 0;

    [SerializeField]
    private GameObject floodImagePlayer, floodImageEnemy;
    private static GameObject floodImagePlayerStatic, floodImageEnemyStatic;

    [SerializeField]
    private CoinFlip coinFlip;
    public GameObject targetingObject;
    public static GameObject targetingObjectStatic;

    public static void AddFloodCount()
    {
        floodCount++;
        if(floodCount == 1)
        {
            floodImageEnemyStatic.SetActive(true);
            floodImagePlayerStatic.SetActive(true);
        }
    }

    public static void RemoveFloodCount()
    {
        floodCount--;
        if (floodCount == 0)
        {
            floodImageEnemyStatic.SetActive(false);
            floodImagePlayerStatic.SetActive(false);
        }
    }

    public static PlayerManager GetIDOwner(ID iD)
    {
        return iD.Owner.Equals(OwnerEnum.Player) ? player : enemy;
    }

    public static PlayerManager GetNotIDOwner(ID iD)
    {
        return iD.Owner.Equals(OwnerEnum.Player) ? enemy : player;
    }

    public static bool allPlayersSetup;
    public static bool canSetupOpDeck;

    private void Awake()
    {
        floodImagePlayerStatic = floodImagePlayer;
        floodImageEnemyStatic = floodImageEnemy;
        targetingObjectStatic = targetingObject;
        BattleVars.shared.elementalMastery = false;
        GameOverVisual.isGameOver = false;
        Command.CommandQueue.Clear();
        ActionManager.actionList = new List<ElementAction>();
        Command.playingQueue = false;
        player = playerObject.GetComponent<PlayerManager>();
        enemy = enemyObject.GetComponent<PlayerManager>();
        discardTextStatic = discardText;
        endTurnButtonStatic = endTurnButton;
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
            player.StartTurn();
            MessageManager.shared.DisplayMessage("Your turn has started!");
        }
        else
        {
            enemy.StartCoroutine(botContoller.StartTurn());
            
        }
    }

    private IEnumerator SetupManagers()
    {
         yield return coinFlip.StartCoroutine(coinFlip.WaitPlease(0.0001f, 1.0f));
        yield return enemy.StartCoroutine(enemy.SetupPlayerManager(false));
        yield return player.StartCoroutine(player.SetupPlayerManager(true));

        BattleVars.shared.isPlayerTurn = coinFlip.playerStarts;
        if (coinFlip.playerStarts)
        {
            player.StartTurn();
            MessageManager.shared.DisplayMessage("Your turn has started!");
        }
        else
        {
            enemy.StartCoroutine(botContoller.StartTurn());

        }
    }

    public static void PlayerEndTurn()
    {
        endTurnButtonStatic.interactable = false;
        BattleVars.shared.isSingularity = 0;
        ResetTargeting();
        player.StartCoroutine(EndTurn());
    }

    public static IEnumerator EndTurn()
    {
        player.HideAllPlayableGlow();
        if (BattleVars.shared.isPlayerTurn)
        {
            if (player.GetHandCards().Count == 8)
            {
                discardTextStatic.gameObject.SetActive(true);
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
            enemy.StartCoroutine(botContoller.StartTurn());
            BattleVars.shared.turnCount++;
        }
        else
        {
            yield return enemy.StartCoroutine(enemy.GeneratePillarQuantaLogic());
            yield return enemy.StartCoroutine(enemy.UpdateCounterAndEffects());
            if (GameOverVisual.isGameOver) { yield break; }
            playerCardsTurn = 0;
            player.StartTurn();
            endTurnButtonStatic.interactable = true;
            BattleVars.shared.spaceTapped = false;
        }
        BattleVars.shared.isPlayerTurn = !BattleVars.shared.isPlayerTurn;
    }

    public static void ResetTargeting()
    {
        targetingObjectStatic.SetActive(false);
        foreach (ID item in validTargets)
        {
            if (item.Owner.Equals(OwnerEnum.Player))
            {
                player.ShouldDisplayTarget(item, false);
                continue;
            }
            enemy.ShouldDisplayTarget(item, false);
        }
        enemy.playerDisplayer.ShouldShowTarget(false);
        player.playerDisplayer.ShouldShowTarget(false);
        validTargets.Clear();
        BattleVars.shared.originId = null;
        BattleVars.shared.cardOnStandBy = null;
        BattleVars.shared.isSelectingTarget = false;
    }

    public static Card GetCard(ID iD)
    {
        if (iD.Owner.Equals(OwnerEnum.Player))
        {
            return player.GetCard(iD);
        }
        return enemy.GetCard(iD);
    }

    public static void SetupValidTargets(string skill)
    {
        List<ID> lists = GetAllIdsInPlay();
        validTargets = new List<ID>();
        foreach (ID item in lists)
        {
            bool isImmaterial = false;
            bool isInvisible = false;
            bool isBurrowed = false;

            if (!item.Field.Equals(FieldEnum.Player))
            {
                isImmaterial = GetIDOwner(item).GetCard(item).innate.Contains("immaterial");
                //isInvisible = GetIDOwner(item).GetCard(item).cardCounters.invisibility > 0;
                isBurrowed = GetIDOwner(item).GetCard(item).innate.Contains("burrowed");
            }

            bool isOwnerInvisible = GetIDOwner(item).playerCounters.invisibility > 0 && !GetIDOwner(item).cloakIndex.Contains(item);
            switch (skill)
            { 
                case "scarab":
                    break;
                case "mutation":
                    break;
                case "improve":
                    break;
                case "dead / alive":
                    break;
                case "lycanthropy":
                    break;
                case "antimatter":
                    break;
                case "web":
                    break;
                case "evolve":
                    break;
                case "endow":
                    break;
                case "liquid shadow":
                    break;
                case "accretion":
                    break;
                case "gravity pull":
                    break;
                case "growth":
                    break;
                case "ablaze":
                    break;
                case "devour":
                    break;
                case "black hole":
                    break;
                case "luciferin":
                    break;
                case "immortality":
                    break;
                case "deja vu":
                    break;
                case "hatch":
                    break;
                case "unstable gas":
                    break;
                case "dive":
                    break;
                case "infect":
                    break;
                case "plague":
                    break;
                case "poison":
                    break;
                case "stone form":
                    break;
                case "congeal":
                    break;
                case "berserk":
                    break;
                case "precognition":
                    break;
                case "lobotomize":
                    break;
                case "rage":
                    break;
                case "photosynthesis":
                    break;
                case "paradox":
                    break;
                case "freeze":
                    break;
                case "steam":
                    break;
                case "queen":
                    break;
                case "nymph":
                    break;
                case "divineshield":
                    break;
                case "infection":
                    break;
                case "heal":
                    break;
                case "burrow":
                    break;
                case "petrify":
                    break;
                case "aflatoxin":
                    break;
                case "guard":
                    break;
                case "rebirth":
                    break;
                default:
                    break;
            }

        }

    }

    private static List<ID> GetAllIdsInPlay()
    {
        List<ID> listToReturn = new List<ID>();
        listToReturn.AddRange(player.GetAllIds());
        listToReturn.AddRange(enemy.GetAllIds());
        return listToReturn;
    }

    public static int GetAllQuantaOfElement(Element element)
    {
        if (BattleVars.shared.isPlayerTurn)
        {
            return player.GetAllQuantaOfElement(element);
        }
        return enemy.GetAllQuantaOfElement(element);
    }

    public static void ApplyCounter(CounterEnum freeze, int amount, ID target)
    {
        if (target.Owner.Equals(OwnerEnum.Opponent))
        {
            enemy.ModifyCreatureLogic(target, freeze, amount);
        }
        else
        {
            player.ModifyCreatureLogic(target, freeze, amount);
        }
    }

    public static PlayerManager GetOtherPlayer() => BattleVars.shared.isPlayerTurn ? enemy : player;

    public static List<ID> GetAllValidTargets() => validTargets;

    public static void RevealOpponentsHand()
    {
        if (!BattleVars.shared.isPlayerTurn) { return; }
        enemy.DisplayHand();
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

    internal static bool IsFloodInPlay()
    {
        foreach (Card card in player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { return true; }
        }
        foreach (Card card in enemy.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { return true; }
        }
        return false;
    }

    internal static bool IsNightfallInPlay()
    {
        int count = 0;
        foreach (Card card in player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { count++; }
        }
        foreach (Card card in enemy.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5ih" || card.iD == "7h1") { count++; }
        }
        return count > 1;
    }

    internal static bool isEclipseInPlay()
    {
        int count = 0;
        foreach (Card card in player.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5uq" || card.iD == "7ta") { count++; }
        }
        foreach (Card card in enemy.playerPermanentManager.GetAllCards())
        {
            if (card.iD == "5uq" || card.iD == "7ta") { count++; }
        }
        return count > 1;
    }
}
