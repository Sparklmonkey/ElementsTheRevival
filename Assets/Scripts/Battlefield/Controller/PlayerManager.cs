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
    //Refactor Section
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
                perm.RemoveCard();
                DeactivateCloakEffect();
            }
        }
    }

    public IDCardPair playerID;

    [SerializeField]
    private List<QuantaDisplayer> quantaDisplayers;

    internal void ShowTargetHighlight(IDCardPair idCardPair)
    {
        idCardPair.IsTargeted(true);
    }

    [SerializeField]
    private DeckDisplayer deckDisplayer;

    public HealthDisplayer healthDisplayer;
    [SerializeField]
    private GameObject cloakVisual;
    [SerializeField]
    private Transform permParent;

    public int GetPossibleDamage()
    {
        int value = 0;
        var creatures = playerCreatureField.GetAllValidCardIds();

        foreach (var item in creatures)
        {
            value += item.card.AtkNow;
        }
        Card weapon = playerPassiveManager.GetWeapon().card;
        if (weapon != null)
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
                creature.RemoveCard();
            }
        }
    }

    [SerializeField]
    private CardDetailView cardDetailView;

    public bool ManageShield(ref int atkNow, ref IDCardPair cardPair)
    {
        IDCardPair shield = playerPassiveManager.GetShield();
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
        if (DuelManager.FloodCount > 0)
        {
            var idList = playerCreatureField.GetAllValidCardIds();
            foreach (var idCard in idList)
            {
                if (safeZones.Contains(idCard.id.index)) { continue; }
                if (idCard.card.costElement.Equals(Element.Other)) { continue; }
                if (idCard.card.costElement.Equals(Element.Water)) { continue; }
                if (idCard.card.innateSkills.Immaterial) { continue; }
                if (idCard.card.passiveSkills.Burrow) { continue; }
                idCard.RemoveCard();
            }
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
        int total = PlayerQuantaManager.GetQuantaForElement(Element.Other);

        if (total <= 9)
        {
            SpendQuantaLogic(Element.Other, total);
            GenerateQuantaLogic(Element.Other, total);

            return;
        }

        SpendQuantaLogic(Element.Other, 9);
        GenerateQuantaLogic(Element.Other, 9);
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
        DrawCardFromDeckLogic();
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
        int count = 0;
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
        ActionManager.AddCardPlayedOnFieldAction(isPlayer, card);
        switch (card.cardType)
        {
            case CardType.Pillar:
                if (card.iD.IsUpgraded())
                {
                    if (card.costElement.Equals(Element.Other))
                    {
                        GenerateQuantaLogic(card.costElement, 3);
                    }
                    else
                    {
                        GenerateQuantaLogic(card.costElement, 1);
                    }
                }
                playerPermanentManager.PlayPermanent(card);
                break;
            case CardType.Creature:
                playerCreatureField.PlayCreature(card);
                break;
            case CardType.Artifact:
                playerPermanentManager.PlayPermanent(card);
                break;
            case CardType.Weapon:
            case CardType.Shield:
            case CardType.Mark:
                playerPassiveManager.PlayPassive(card);
                break;
        }
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
            SpendQuantaLogic(BattleVars.Shared.AbilityOrigin.card.skillElement, BattleVars.Shared.AbilityOrigin.card.skillCost);

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
        bool canAfford = PlayerQuantaManager.HasEnoughQuanta(cardToCheck.costElement, cardToCheck.cost);
        bool hasSpace = true;

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
        int amountToAdd = 8 - playerHand.GetAllValidCardIds().Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            DeckManager.AddCardToTop(CardDatabase.Instance.GetCardFromId(newCard.iD));
            DrawCardFromDeckLogic();
        }
    }

    //Modify Health Logic and Visual Command Pair
    public void ModifyHealthLogic(int amount, bool isDamage, bool fromSpell)
    {
        if (sacrificeCount > 0) { isDamage = !isDamage; }
        HealthManager.ModifyHealth(amount, isDamage);
    }

    public List<ID> cloakIndex = new();
    public int sacrificeCount;

    //Play Card From Hand Logic and Visual Command Pair
    public void PlayCardFromHandLogic(IDCardPair cardID)
    {
        //Logic Side
        //Get Card SO In Hand
        SoundManager.Instance.PlayAudioClip("CardPlay");
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
            SpendQuantaLogic(cardID.card.costElement, cardID.card.cost);
        }
        //Remove Card From Hand
        playerHand.UpdateHandVisual(cardID);
        DisplayPlayableGlow();
    }

    internal void ModifyMaxHealthLogic(int maxHpBuff, bool isIncrease)
    {
        HealthManager.ModifyMaxHealth(maxHpBuff, isIncrease);
    }

    //Draw Card From Deck Logic and Visual Command Pair
    public void DrawCardFromDeckLogic(bool isInitialDraw = false)
    {
        if (playerHand == null) { return; }
        if (playerHand.GetAllValidCardIds().Count == 8) { return; }
        //Logic Side
        //Get Card From Deck
        Card newCard = DeckManager.DrawCard();
        //Add Card To Hand
        playerHand.AddCardToHand(newCard);
        if (!isInitialDraw)
        {
            ActionManager.AddCardDrawAction(isPlayer, newCard);
        }
        DisplayPlayableGlow();
    }

    public void HideAllPlayableGlow()
    {
        if (playerHand.GetAllValidCardIds().Count > 0)
        {
            foreach (IDCardPair displayer in playerHand.GetAllValidCardIds())
            {
                displayer.IsPlayable(false);
            }
        }

        if (playerCreatureField.GetAllValidCardIds().Count > 0)
        {
            foreach (IDCardPair displayer in playerCreatureField.GetAllValidCardIds())
            {
                displayer.IsPlayable(false);
            }
        }

        if (playerPermanentManager.GetAllValidCardIds().Count > 0)
        {

            foreach (IDCardPair displayer in playerPermanentManager.GetAllValidCardIds())
            {
                displayer.IsPlayable(false);
            }
        }
        playerPassiveManager.GetWeapon().IsPlayable(false);
    }

    void DisplayPlayableGlow()
    {
        HideAllPlayableGlow();
        if (!isPlayer) { return; }

        if (playerHand.GetAllValidCardIds().Count > 0)
        {
            foreach (IDCardPair displayer in playerHand.GetAllValidCardIds())
            {
                displayer.IsPlayable(IsCardPlayable(displayer.card));
            }
        }

        if (playerCreatureField.GetAllValidCardIds().Count > 0)
        {
            foreach (IDCardPair displayer in playerCreatureField.GetAllValidCardIds())
            {
                displayer.IsPlayable(IsAbilityUsable(displayer));
            }
        }

        if (playerPermanentManager.GetAllValidCardIds().Count > 0)
        {

            foreach (IDCardPair displayer in playerPermanentManager.GetAllValidCardIds())
            {
                displayer.IsPlayable(IsAbilityUsable(displayer));
            }
        }
        playerPassiveManager.GetWeapon().IsPlayable(IsAbilityUsable(playerPassiveManager.GetWeapon()));
    }

    public void ActivateDeathTriggers()
    {
        var cardsToCheck = playerCreatureField.GetAllValidCardIds();

        if (cardsToCheck.Count > 0)
        {
            foreach (var creature in cardsToCheck)
            {
                creature.cardBehaviour.DeathTrigger();
            }
        }
        cardsToCheck = playerPermanentManager.GetAllValidCardIds();

        if (cardsToCheck.Count > 0)
        {
            foreach (var permanent in cardsToCheck)
            {
                permanent.cardBehaviour.DeathTrigger();
            }
        }

        if (playerPassiveManager.GetShield().HasCard())
        {
            playerPassiveManager.GetShield().cardBehaviour.DeathTrigger();
        }
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
        List<IDCardPair> creatures = playerCreatureField.GetAllValidCardIds();

        int eclipseCount = DuelManager.Instance.GetCardCount(new() { "7ta" });
        int nightfallCount = DuelManager.Instance.GetCardCount(new() { "5uq" });

        int atkMod = 0;
        int defMod = 0;
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
            if (creature.card.costElement.Equals(Element.Darkness) || creature.card.costElement.Equals(Element.Death))
            {
                creature.card.DefModify += defMod;
                creature.card.AtkModify += atkMod;
                creature.UpdateCard();
            }
        }
    }

    public void DiscardCard(IDCardPair cardToDiscard)
    {
        if (cardToDiscard.card.innateSkills.Obsession)
        {
            ModifyHealthLogic(cardToDiscard.card.iD.IsUpgraded() ? 13 : 10, true, false);
        }
        playerHand.UpdateHandVisual(cardToDiscard);
        BattleVars.Shared.HasToDiscard = false;
    }

    public void EndTurnRoutine()
    {
        var permList = playerPermanentManager.GetAllValidCardIds();

        var pillarList = permList.FindAll(x => x.card.cardType == CardType.Pillar);
        if (pillarList.Count > 0)
        {
            foreach (var pillar in pillarList)
            {
                pillar.cardBehaviour.OnTurnEnd();
            }
        }
        playerPassiveManager.GetMark().cardBehaviour.OnTurnEnd();

        pillarList = permList.FindAll(x => x.card.cardType == CardType.Artifact);
        if (pillarList.Count > 0)
        {
            foreach (var perm in pillarList)
            {
                perm.cardBehaviour.OnTurnEnd();
            }
        }

        DuelManager.Instance.GetNotIDOwner(playerID.id).DealPoisonDamage();
        var creaturelist = playerCreatureField.GetAllValidCardIds();
        if (creaturelist.Count > 0)
        {
            foreach (var creature in creaturelist)
            {
                creature.cardBehaviour.OnTurnEnd();
            }
        }

        if (playerPassiveManager.GetWeapon().HasCard())
        {
            playerPassiveManager.GetWeapon().cardBehaviour.OnTurnEnd();
        }

        if (playerPassiveManager.GetShield().HasCard())
        {
            playerPassiveManager.GetShield().cardBehaviour.OnTurnEnd();
        }

    }

    public void SpendQuantaLogic(Element element, int amount)
    {

        if ((isPlayer && !BattleVars.Shared.IsPlayerTurn) || (!isPlayer && BattleVars.Shared.IsPlayerTurn))
        {
            if (playerCounters.sanctuary > 0)
            {
                return;
            }
        }
        PlayerQuantaManager.ChangeQuanta(element, amount, false);
    }

    public void GenerateQuantaLogic(Element element, int amount)
    {

        if ((isPlayer && !BattleVars.Shared.IsPlayerTurn) || (!isPlayer && BattleVars.Shared.IsPlayerTurn))
        {
            if (playerCounters.sanctuary > 0)
            {
                return;
            }
        }
        PlayerQuantaManager.ChangeQuanta(element, amount, true);
    }

    private IEnumerator SetupPassiveDisplayers()
    {
        Element markElement;
        markElement = isPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;

        Card mark = CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.MarkIds[(int)markElement]);
        playerPassiveManager.PlayPassive(mark);
        playerPassiveManager.PlayPassive(CardDatabase.Instance.GetPlaceholderCard(2));
        playerPassiveManager.PlayPassive(CardDatabase.Instance.GetPlaceholderCard(1));
        yield return null;
    }


    private IEnumerator SetupOtherDisplayers()
    {
        playerCreatureField.ClearField();
        playerPermanentManager.ClearField();
        PlayerQuantaManager = new QuantaManager(quantaDisplayers, this);
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
        DeckManager = new DeckManager(deck);
        DeckManager.OnDeckCountChange += deckDisplayer.UpdateDeckCount;
        for (int i = 0; i < 7; i++)
        {
            DrawCardFromDeckLogic(true);
        }

        HealthManager = new HealthManager(isPlayer ? 100 : BattleVars.Shared.EnemyAiData.maxHp, isPlayer);
        healthDisplayer.SetHpStart(HealthManager.GetCurrentHealth());
        HealthManager.OnHealthChangedEvent += healthDisplayer.OnHealthChanged;
        HealthManager.OnMaxHealthUpdatedEvent += healthDisplayer.OnMaxHealthChanged;
        playerID.card = null;
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
            SpendQuantaLogic(iDCardPair.card.skillElement, iDCardPair.card.skillCost);
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