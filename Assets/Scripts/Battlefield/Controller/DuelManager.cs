using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    public static List<CardObject> opponentShuffledDeck;
    public static void SetOpponentDeck(List<CardObject> opponentShuffledDeck)
    {
        Debug.Log("Reveiced Opponent Deck");
        DuelManager.opponentShuffledDeck = opponentShuffledDeck;
    }

    public static void ReceivePvpAction(PvP_Action action)
    {
        pvp_PlayerController.GetPvpAction(action);
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
    private static List<ID> validTargets = new List<ID>();

    private static Pvp_PlayerController pvp_PlayerController;
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
            player.ClearFloodedArea(new List<int> { 11, 12, 13, 14, 15, 16 });
            enemy.ClearFloodedArea(new List<int> { 11, 12, 13, 14, 15, 16 });
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

    public static void GenerateQuanta(Element element)
    {
        if (BattleVars.shared.isPlayerTurn)
        {
            enemy.GenerateQuantaLogic(element, 1);
            return;
        }
        player.GenerateQuantaLogic(element, 1);
    }

    public static void UpdateEclipseNightCounter(bool wasAdded, bool isEclipse)
    {
        enemy.UpdateEclipseNight(wasAdded, isEclipse);
        player.UpdateEclipseNight(wasAdded, isEclipse);
    }

    public static void StealQuanta(Element element, int amount)
    {
        if (BattleVars.shared.isPlayerTurn)
        {
            player.SpendQuantaLogic(element, amount);
            return;
        }
        enemy.SpendQuantaLogic(element, amount);
    }

    public static int GetAllOpponentQuanta(Element element)
    {
        if (BattleVars.shared.isPlayerTurn)
        {
            return enemy.GetAllQuantaOfElement(element);
        }
        return player.GetAllQuantaOfElement(element);
    }
    public static bool allPlayersSetup;
    public static bool canSetupOpDeck;

    public void FlipCoinWithResult(int flipCounts, bool willStart)
    {
        coinFlip.FlipCoinWithResult(flipCounts, willStart);
    }

    private void Start()
    {
        floodImagePlayerStatic = floodImagePlayer; 
        floodImageEnemyStatic = floodImageEnemy;
        targetingObjectStatic = targetingObject;
        BattleVars.shared.elementalMastery = false;
        GameOverVisual.isGameOver = false;
        Command.CommandQueue.Clear();
        Command.playingQueue = false;
        BattleVars.shared.gameStartInTicks = DateTime.Now.Ticks;
        player = playerObject.GetComponent<PlayerManager>();
        enemy = enemyObject.GetComponent<PlayerManager>();
        StartCoroutine(SetupManagers());
        discardTextStatic = discardText;
        endTurnButtonStatic = endTurnButton;
        if (!BattleVars.shared.isPvp)
        {
            coinFlip.FlipCoinRandom();
            botContoller = new EnemyController(enemy);
        }
        else
        {
            FlipCoinWithResult(BattleVars.shared.coinFlip, BattleVars.shared.willStart);
            BattleVars.shared.isPlayerTurn = BattleVars.shared.willStart;
            endTurnButtonStatic.interactable = BattleVars.shared.willStart;
            pvp_PlayerController = new Pvp_PlayerController(enemy);
            //enemyName.text = Game_PvpHubConnection.shared.GetOpponentName();
        }
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
            if (BattleVars.shared.isPvp)
            {
                enemy.StartTurn();
            }
            else
            {
                botContoller.StartTurn();
            }
        }
    }

    public static void ScrambleQuanta()
    {
        if (BattleVars.shared.isPlayerTurn)
        {
            player.ScrambleQuanta();
            return;
        }
        enemy.ScrambleQuanta();
    }

    public static Element GetMarkElement()
    {
        return BattleVars.shared.isPlayerTurn ? BattleVars.shared.enemyAiData.mark : PlayerData.shared.markElement;
    }

    public static Element GetOtherMarkElement()
    {
        return BattleVars.shared.isPlayerTurn ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
    }

    private IEnumerator SetupManagers()
    {
        player.SetupPlayerManager(true);
        yield return new WaitForSeconds(0.5f);

        enemy.SetupPlayerManager(false);
        yield return new WaitForSeconds(0.5f);
    }

    public static void PlayerEndTurn()
    {
        endTurnButtonStatic.interactable = false;
        BattleVars.shared.isSingularity = false;
        ResetTargeting();
        EndTurn();
    }

    public static async void EndTurn()
    {
        player.UpdateHandLogic();
        if (BattleVars.shared.isPlayerTurn)
        {
            if (player.GetHandCards().Count == 8)
            {
                discardTextStatic.gameObject.SetActive(true);
                BattleVars.shared.hasToDiscard = true;
                return;
            }
        }
        BattleVars.shared.isPlayerTurn = !BattleVars.shared.isPlayerTurn;
        BattleVars.shared.turnCount++;
        if (!BattleVars.shared.isPlayerTurn)
        {
            List<QuantaObject> pvpQuantaList = player.GeneratePillarQuantaLogic();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            player.SendCreatureDamage();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            player.SendWeaponDamage();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            player.ActivateEndTurnAbilities();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            player.UpdateCounterAndEffects();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            if (BattleVars.shared.isPvp)
            {
                //Game_PvpHubConnection.shared.EndPlayerTurn(pvpQuantaList);
                enemy.StartTurn();
            }
            else
            {
                botContoller.StartTurn();
            }
        }
        else
        {
            if (BattleVars.shared.isPvp)
            {
                enemy.GeneratePillarPvPQuantaLogic();
            }
            else
            {
                enemy.GeneratePillarQuantaLogic();
            }
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            enemy.SendCreatureDamage();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            enemy.SendWeaponDamage();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            enemy.ActivateEndTurnAbilities();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            enemy.UpdateCounterAndEffects();
            if (GameOverVisual.isGameOver) { return; }
            await new WaitUntil(() => !Command.playingQueue);
            player.StartTurn();
            endTurnButtonStatic.interactable = true;
            BattleVars.shared.spaceTapped = false;
        }
    }

    public static int GetLightEmittingCreatures()
    {
        if (!BattleVars.shared.isPlayerTurn)
        {
            return enemy.GetLightEmittingCreatures();
        }
        return player.GetLightEmittingCreatures();

    }

    public static int SendCreatureDamage(Card creature)
    {
        if (!BattleVars.shared.isPlayerTurn)
        {
            return enemy.ReceiveCreatureDamage(creature);
        }
        else
        {
            return player.ReceiveCreatureDamage(creature);
        }
    }

    public static void SetupHighlight(ISpellAbility spell, IActivateAbility ability)
    {
        targetingObjectStatic.SetActive(true);
        SetupValidTargets(spell, ability);
        foreach (ID item in validTargets)
        {
            if (item.Owner.Equals(OwnerEnum.Player))
            {
                player.ShouldDisplayTarget(item, true);
                continue;
            }
            enemy.ShouldDisplayTarget(item, true);
        }
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
        validTargets.Clear();
        BattleVars.shared.originId = null;
        BattleVars.shared.spellOnStandBy = null;
        BattleVars.shared.abilityOnStandBy = null;
        BattleVars.shared.isSelectingTarget = false;
    }

    public static void CheckValidTarget(ID tappedID)
    {
        if (validTargets.Contains(tappedID))
        {
            if (BattleVars.shared.spellOnStandBy != null)
            {
                if (tappedID.Equals(FieldEnum.Player))
                {
                    ActionManager.AddCardPlayedWithTargetAction(true, player.GetCard(BattleVars.shared.originId), null, tappedID.Owner.Equals(OwnerEnum.Player));
                }
                else
                {
                    ActionManager.AddCardPlayedWithTargetAction(true, player.GetCard(BattleVars.shared.originId), GetIDOwner(tappedID).GetCard(tappedID), tappedID.Owner.Equals(OwnerEnum.Player));
                }
                BattleVars.shared.spellOnStandBy.ActivateAbility(tappedID);
                player.PlayCardFromHandLogic(BattleVars.shared.originId);
                if (BattleVars.shared.isPvp)
                {
                    //Game_PvpHubConnection.shared.SendPvpAction(new PvP_Action(ActionType.PlaySpell, BattleVars.shared.originId, tappedID));
                }
                ResetTargeting();
            }
            else if (BattleVars.shared.abilityOnStandBy != null)
            {
                if (tappedID.Equals(FieldEnum.Player))
                {
                    ActionManager.AddActivateAbilityWithTargetAction(true, player.GetCard(BattleVars.shared.originId), null, tappedID.Owner.Equals(OwnerEnum.Player));
                }
                else
                {
                    ActionManager.AddActivateAbilityWithTargetAction(true, player.GetCard(BattleVars.shared.originId), GetIDOwner(tappedID).GetCard(tappedID), tappedID.Owner.Equals(OwnerEnum.Player));
                }
                BattleVars.shared.abilityOnStandBy.ActivateAbility(tappedID);
                player.SpendQuantaLogic(BattleVars.shared.abilityOnStandBy.AbilityElement, BattleVars.shared.abilityOnStandBy.AbilityCost);
                if (BattleVars.shared.isPvp)
                {
                    //Game_PvpHubConnection.shared.SendPvpAction(new PvP_Action(ActionType.PlaySpell, BattleVars.shared.originId, tappedID));
                }
                ResetTargeting();
            }
            return;
        }
        Debug.Log("Invalid Target");
    }

    public static Card GetOpponentCard(ID iD)
    {
        return enemy.GetCard(iD);
    }

    public static void SetupValidTargets(ISpellAbility spell, IActivateAbility ability)
    {
        List<ID> lists = GetAllIdsInPlay();
        validTargets = new List<ID>();
        if (spell != null)
        {
            foreach (ID item in lists)
            {
                bool isImmaterial = false;

                //if (GetIDOwner(item).GetCard(item) == null)
                //{
                //    continue;
                //}

                if (!item.Field.Equals(FieldEnum.Player))
                {
                    isImmaterial = GetIDOwner(item).GetCard(item).cardPassives.isImmaterial;
                }
                if (spell.IsValidTarget(item))
                {
                    if(spell is SpellWisdomShard)
                    {
                        validTargets.Add(item);
                    }
                    else if(!isImmaterial)
                    {
                        validTargets.Add(item);
                    }
                }
            }
            return;
        }

        if (ability != null)
        {
            foreach (ID item in lists)
            {
                bool isImmaterial = false;
                if (!item.Field.Equals(FieldEnum.Player))
                {
                    isImmaterial = GetIDOwner(item).GetCard(item).cardPassives.isImmaterial;
                }
                if (ability.IsValidTarget(item) && !isImmaterial)
                {
                    validTargets.Add(item);
                }
            }
            return;
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

    internal static void ActivateDeathTriggers()
    {
        player.ActivateDeathTriggers();
        enemy.ActivateDeathTriggers();
    }
}
