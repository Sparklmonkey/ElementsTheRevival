using Elements.Duel.Manager;
using Elements.Duel.Visual;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public event Action<Counters> OnPlayerCounterUpdate;
    public void AddPlayerCounter(PlayerCounters counter, int amount)
    {
        switch (counter)
        {
            case PlayerCounters.Bone:
                playerCounters.bone += amount;
                if (playerCounters.bone < 0) { playerCounters.bone = 0; }

                break;
            case PlayerCounters.Invisibility:
                playerCounters.invisibility += amount;
                if (playerCounters.invisibility < 0) { playerCounters.invisibility = 0; }
                break;
            case PlayerCounters.Freeze:
                playerCounters.freeze += amount;
                if (playerCounters.freeze < 0) { playerCounters.freeze = 0; }
                break;
            case PlayerCounters.Poison:
                playerCounters.poison += amount;
                break;
            case PlayerCounters.Neurotoxin:
                playerCounters.neurotoxin += amount;
                playerCounters.poison += amount;
                break;
            case PlayerCounters.Sanctuary:
                playerCounters.sanctuary += amount;
                if (playerCounters.sanctuary < 0) { playerCounters.sanctuary = 0; }
                break;
            case PlayerCounters.Freedom:
                playerCounters.freedom += amount;
                if (playerCounters.freedom < 0) { playerCounters.freedom = 0; }
                break;
            case PlayerCounters.Patience:
                playerCounters.patience += amount;
                if (playerCounters.patience < 0) { playerCounters.patience = 0; }
                break;
            case PlayerCounters.Scarab:
                playerCounters.scarab += amount;
                break;
            case PlayerCounters.Silence:
                playerCounters.silence += amount;
                if (playerCounters.silence < 0) { playerCounters.silence = 0; }
                break;
            case PlayerCounters.Purify:
                if (playerCounters.poison > 0)
                {
                    playerCounters.poison = 0;
                    playerCounters.neurotoxin = 0;
                }
                playerCounters.poison -= amount;
                break;
        }
        OnPlayerCounterUpdate?.Invoke(playerCounters);
    }

    public void RemoveAllCloaks()
    {
        foreach (var perm in playerPermanentManager.GetAllValidCardIds())
        {
            if (perm.card.iD == "5v2" || perm.card.iD == "7ti")
            {
                ResetCloakPermParent(perm);
                EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(perm.id));
                DeactivateCloakEffect();
            }
        }
    }

    public IDCardPair playerID;

    [SerializeField]
    private DeckDisplayer deckDisplayer;

    public HealthDisplayer healthDisplayer;
    [SerializeField]
    private GameObject cloakVisual;
    [SerializeField]
    private Transform permParent;

    public int GetPossibleDamage()
    {
        var value = 0;
        var creatures = playerCreatureField.GetAllValidCardIds();

        foreach (var item in creatures)
        {
            value += item.card.AtkNow;
        }
        var weapon = playerPassiveManager.GetWeapon().card;
        if (weapon is not null)
        {
            if (weapon.cardName != "Weapon") { value += weapon.AtkNow; }
        }
        return value;
    }

    public void ManageGravityCreatures(ref int atkNow)
    {
        var gravityCreatures = playerCreatureField.GetCreaturesWithGravity();
        if (gravityCreatures.Count == 0)
        {
            return;
        }
   
        foreach (var creature in gravityCreatures)
        {
            if (creature.card.DefNow >= atkNow)
            {
                creature.card.DefModify -= atkNow;
                creature.UpdateCard();
                atkNow = 0;
                return;
            }
            else
            {
                atkNow -= creature.card.DefNow;
                EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(creature.id));
            }
        }
    }

    [SerializeField]
    private CardDetailView cardDetailView;

    public bool ManageShield(ref int atkNow, ref IDCardPair cardPair)
    {
        var shield = playerPassiveManager.GetShield();
        if (shield.card.skill == "") { return false; }
        var shieldSkill = shield.card.skill.GetShieldScript<ShieldAbility>();
        shieldSkill.Owner = this;
        shieldSkill.Enemy = DuelManager.Instance.GetNotIDOwner(playerID.id);

        shieldSkill.ActivateShield(ref atkNow, ref cardPair);
        return false;
    }

    public PlayerDisplayer playerDisplayer;

    public HandManager playerHand;
    public CreatureManager playerCreatureField;
    public PermanentManager playerPermanentManager;

    public QuantaManager PlayerQuantaManager;
    public DeckManager DeckManager;
    public PassiveManager playerPassiveManager;
    public HealthManager HealthManager;
    public CardDetailManager CardDetailManager;
    public Counters playerCounters;
    
    public bool isPlayer;
    public void ClearFloodedArea(List<int> safeZones)
    {
        if (DuelManager.FloodCount <= 0) return;
        var idList = playerCreatureField.GetAllValidCardIds();
        foreach (var idCard in idList)
        {
            if (safeZones.Contains(idCard.id.index)) { continue; }
            if (idCard.card.costElement.Equals(Element.Other)) { continue; }
            if (idCard.card.costElement.Equals(Element.Water)) { continue; }
            if (idCard.card.innateSkills.Immaterial) { continue; }
            if (idCard.card.passiveSkills.Burrow) { continue; }
            EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(idCard.id));
        }
    }

    public bool HasSufficientQuanta(Element element, int cost)
    {
        return PlayerQuantaManager.HasEnoughQuanta(element, cost);
    }

    public List<IDCardPair> GetHandCards()
    {
        return playerHand.GetAllValidCardIds();
    }

    public int GetAllQuantaOfElement(Element element)
    {
        return PlayerQuantaManager.GetQuantaForElement(element);
    }

    public void ScrambleQuanta()
    {
        var total = PlayerQuantaManager.GetQuantaForElement(Element.Other);

        if (total <= 9)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, isPlayer, false));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, isPlayer, true));
            return;
        }
        
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, isPlayer, false)); 
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, isPlayer, true));
    }

    public void TurnDownTick()
    {
        playerPassiveManager.PassiveTurnDown();

        playerPermanentManager.PermanentTurnDown();

        playerCreatureField.CreatureTurnDown();
    }

    public void StartTurn()
    {
        TurnDownTick();
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(isPlayer));
        DisplayPlayableGlow();
    }

    public void UpdateCounterAndEffects()
    {
        //Player Counters First
        if (playerCounters.freeze > 0)
        {
            playerCounters.freeze--;
        }

        if (playerCounters.stasis > 0)
        {
            playerCounters.stasis--;
        }

        if (playerCounters.silence > 0)
        {
            playerCounters.silence--;
        }

        if (playerCounters.delay > 0)
        {
            playerCounters.delay--;
        }
        if (playerCounters.invisibility > 0)
        {
            playerCounters.invisibility--;
        }
        OnPlayerCounterUpdate?.Invoke(playerCounters);
    }

    public int GetLightEmittingCreatures()
    {
        var allIds = playerCreatureField.GetAllValidCardIds();
        var count = 0;
        foreach (var id in allIds)
        {
            if (id.card.passiveSkills.Light)
            {
                count++;
            }
        }
        return count;
    }

    public void AddCardToDeck(Card card)
    {
        DeckManager.AddCardToTop(card);
    }
    //Logic

    public void PlayCardOnField(Card card)
    {
        card.AbilityUsed = true;
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, isPlayer));
        EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(card, isPlayer));
    }

    internal void DisplayHand()
    {
        playerHand.ShowCardsForPrecog();
    }

    public void CardDetailButton(Button buttonCase)
    {
        switch (buttonCase.name)
        {
            case "Play":
                if (playerCounters.silence > 0) { return; }
                PlayCardFromHandLogic(CardDetailManager.GetCardID());
                CardDetailManager.ClearID();
                break;
            case "Activate":
                ActivateAbility(CardDetailManager.GetCardID());
                CardDetailManager.ClearID();
                break;
            case "Select Target":
                BattleVars.Shared.IsSelectingTarget = true;
                SkillManager.Instance.SetupTargetHighlights(this, BattleVars.Shared.AbilityOrigin);
                break;
            default:
                CardDetailManager.ClearID();
                break;
        }
    }

    public void ActivateAbility(IDCardPair target)
    {
        if (BattleVars.Shared.AbilityOrigin.card.cardType.Equals(CardType.Spell))
        {
            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.Shared.AbilityOrigin))
            {
                SkillManager.Instance.SkillRoutineWithTarget(this, target);
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.Shared.AbilityOrigin);
            }
            PlayCardFromHandLogic(BattleVars.Shared.AbilityOrigin);
        }
        else
        {
            if (BattleVars.Shared.AbilityOrigin.card.skill != "photosynthesis")
            {
                BattleVars.Shared.AbilityOrigin.card.AbilityUsed = true;
            }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(BattleVars.Shared.AbilityOrigin.card.skillCost, BattleVars.Shared.AbilityOrigin.card.skillElement, isPlayer, false));

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.Shared.AbilityOrigin))
            {
                SkillManager.Instance.SkillRoutineWithTarget(this, target);
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.Shared.AbilityOrigin);
            }

        }
        DuelManager.Instance.ResetTargeting();
        DisplayPlayableGlow();
    }

    public void DealPoisonDamage()
    {
        ModifyHealthLogic(playerCounters.poison, true, false);
    }

    public bool IsCardPlayable(Card cardToCheck)
    {
        var canAfford = PlayerQuantaManager.HasEnoughQuanta(cardToCheck.costElement, cardToCheck.cost);
        var hasSpace = true;

        switch (cardToCheck.cardType)
        {
            case CardType.Pillar:
                hasSpace = playerPermanentManager.GetAllValidCardIds().Count < 14;
                break;
            case CardType.Creature:
                hasSpace = playerCreatureField.GetAllValidCardIds().Count < 23;
                break;
            case CardType.Artifact:
                hasSpace = playerPermanentManager.GetAllValidCardIds().Count < 14;
                break;
        }

        return canAfford && hasSpace;
    }

    public bool IsAbilityUsable(IDCardPair cardToCheck)
    {
        if (!cardToCheck.HasCard()) { return false; }
        if (cardToCheck.card.skill is "" or "none" or null or " ") { return false; }
        if (cardToCheck.card.AbilityUsed) { return false; }
        if (cardToCheck.card.innateSkills.Delay > 0) { return false; }
        if (cardToCheck.card.Freeze > 0) { return false; }
        if (cardToCheck.card.cardType is CardType.Shield or CardType.Pillar or CardType.Mark) { return false; }
        
        var canAfford = PlayerQuantaManager.HasEnoughQuanta(cardToCheck.card.skillElement, cardToCheck.card.skillCost);
        if (canAfford && !SkillManager.Instance.ShouldAskForTarget(cardToCheck)) { return true; }

        if (!SkillManager.Instance.HasEnoughTargets(this, cardToCheck)) { return false; }

        if (!cardToCheck.card.skill.Contains("hasten")) return canAfford;
        return playerHand.GetAllValidCardIds().Count != 8 && canAfford;
    }

    //Command Methods
    public void FillHandWith(Card newCard)
    {
        var amountToAdd = 8 - playerHand.GetAllValidCardIds().Count;
        for (var i = 0; i < amountToAdd; i++)
        {
            DeckManager.AddCardToTop(CardDatabase.Instance.GetCardFromId(newCard.iD));
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(isPlayer));
        }
    }

    //Modify Health Logic and Visual Command Pair
    public void ModifyHealthLogic(int amount, bool isDamage, bool fromSpell)
    {
        if (sacrificeCount > 0) { isDamage = !isDamage; }
        EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(isDamage ? -amount : amount, isPlayer, false));
    }

    public List<ID> cloakIndex = new();
    public int sacrificeCount;

    //Play Card From Hand Logic and Visual Command Pair
    public void PlayCardFromHandLogic(IDCardPair cardID)
    {
        //Logic Side
        //Get Card SO In Hand
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("CardPlay"));
        if (playerCounters.neurotoxin > 0)
        {
            AddPlayerCounter(PlayerCounters.Neurotoxin, 1);
        }
        //Play Card on field if it is not a spell
        if (!cardID.card.cardType.Equals(CardType.Spell))
        {
            PlayCardOnField(cardID.card);
        }

        //Spend Quanta
        if (cardID.card.cost > 0)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(cardID.card.cost, cardID.card.costElement, isPlayer, false));
        }
        //Remove Card From Hand
        EventBus<RemoveCardFromHandEvent>.Raise(new RemoveCardFromHandEvent(isPlayer, cardID.id));
        DisplayPlayableGlow();
    }
    
    public void HideAllPlayableGlow()
    {
        if (playerHand.GetAllValidCardIds().Count > 0)
        {
            foreach (var displayer in playerHand.GetAllValidCardIds())
            {
                EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(false, displayer.id));
            }
        }

        if (playerCreatureField.GetAllValidCardIds().Count > 0)
        {
            foreach (var displayer in playerCreatureField.GetAllValidCardIds())
            {
                EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(false, displayer.id));
            }
        }

        if (playerPermanentManager.GetAllValidCardIds().Count > 0)
        {

            foreach (var displayer in playerPermanentManager.GetAllValidCardIds())
            {
                EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(false, displayer.id));
            }
        }
        EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(false, playerPassiveManager.GetWeaponID()));
    }

    private void DisplayPlayableGlow()
    {
        HideAllPlayableGlow();
        if (!isPlayer) { return; }

        if (playerHand.GetAllValidCardIds().Count > 0)
        {
            foreach (var displayer in playerHand.GetAllValidCardIds())
            {
                EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(IsCardPlayable(displayer.card), displayer.id));
            }
        }

        if (playerCreatureField.GetAllValidCardIds().Count > 0)
        {
            foreach (var displayer in playerCreatureField.GetAllValidCardIds())
            {
                EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(IsCardPlayable(displayer.card), displayer.id));
            }
        }

        if (playerPermanentManager.GetAllValidCardIds().Count > 0)
        {

            foreach (var displayer in playerPermanentManager.GetAllValidCardIds())
            {
                EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(IsCardPlayable(displayer.card), displayer.id));
            }
        }
        EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(IsAbilityUsable(playerPassiveManager.GetWeapon()), playerPassiveManager.GetWeaponID()));
    }

    public void ActivateCloakEffect(IDCardPair cardPair)
    {
        if (isPlayer) { return; }
        cloakVisual.SetActive(true); 
        cardPair.transform.parent.transform.parent = cloakVisual.transform;

    }
    public void DeactivateCloakEffect()
    {
        if (isPlayer) { return; }
        cloakVisual.SetActive(false);
    }

    public void ResetCloakPermParent(IDCardPair cardPair)
    {
        if (isPlayer) { return; }
        cardPair.transform.parent.transform.parent = permParent.transform;
        cardPair.transform.SetSiblingIndex(cardPair.id.index);
    }

    public void CheckEclipseNightfall(bool isAdded, string id)
    {
        var creatures = playerCreatureField.GetAllValidCardIds();

        var eclipseCount = DuelManager.Instance.GetCardCount(new() { "7ta" });
        var nightfallCount = DuelManager.Instance.GetCardCount(new() { "5uq" });

        var atkMod = 0;
        var defMod = 0;
        if (isAdded)
        {
            switch (id)
            {
                case "7ta":
                    if (eclipseCount == 1 && nightfallCount > 0)
                    {
                        atkMod = 1;
                        defMod = 0;
                        break;
                    }
                    if (eclipseCount == 1 && nightfallCount == 0)
                    {
                        atkMod = 2;
                        defMod = 1;
                    }
                    break;
                case "5uq":
                    if (eclipseCount == 0 && nightfallCount == 1)
                    {
                        atkMod = 1;
                        defMod = 1;
                    }
                    break;
            }
        }
        else
        {
            switch (id)
            {
                case "7ta":
                    if (eclipseCount == 0 && nightfallCount > 0)
                    {
                        atkMod = -1;
                        defMod = 0;
                        break;
                    }
                    if (eclipseCount == 0 && nightfallCount == 0)
                    {
                        atkMod = -2;
                        defMod = -1;
                    }
                    break;
                case "5uq":
                    if (eclipseCount == 0 && nightfallCount == 0)
                    {
                        atkMod = -1;
                        defMod = -1;
                    }
                    break;
            }
        }

        foreach (var creature in creatures)
        {
            if (!creature.card.costElement.Equals(Element.Darkness) &&
                !creature.card.costElement.Equals(Element.Death)) continue;
            creature.card.DefModify += defMod;
            creature.card.AtkModify += atkMod;
            creature.UpdateCard();
        }
    }

    public void DiscardCard(IDCardPair cardToDiscard)
    {
        if (cardToDiscard.card.innateSkills.Obsession)
        {
            ModifyHealthLogic(cardToDiscard.card.iD.IsUpgraded() ? 13 : 10, true, false);
        }
        EventBus<RemoveCardFromHandEvent>.Raise(new RemoveCardFromHandEvent(isPlayer, cardToDiscard.id));
        BattleVars.Shared.HasToDiscard = false;
    }

    public IEnumerator EndTurnRoutine()
    {
        if (playerPermanentManager.GetAllValidCardIds().Count > 0)
        {
            EventBus<OnTurnEndEvent>.Raise(new OnTurnEndEvent(CardType.Pillar, isPlayer));
            yield return new WaitForSeconds(0.25f);
        }

        EventBus<OnTurnEndEvent>.Raise(new OnTurnEndEvent(CardType.Mark, isPlayer));
        yield return new WaitForSeconds(0.25f);
        
        if (playerPermanentManager.GetAllValidCardIds().Count > 0)
        {
            EventBus<OnTurnEndEvent>.Raise(new OnTurnEndEvent(CardType.Artifact, isPlayer));
            yield return new WaitForSeconds(0.25f);
        }
        
        DuelManager.Instance.GetNotIDOwner(playerID.id).DealPoisonDamage();
        
        if (playerCreatureField.GetAllValidCardIds().Count > 0)
        {
            EventBus<OnTurnEndEvent>.Raise(new OnTurnEndEvent(CardType.Creature, isPlayer));
            yield return new WaitForSeconds(0.25f);
        }
        EventBus<OnTurnEndEvent>.Raise(new OnTurnEndEvent(CardType.Weapon, isPlayer));
        yield return new WaitForSeconds(0.25f);
        EventBus<OnTurnEndEvent>.Raise(new OnTurnEndEvent(CardType.Shield, isPlayer));
        yield return new WaitForSeconds(0.25f);

        if (isPlayer)
        {
            BattleVars.Shared.ChangePlayerTurn();
        }
    }
    
    private IEnumerator SetupPassiveDisplayers()
    {
        playerPassiveManager.SetupManager(isPlayer);
        playerCreatureField.SetupManager(isPlayer);
        playerPermanentManager.SetupManager(isPlayer);
        Element markElement;
        markElement = isPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;

        var mark = CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.MarkIds[(int)markElement]);
        EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(mark, isPlayer));
        EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(CardDatabase.Instance.GetPlaceholderCard(2), isPlayer));
        EventBus<PlayCardOnFieldEvent>.Raise(new PlayCardOnFieldEvent(CardDatabase.Instance.GetPlaceholderCard(1), isPlayer));
        yield return null;
    }


    private IEnumerator SetupOtherDisplayers()
    {
        playerCreatureField.ClearField();
        playerPermanentManager.ClearField();
        PlayerQuantaManager = new QuantaManager(isPlayer);
        CardDetailManager = new CardDetailManager();
        CardDetailManager.OnDisplayNewCard += cardDetailView.SetupCardDisplay;
        CardDetailManager.OnRemoveCard += cardDetailView.CancelButtonAction;
        playerCounters = new Counters();
        playerID.id = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Player, 0);

        List<Card> deck = new(isPlayer ?
                    PlayerData.Shared.currentDeck.DeserializeCard()
                    : new List<string>(BattleVars.Shared.EnemyAiData.deck.Split(" ")).DeserializeCard());

        if (BattleVars.Shared.EnemyAiData.maxHp == 200 && !isPlayer)
        {
            deck.AddRange(deck);

        }
        deck.Shuffle();
        DeckManager = new DeckManager(deck, isPlayer);

        HealthManager = new HealthManager(isPlayer ? 100 : BattleVars.Shared.EnemyAiData.maxHp, isPlayer);
        healthDisplayer.SetHpStart(HealthManager.GetCurrentHealth());
        playerID.card = null;
        playerHand.SetIsPlayer(isPlayer);
        
        for (var i = 0; i < 7; i++)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(isPlayer));
        }
        //if (isPlayer && PlayerData.shared.petName != "" && PlayerData.shared.petName != null)
        //{
        //    Card petCard = CardDatabase.Instance.GetCardFromId(PlayerData.shared.petName);
        //    PlayerData.shared.petCount--;
        //    if (PlayerData.shared.petCount <= 0)
        //    {
        //        PlayerData.shared.petName = "";
        //    }
        //    PlayCardOnFieldLogic(petCard);
        //}
        playerDisplayer.isPlayer = isPlayer;
        OnPlayerCounterUpdate += playerDisplayer.UpdatePlayerIndicators;
        if (!isPlayer)
        {
            DuelManager.Instance.allPlayersSetup = true;
        }
        yield return null;
    }

    public IEnumerator SetupPlayerManager(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        yield return StartCoroutine(SetupPassiveDisplayers());
        yield return StartCoroutine(SetupOtherDisplayers());
    }

    public void ReceivePhysicalDamage(int damage)
    {
        if (damage == 0) { return; }
        //Damage player with leftover damage or direct damage
        ModifyHealthLogic(damage, true, false);
    }

    public void SetupCardDisplay(IDCardPair iDCardPair)
    {
        CardDetailManager.SetCardOnDisplay(iDCardPair);
    }

    public void QuickPlay(IDCardPair iDCardPair)
    {
        if (iDCardPair.IsFromHand())
        {
            if (playerCounters.silence > 0 && playerCounters.sanctuary == 0) { return; }
            if (!IsCardPlayable(iDCardPair.card))
            {
                SetupCardDisplay(iDCardPair);
                return;
            }

            if (!iDCardPair.card.cardType.Equals(CardType.Spell))
            {
                PlayCardFromHandLogic(iDCardPair);
            }
            else
            {
                ProcessSpellCard(iDCardPair);
            }
        }
        else
        {
            if (!IsAbilityUsable(iDCardPair))
            {
                SetupCardDisplay(iDCardPair);
                return;
            }

            ProcessAbilityCard(iDCardPair);
        }
    }

    private void ProcessAbilityCard(IDCardPair iDCardPair)
    {
        BattleVars.Shared.AbilityOrigin = iDCardPair;

        if (!SkillManager.Instance.ShouldAskForTarget(iDCardPair))
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(iDCardPair.card.skillCost, iDCardPair.card.skillElement, isPlayer, false));
            SkillManager.Instance.SkillRoutineNoTarget(this, iDCardPair);
            if (iDCardPair.card.skill != "photosynthesis")
            {
                iDCardPair.card.AbilityUsed = true;
            }
        }
        else
        {
            BattleVars.Shared.IsSelectingTarget = true;
            SkillManager.Instance.SetupTargetHighlights(this, iDCardPair);
        }
    }

    private void ProcessSpellCard(IDCardPair iDCardPair)
    {
        BattleVars.Shared.AbilityOrigin = iDCardPair;
        if (!SkillManager.Instance.ShouldAskForTarget(iDCardPair))
        {
            SkillManager.Instance.SkillRoutineNoTarget(this, iDCardPair);
            PlayCardFromHandLogic(iDCardPair);
        }
        else
        {
            BattleVars.Shared.IsSelectingTarget = true;
            SkillManager.Instance.SetupTargetHighlights(this, iDCardPair);
        }
    }

    public bool HasGravityCreatures()
    {
        return playerCreatureField.GetCreaturesWithGravity().Count > 0;
    }
}


public enum PlayerCounters
{
    Bone,
    Invisibility,
    Freeze,
    Poison,
    Neurotoxin,
    Sanctuary,
    Freedom,
    Patience,
    Scarab,
    Silence,
    Sacrifice,
    Purify,
    Delay
}