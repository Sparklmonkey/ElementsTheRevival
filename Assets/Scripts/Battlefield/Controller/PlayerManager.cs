using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Elements.Duel.Manager;
using Elements.Duel.Visual;
using System.Linq;
using TMPro;

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
            default:
                break;
        }
        OnPlayerCounterUpdate?.Invoke(playerCounters);
    }

    public void RemoveAllCloaks()
    {
        foreach(var perm in playerPermanentManager.GetAllValidCardIds())
        {
            if (perm.card.skill == "cloak")
            {
                perm.RemoveCard();
                DeactivateCloakEffect(perm);
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
    public int scarabsPlayed = 0;

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

    public void ManageGravityCreatures(ref int atkNow, ref IDCardPair attacker)
    {
        var gravityCreatures = playerCreatureField.GetCreaturesWithGravity();
        if(gravityCreatures.Count == 0) { return; }

        foreach (var creature in gravityCreatures)
        {
            attacker.card.DefDamage += creature.card.AtkNow;
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
            attacker.UpdateCard();
        }
    }

    [SerializeField]
    private CardDetailView cardDetailView;

    public void ManageShield(ref int atkNow, ref IDCardPair cardPair)
    {
        IDCardPair shield = playerPassiveManager.GetShield();
        var shieldSkill = shield.card.skill.GetShieldScript<ShieldAbility>();
        shieldSkill.Owner = this;
        shieldSkill.Enemy = DuelManager.GetNotIDOwner(playerID.id);

        shieldSkill.ActivateShield(ref atkNow, ref cardPair);
    }

    public PlayerDisplayer playerDisplayer;

    public HandManager playerHand;
    public CreatureManager playerCreatureField;
    public PermanentManager playerPermanentManager;

    public QuantaManager playerQuantaManager;
    public DeckManager deckManager;
    public PassiveManager playerPassiveManager;
    public HealthManager healthManager;
    public CardDetailManager cardDetailManager;
    public Counters playerCounters;

    public bool isPlayer;

    private float animSpeed;
    private void Update()
    {
        animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
    }

    public void ClearFloodedArea(List<int> safeZones)
    {
        if (DuelManager.floodCount > 0)
        {
            var idList = playerCreatureField.GetAllValidCardIds();
            foreach (var idCard in idList)
            {
                if (safeZones.Contains(idCard.id.Index)) { continue; }
                if (idCard.card.costElement.Equals(Element.Other)) { continue; }
                if (idCard.card.costElement.Equals(Element.Water)) { continue; }
                if (idCard.card.innate.Contains("immaterial")) { continue; }
                if (idCard.card.innate.Contains("burrow")) { continue; }
                idCard.RemoveCard();
            }
        }
    }

    public bool HasSufficientQuanta(Element element, int cost)
    {
        return playerQuantaManager.HasEnoughQuanta(element, cost);
    }

    public List<IDCardPair> GetHandCards()
    {
        return playerHand.GetAllValidCardIds();
    }

    public int GetAllQuantaOfElement(Element element)
    {
        return playerQuantaManager.GetQuantaForElement(element);
    }

    public void ScrambleQuanta()
    {
        int total = playerQuantaManager.GetQuantaForElement(Element.Other);

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
        if (deckManager.GetDeckCount() <= 0)
        {
            GameOverVisual.ShowGameOverScreen(!isPlayer);
            return;
        }
        playerCounters.scarab = 0;
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
            if (id.card.passive.Contains("light"))
            {
                count++;
            }
        }
        return count;
    }

    public void AddCardToDeck(Card card)
    {
        deckManager.AddCardToTop(card);
    }
    //Logic

    public IDCardPair PlayCardOnField(Card card)
    {
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
                return playerPermanentManager.PlayPermanent(card);
            case CardType.Creature:
                return playerCreatureField.PlayCreature(card);
            case CardType.Spell:
                return null;
            case CardType.Artifact:
                return playerPermanentManager.PlayPermanent(card);
            case CardType.Weapon:
                return playerPassiveManager.PlayPassive(card);
            case CardType.Shield:
                return playerPassiveManager.PlayPassive(card);
            case CardType.Mark:
                return playerPassiveManager.PlayPassive(card);
            default:
                break;
        }
        return null;
    }

    internal void DisplayHand()
    {
        var cards = playerHand.GetAllValidCardIds();

        for (int i = 0; i < cards.Count; i++)
        {
            //handDisplayers[i].ShowCardForPrecog(cards[i]);
        }
        shouldHideCards = true;
    }
    internal void HideHand()
    {
        if (!shouldHideCards) { return; }
        var cards = playerHand.GetAllValidCardIds();

        for (int i = 0; i < cards.Count; i++)
        {
            //handDisplayers[i].HideCardForPrecog();
        }
        shouldHideCards = false;
    }

    public void CardDetailButton(Button buttonCase)
    {
        switch (buttonCase.name)
        {
            case "Play":
                if (playerCounters.silence > 0) { return; }
                PlayCardFromHandLogic(cardDetailManager.GetCardID());
                cardDetailManager.ClearID();
                break;
            case "Activate":
                ActivateAbility(cardDetailManager.GetCardID());
                cardDetailManager.ClearID();
                break;
            case "Select Target":
                BattleVars.shared.isSelectingTarget = true;
                SkillManager.Instance.SetupTargetHighlights(this, BattleVars.shared.abilityOrigin);
                break;
            default:
                cardDetailManager.ClearID();
                break;
        }
    }

    public void ActivateAbility(IDCardPair target)
    {
        if (BattleVars.shared.abilityOrigin.card.cardType.Equals(CardType.Spell))
        {
            //ActionManager.AddSpellPlayedAction(isPlayer, originCard, BattleVars.shared.IsFixedTarget() ? null : targetCard);

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.shared.abilityOrigin))
            {
                SkillManager.Instance.SkillRoutineWithTarget(this, target);
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.shared.abilityOrigin);
            }
            PlayCardFromHandLogic(BattleVars.shared.abilityOrigin);
        }
        else
        {
            //ActionManager.AddAbilityActivatedAction(isPlayer, originCard, BattleVars.shared.IsFixedTarget() ? null : targetCard);
            if (BattleVars.shared.abilityOrigin.card.skill != "photosynthesis")
            {
                BattleVars.shared.abilityOrigin.card.AbilityUsed = true;
            }
            SpendQuantaLogic(BattleVars.shared.abilityOrigin.card.skillElement, BattleVars.shared.abilityOrigin.card.skillCost);

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.shared.abilityOrigin))
            {
                SkillManager.Instance.SkillRoutineWithTarget(this, target);
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.shared.abilityOrigin);
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
        bool canAfford = playerQuantaManager.HasEnoughQuanta(cardToCheck.costElement, cardToCheck.cost);
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
            default:
                break;
        }

        return canAfford && hasSpace && !cardToCheck.AbilityUsed;
    }

    public bool IsAbilityUsable(Card cardToCheck)
    {
        if (cardToCheck == null) { return false; }
        if (cardToCheck.skill == "" || cardToCheck.skill == "none" || cardToCheck.skill == null || cardToCheck.skill == " ") { return false; }
        if (cardToCheck.AbilityUsed) { return false; }
        if (cardToCheck.IsDelayed) { return false; }
        if (cardToCheck.Freeze > 0) { return false; }

        bool canAfford = playerQuantaManager.HasEnoughQuanta(cardToCheck.skillElement, cardToCheck.skillCost);
        if (cardToCheck.skill.ToString().Contains("Haste"))
        {
            if (playerHand.GetAllValidCardIds().Count == 8) { return false; }
        }

        return canAfford;
    }

    public bool IsSpellPlayable(Card cardToCheck)
    {
        if (cardToCheck == null) { return false; }
        if (cardToCheck.skill == "" || cardToCheck.skill == "none") { return false; }

        bool canAfford = playerQuantaManager.HasEnoughQuanta(cardToCheck.costElement, cardToCheck.cost);
        if (cardToCheck.skill == "flying")
        {
            if (playerPassiveManager.GetWeapon().card.iD == "4t2") { return false; }
        }

        return canAfford;
    }

    //Command Methods
    public void FillHandWith(Card newCard)
    {
        int amountToAdd = 8 - playerHand.GetAllValidCardIds().Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            deckManager.AddCardToTop(CardDatabase.Instance.GetCardFromId(newCard.iD));
            DrawCardFromDeckLogic();
        }
    }

    //Modify Health Logic and Visual Command Pair
    public void ModifyHealthLogic(int amount, bool isDamage, bool fromSpell)
    {
        if (sacrificeCount > 0) { isDamage = !isDamage; }
        healthManager.ModifyHealth(amount, isDamage);
    }

    public List<ID> cloakIndex = new();
    public int sacrificeCount;
    private bool shouldHideCards;

    //Play Card From Hand Logic and Visual Command Pair
    public void PlayCardFromHandLogic(IDCardPair cardID)
    {
        //Logic Side
        //Get Card SO In Hand
        Game_SoundManager.shared.PlayAudioClip("CardPlay");
        if (playerCounters.neurotoxin > 0)
        {
            AddPlayerCounter(PlayerCounters.Neurotoxin, 1);
        }
        //Play Card on field if it is not a spell
        if (!cardID.card.cardType.Equals(CardType.Spell))
        {
            var isNull = PlayCardOnFieldLogic(cardID.card);
            if (isNull == null)
            {
                return;
            }
            ActionManager.AddCardPlayedOnFieldAction(isPlayer, cardID.card);
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

    internal void ModifyMaxHealthLogic(int maxHPBuff, bool isIncrease)
    {
        healthManager.ModifyMaxHealth(maxHPBuff, isIncrease);
    }

    //Draw Card From Deck Logic and Visual Command Pair
    public void DrawCardFromDeckLogic(bool isInitialDraw = false)
    {
        if (playerHand == null) { return; }
        if (playerHand.GetAllValidCardIds().Count == 8) { return; }
        //Logic Side
        //Get Card From Deck
        Card newCard = deckManager.DrawCard();
        if (newCard == null)
        {
            Debug.Log("Game Over");
            GameOverVisual.ShowGameOverScreen(!isPlayer);
            return;
        }
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
        if (!isPlayer) { return; }
        HideAllPlayableGlow();

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
                displayer.IsPlayable(IsAbilityUsable(displayer.card));
            }
        }

        if (playerPermanentManager.GetAllValidCardIds().Count > 0)
        {

            foreach (IDCardPair displayer in playerPermanentManager.GetAllValidCardIds())
            {
                displayer.IsPlayable(IsAbilityUsable(displayer.card));
            }
        }
        playerPassiveManager.GetWeapon().IsPlayable(false);
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
        cloakVisual.SetActive(true); ;
        cardPair.transform.parent.transform.parent = cloakVisual.transform;

    }
    public void DeactivateCloakEffect(IDCardPair cardPair)
    {
        if (isPlayer) { return; }
        cloakVisual.SetActive(false);
        cardPair.transform.parent.transform.parent = permParent.transform;
        cardPair.transform.SetSiblingIndex(cardPair.id.Index);
    }

    public void CheckEclipseNightfall(bool isAdded, string skill)
    {
        List<IDCardPair> creatures = playerCreatureField.GetAllValidCardIds();
        int atkMod = 0;
        int defMod = 0;
        switch (skill)
        {
            case "eclipse":
                atkMod = DuelManager.IsEclipseInPlay() ? 0 : DuelManager.IsNightfallInPlay() ? 1 : 2;
                defMod = DuelManager.IsEclipseInPlay() ? 0 : DuelManager.IsNightfallInPlay() ? 0 : 1;
                break;
            case "nightfall":
                atkMod = DuelManager.IsEclipseInPlay() || DuelManager.IsNightfallInPlay() ? 0 : 1;
                defMod = DuelManager.IsEclipseInPlay() || DuelManager.IsNightfallInPlay() ? 0 : 1;
                break;
            default:
                break;
        }

        atkMod = isAdded ? atkMod : -atkMod;
        defMod = isAdded ? defMod : -defMod;

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

    //Play Card from anywhere Logic and Visual
    public IDCardPair PlayCardOnFieldLogic(Card card)
    {
        var newLocationId = PlayCardOnField(card);
        if (newLocationId == null) { return null; }
        newLocationId.card.AbilityUsed = true;
        if (newLocationId.card.cardType.Equals(CardType.Creature))
        {
            if (newLocationId.card.costElement.Equals(Element.Darkness) || newLocationId.card.costElement.Equals(Element.Death))
            {
                if (DuelManager.IsEclipseInPlay())
                {
                    newLocationId.card.DefModify += 1;
                    newLocationId.card.AtkModify += 2;
                }
                else if (DuelManager.IsNightfallInPlay())
                {
                    newLocationId.card.DefModify += 1;
                    newLocationId.card.AtkModify += 1;
                }
                newLocationId.UpdateCard();
            }
        }
        
        return newLocationId;
    }

    public void DiscardCard(IDCardPair cardToDiscard)
    {
        playerHand.UpdateHandVisual(cardToDiscard);
        BattleVars.shared.hasToDiscard = false;
    }

    public void EndTurnRoutine()
    {
        var permList = playerPermanentManager.GetAllValidCardIds();

        var pillarList = permList.FindAll(x => x.card.cardType == CardType.Pillar);
        if(pillarList.Count > 0)
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

        DuelManager.GetNotIDOwner(playerID.id).DealPoisonDamage();
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

        if ((isPlayer && !BattleVars.shared.isPlayerTurn) || (!isPlayer && BattleVars.shared.isPlayerTurn))
        {
            if (playerCounters.sanctuary > 0)
            {
                return;
            }
        }
        playerQuantaManager.ChangeQuanta(element, amount, false);
    }

    public void GenerateQuantaLogic(Element element, int amount)
    {

        if ((isPlayer && !BattleVars.shared.isPlayerTurn) || (!isPlayer && BattleVars.shared.isPlayerTurn))
        {
            if (playerCounters.sanctuary > 0)
            {
                return;
            }
        }
        playerQuantaManager.ChangeQuanta(element, amount, true);
    }

    private IEnumerator SetupPassiveDisplayers()
    {
        Element markElement;
        markElement = isPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;

        Card mark = CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.markIds[(int)markElement]);
        playerPassiveManager.PlayPassive(mark);
        playerPassiveManager.PlayPassive(CardDatabase.Instance.GetPlaceholderCard(2));
        playerPassiveManager.PlayPassive(CardDatabase.Instance.GetPlaceholderCard(1));
        yield return null;
    }


    private IEnumerator SetupOtherDisplayers()
    {
        playerQuantaManager = new QuantaManager(quantaDisplayers, this);
        cardDetailManager = new CardDetailManager();
        cardDetailManager.OnDisplayNewCard += cardDetailView.SetupCardDisplay;
        cardDetailManager.OnRemoveCard += cardDetailView.CancelButtonAction;
        playerCounters = new Counters();
        playerDisplayer.SetID(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Player, 0, playerDisplayer.transform);

        List<Card> deck = new (isPlayer ?
                    PlayerData.shared.currentDeck.DeserializeCard()
                    : new List<string>(BattleVars.shared.enemyAiData.deck.Split(" ")).DeserializeCard());

        if (BattleVars.shared.enemyAiData.maxHP == 200 && !isPlayer)
        {
            deck.AddRange(deck);

        }
        deck.Shuffle();
        deckManager = new DeckManager(deck);
        deckManager.OnDeckCountChange += deckDisplayer.UpdateDeckCount;
        for (int i = 0; i < 7; i++)
        {
            DrawCardFromDeckLogic(true);
        }

        healthManager = new HealthManager(isPlayer ? 100 : BattleVars.shared.enemyAiData.maxHP, isPlayer);
        healthDisplayer.SetHPStart(healthManager.GetCurrentHealth());
        healthManager.HealthChangedEvent += healthDisplayer.OnHealthChanged;
        healthManager.MaxHealthUpdatedEvent += healthDisplayer.OnMaxHealthChanged;
        playerID.id = playerDisplayer.GetObjectID();
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
        OnPlayerCounterUpdate += playerDisplayer.UpdatePlayerIndicators;
        if (!isPlayer)
        {
            DuelManager.allPlayersSetup = true;
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
        cardDetailManager.SetCardOnDisplay(iDCardPair);
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
            if (!IsAbilityUsable(iDCardPair.card))
            {
                SetupCardDisplay(iDCardPair);
                return;
            }

            ProcessAbilityCard(iDCardPair);
        }
    }

    private void ProcessAbilityCard(IDCardPair iDCardPair)
    {
        BattleVars.shared.abilityOrigin = iDCardPair;

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
            BattleVars.shared.isSelectingTarget = true;
            SkillManager.Instance.SetupTargetHighlights(this, iDCardPair);
        }
    }

    private void ProcessSpellCard(IDCardPair iDCardPair)
    {
        BattleVars.shared.abilityOrigin = iDCardPair;
        if (!SkillManager.Instance.ShouldAskForTarget(iDCardPair))
        {
            SkillManager.Instance.SkillRoutineNoTarget(this, iDCardPair);
            PlayCardFromHandLogic(iDCardPair);
        }
        else
        {
            BattleVars.shared.isSelectingTarget = true;
            SkillManager.Instance.SetupTargetHighlights(this, iDCardPair);
        }
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