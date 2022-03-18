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

    //Logic Properties
    [SerializeField]
    private List<PlayerCardInHand> handDisplayers;
    [SerializeField]
    private List<CreatureInPlay> creatureDisplayers;
    [SerializeField]
    private List<PermanentInPlay> permanentDisplayers;
    [SerializeField]
    private List<PassiveInPlay> passiveDisplayers;
    [SerializeField]
    private List<QuantaDisplayer> quantaDisplayers;
    [SerializeField]
    private DeckDisplayer deckDisplayer;
    [SerializeField]
    private HealthDisplayer healthDisplayer;
    [SerializeField]
    private CardDetailView cardDetailView;
    [SerializeField]
    private PlayerDisplayer playerDisplayer;
    public int freedomCount;

    public TextMeshProUGUI poisonLabel;

    public HandManager playerHand;
    public CreatureManager playerCreatureField;
    public PermanentManager playerPermanentManager;

    public QuantaManager playerQuantaManager;
    public DeckManager deckManager;
    public PassiveManager playerPassiveManager;
    public HealthManager healthManager;
    public CardDetailManager cardDetailManager;
    public Counters playerCounters;

    public List<ID> deathTriggers = new List<ID>();
    private int scarabCountCurrent;
    private int scarabCountPreviousTurn;

    private int eclipseCount = 0;
    private int nightfallCount = 0;

    private bool isPlayer;


    private float animSpeed;
    public bool isHealSwapped = false;
    public int sanctuaryCount = 0;
    public int patienceCount = 0;
    private void Update()
    {
        animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
    }

    public void ClearFloodedArea(List<int> safeZones)
    {
        List<ID> creatureIDs = playerCreatureField.GetAllIds();
        List<Card> creatureCards = playerCreatureField.GetAllCards();
        for (int i = 0; i < creatureIDs.Count; i++)
        {
            if (safeZones.Contains(creatureIDs[i].Index)) { continue; }
            if (creatureCards[i].element.Equals(Element.Other)) { continue; }
            if (creatureCards[i].element.Equals(Element.Water)) { continue; }

            RemoveCardFromFieldLogic(creatureIDs[i]);
        }
    }


    public int GetScarabDiff()
    {
        return scarabCountCurrent - scarabCountPreviousTurn;
    }

    public int UpdateScarabs(bool isAdded)
    {

        if (isAdded)
        {
            scarabCountCurrent++;
        }
        else
        {
            scarabCountCurrent--;
        }
        return scarabCountCurrent;
    }


    public void UpdateEclipseNight(bool wasAdded, bool isEclipse)
    {
        List<Card> creatureCards = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();

        for (int i = 0; i < creatureCards.Count; i++)
        {
            if (creatureCards[i].element.Equals(Element.Darkness) || creatureCards[i].element.Equals(Element.Death))
            {
                int power = isEclipse ? 2 : 1;
                ModifyCreatureLogic(creatureIds[i], null, 0, wasAdded ? power : (power * -1), wasAdded ? 1 : -1);
            }
        }

        if (wasAdded)
        {
            if (isEclipse)
            {
                eclipseCount += 1;
            }
            else
            {
                nightfallCount += 1;
            }
        }
        else
        {
            if (isEclipse)
            {
                eclipseCount -= 1;
            }
            else
            {
                nightfallCount -= 1;
            }
        }
    }

    public bool HasSufficientQuanta(Element element, int cost)
    {
        return playerQuantaManager.HasEnoughQuanta(element, cost);
    }

    public List<ID> GetHandIds()
    {
        return playerHand.GetAllIds();
    }
    public List<Card> GetHandCards()
    {
        return playerHand.GetAllCards();
    }

    public int GetAllQuantaOfElement(Element element)
    {
        return playerQuantaManager.GetQuantaForElement(element);
    }

    public void ScrambleQuanta()
    {
        int total = playerQuantaManager.GetCurrentQuanta().GetIntTotal();
        
        if(total <= 9)
        {
            playerQuantaManager.SpendQuanta(Element.Other, total);
            GenerateQuantaLogic(Element.Other, total);
            return;
        }

        playerQuantaManager.SpendQuanta(Element.Other, 9);
        GenerateQuantaLogic(Element.Other, 9);
    }


    public void StartTurn()
    {
        if (deckManager.GetDeckCount() <= 0)
        {
            GameOverVisual.ShowGameOverScreen(!isPlayer);
        }
        DrawCardFromDeckLogic();
    }

    public void UpdateCounterAndEffects()
    {
        //Player Counters First
        ModifyHealthLogic(playerCounters.purity, false, false);
        ModifyHealthLogic(playerCounters.poison, true, false);
        if (playerCounters.freeze > 0)
        {
            playerCounters.freeze--;
        }

        if (playerCounters.silence > 0)
        {
            playerCounters.silence--;
        }

        if (playerCounters.delay > 0)
        {
            playerCounters.delay--;
        }
        //Creature Counters Last
        List<Card> creatureCards = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();
        for (int i = 0; i < creatureCards.Count; i++)
        {
            Card creature = creatureCards[i];
            creature.abilityUsed = false;
            creature.firstTurn = false;
            creature.cardPassives.isDiving = false;
            ID creatureID = creatureIds[i];
            creatureCards[i].turnsInPlay--;
            if (creature.cardCounters.freeze > 0)
            {
                creature.cardCounters.freeze--;
            }

            if (creature.cardCounters.charge > 0)
            {
                creature.cardCounters.charge--;
                creature.power--;
            }

            if (creature.cardCounters.delay > 0)
            {
                creature.cardCounters.delay--;
            }

            if (creature.hp <= 0 || creature.turnsInPlay <= 0)
            {
                RemoveCardFromFieldLogic(creatureID);
            }
            else
            {
                ModifyCreatureLogic(creatureID, null, 0, 0, 0, null, false, creature.cardCounters.poison);
            }
        }

        creatureCards = playerPermanentManager.GetAllCards();
        creatureIds = playerPermanentManager.GetAllIds();

        for (int i = 0; i < creatureCards.Count; i++)
        {
            creatureCards[i].firstTurn = false;
            creatureCards[i].abilityUsed = false;
            creatureCards[i].turnsInPlay--;
            if (creatureCards[i].turnsInPlay <= 0)
            {
                RemoveCardFromFieldLogic(creatureIds[i]);
            }
        }
        Card shield = playerPassiveManager.GetShield();
        if (shield != null)
        {
            shield.turnsInPlay--;
            if (shield.turnsInPlay <= 0)
            {
                RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID());
            }
        }
    }

    public int GetLightEmittingCreatures()
    {
        List<ID> allIds = GetAllIds();
        int count = 0;
        foreach (ID id in allIds)
        {
            Card card = GetCard(id);
            if (card.endTurnAbility is EndTurnBioluminescence)
            {
                count++;
            }
        }
        return count;
    }

    public void GeneratePillarPvPQuantaLogic()
    {
        List<(QuantaObject, ID)> quantaResults = playerPermanentManager.GetQuantaToGenerate();

        foreach (QuantaObject item in BattleVars.shared.opponentPvpQuanta)
        {
            GenerateQuantaLogic(item.element, item.count);
        }

        foreach ((QuantaObject, ID) item in quantaResults)
        {
            new Command_PlayActionAnimation(this, item.Item2, item.Item1.element).AddToQueue();
        }

        new Command_PlayActionAnimation(this, playerPassiveManager.GetMarkID(), playerPassiveManager.GetMark().element).AddToQueue();

    }

    public void ShouldDisplayTarget(ID item, bool shouldShow)
    {
        switch (item.Field)
        {
            case FieldEnum.Hand:
                handDisplayers[item.Index].ShouldShowTarget(shouldShow);
                break;
            case FieldEnum.Creature:
                creatureDisplayers[item.Index].ShouldShowTarget(shouldShow);
                break;
            case FieldEnum.Passive:
                passiveDisplayers[item.Index].ShouldShowTarget(shouldShow);
                break;
            case FieldEnum.Permanent:
                permanentDisplayers[item.Index].ShouldShowTarget(shouldShow);
                break;
            case FieldEnum.Player:
                playerDisplayer.ShouldShowTarget(shouldShow);
                break;
            default:
                break;
        }
    }

    public List<ID> GetAllIds()
    {
        List<ID> listToReturn = new List<ID>();
        listToReturn.AddRange(playerCreatureField.GetAllIds());
        listToReturn.AddRange(playerPassiveManager.GetAllIds());
        listToReturn.AddRange(playerPermanentManager.GetAllIds());
        //listToReturn.AddRange(playerHand.GetAllIds());
        listToReturn.Add(playerDisplayer.GetObjectID());
        return listToReturn;
    }

    public void AddCardToDeck(Card card)
    {
        deckManager.AddCardToTop(card);
        deckDisplayer.UpdateDeckCount(deckManager.GetDeckCount());
    }

    private List<QuantaObject> GetOtherElementGenerator(int count)
    {
        List<Element> elements = new List<Element>();
        for (int i = 0; i < count; i++)
        {
            elements.Add((Element)UnityEngine.Random.Range(0, 12));
        }
        List<QuantaObject> quantaObjects = new List<QuantaObject>();

        foreach (Element element in elements)
        {
            int? quantaIndex = quantaObjects.ContainsElement(element);
            if (quantaIndex != null)
            {
                quantaObjects[(int)quantaIndex].count++;
            }
            else
            {
                quantaObjects.Add(new QuantaObject(element, 1));
            }
        }
        return quantaObjects;
    }
    private List<QuantaObject> GetOtherElementToSpendGenerator(int count)
    {
        List<Element> elements = new List<Element>();
        for (int i = 0; i < 12; i++)
        {
            Element elementToAdd = (Element)i;
            if (GetAllQuantaOfElement(elementToAdd) > 0)
            {
                elements.Add(elementToAdd);
            }
        }
        List<QuantaObject> quantaObjects = new List<QuantaObject>();

        foreach (Element element in elements)
        {
            int? quantaIndex = quantaObjects.ContainsElement(element);
            if (quantaIndex != null)
            {
                quantaObjects[(int)quantaIndex].count++;
            }
            else
            {
                quantaObjects.Add(new QuantaObject(element, 1));
            }
            count--;
            if (count == 0) { break; }
        }
        return quantaObjects;
    }

    //Logic

    public void SendCreatureDamage()
    {
        if (playerCounters.delay > 0) { return; }
        if (patienceCount > 0) { return; }
        List<Card> creatureCards = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();
        if (creatureIds.Count == 0) { return; }
        for (int i = 0; i < creatureCards.Count; i++)
        {
            if (GameOverVisual.isGameOver) { return; }
            if (!(creatureCards[i].cardCounters.freeze > 0) && !(creatureCards[i].cardCounters.delay > 0))
            {
                int damageToShow = DuelManager.SendCreatureDamage(creatureCards[i]);
                if (damageToShow > 0)
                {
                    Game_SoundManager.PlayAudioClip("CreatureDamage");
                }
                creatureDisplayers[creatureIds[i].Index].ShowDamage(damageToShow);
            }
        }
    }

    public void SendWeaponDamage()
    {
        if (playerCounters.freeze > 0 || playerPassiveManager.GetWeapon() == null)
        {
            return;
        }
        Card weapon = playerPassiveManager.GetWeapon();
        if (weapon != null)
        {
            int damage = weapon.weaponAbility.GetDamageToDeal(weapon.power);
            int placeHolder = weapon.power;
            weapon.power = damage;
            int damageToShow = DuelManager.SendCreatureDamage(weapon);
            weapon.power = placeHolder;
            if (damage > 0)
            {
                Game_SoundManager.PlayAudioClip("CreatureDamage");
                passiveDisplayers[1].ShowDamage(damage);
            }
            weapon.weaponAbility.EffectAfterDamage(damageToShow, new ID(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Player, 0));
        }
    }

    public void ActivateEndTurnAbilities()
    {
        scarabCountPreviousTurn = scarabCountCurrent;
        List<ID> allIds = GetAllIds();
        allIds.RemoveAt(allIds.Count - 1);
        if (allIds.Count == 0)
        {
            return;
        }
        foreach (ID id in allIds)
        {
            if (GameOverVisual.isGameOver) { return; }
            Card card = GetCard(id);
            if (card.endTurnAbility != null)
            {
                card.endTurnAbility.ActivateAbility(id);
            }
        }
    }

    public ID PlayCardOnField(Card card)
    {
        switch (card.type)
        {
            case CardType.Pillar:
                if (!card.isUpgradable)
                {
                    GenerateQuantaLogic(card.element, 1);
                }

                return playerPermanentManager.PlayCardAtRandomLocation(card);
            case CardType.Creature:
                return playerCreatureField.PlayCardAtRandomLocation(card);
            case CardType.Spell:
                return null;
            case CardType.Artifact:
                return playerPermanentManager.PlayCardAtRandomLocation(card);
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


    public void ManageID(ID iD)
    {
        if (!BattleVars.shared.isPlayerTurn) { return; }

        if (iD.Owner.Equals(OwnerEnum.Opponent))
        {
            if (iD.Field.Equals(FieldEnum.Hand)) { return; }
            cardDetailView.SetupCardDisplay(iD, DuelManager.GetOpponentCard(iD), false);
            cardDetailManager.SetCardOnDisplay(iD);
            return;
        }

        if (BattleVars.shared.hasToDiscard)
        {
            if (iD.Field.Equals(FieldEnum.Hand))
            {
                DiscardCard(iD);
                BattleVars.shared.hasToDiscard = false;
                DuelManager.discardTextStatic.gameObject.SetActive(false);
                DuelManager.EndTurn();
                return;
            }
        }

        Card card = GetCard(iD);
        if (PlayerPrefs.GetInt("QuickPlay") == 0)
        {
            if (iD.Field.Equals(FieldEnum.Hand) && playerQuantaManager.HasEnoughQuanta(card.element, card.cost))
            {
                if (playerCounters.silence > 0) { return; }
                if (!card.type.Equals(CardType.Spell))
                {
                    PlayCardFromHandLogic(iD);
                }
                else
                {
                    BattleVars.shared.originId = iD;
                    BattleVars.shared.spellOnStandBy = card.spellAbility;
                    if (card.spellAbility.isTargetFixed)
                    {
                        ActivateAbility(iD);
                    }
                    else
                    {
                        BattleVars.shared.isSelectingTarget = true;
                        DuelManager.SetupHighlight(BattleVars.shared.spellOnStandBy, BattleVars.shared.abilityOnStandBy);
                    }
                }
            }
            else if ((iD.Field.Equals(FieldEnum.Creature) || iD.Field.Equals(FieldEnum.Passive) || iD.Field.Equals(FieldEnum.Permanent)) && IsAbilityUsable(card))
            {
                BattleVars.shared.originId = iD;
                BattleVars.shared.abilityOnStandBy = card.activeAbility;
                if (card.activeAbility.ShouldSelectTarget)
                {
                    BattleVars.shared.isSelectingTarget = true;
                    DuelManager.SetupHighlight(BattleVars.shared.spellOnStandBy, BattleVars.shared.abilityOnStandBy);
                }
                else
                {
                    ActivateAbility(iD);
                }

            }

            return;
        }


        cardDetailView.SetupCardDisplay(iD, GetCard(iD), playerQuantaManager.HasEnoughQuanta(card.element, card.cost));
        cardDetailManager.SetCardOnDisplay(iD);
    }

    internal void DisplayHand()
    {
        List<Card> cards = playerHand.GetAllCards();

        for (int i = 0; i < cards.Count; i++)
        {
            handDisplayers[i].ShowCardForPrecog(cards[i]);
        }
    }

    public Card GetCard(ID iD)
    {
        return iD.Field switch
        {
            FieldEnum.Hand => playerHand.GetCardWithID(iD),
            FieldEnum.Creature => playerCreatureField.GetCardWithID(iD),
            FieldEnum.Passive => playerPassiveManager.GetCardWithID(iD),
            FieldEnum.Permanent => playerPermanentManager.GetCardWithID(iD),
            _ => null,
        };
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
                DuelManager.SetupHighlight(BattleVars.shared.spellOnStandBy, BattleVars.shared.abilityOnStandBy);
                break;
            default:
                cardDetailManager.ClearID();
                break;
        }
    }

    public void ActivateAbility(ID iD)
    {
        if (BattleVars.shared.spellOnStandBy != null)
        {
            BattleVars.shared.spellOnStandBy.ActivateAbility(iD);

            ActionManager.AddCardPlayedAction(isPlayer, GetCard(BattleVars.shared.originId));
            PlayCardFromHandLogic(BattleVars.shared.originId);
        }
        else if (BattleVars.shared.abilityOnStandBy != null)
        {
            if (!nameof(BattleVars.shared.abilityOnStandBy).Equals("ActiveAPhotosynthesis"))
            {
                GetCard(BattleVars.shared.originId).abilityUsed = true;
            }
            BattleVars.shared.abilityOnStandBy.ActivateAbility(iD);
            SpendQuantaLogic(BattleVars.shared.abilityOnStandBy.AbilityElement, BattleVars.shared.abilityOnStandBy.AbilityCost);
        }
        BattleVars.shared.abilityOnStandBy = null;
        BattleVars.shared.spellOnStandBy = null;
        BattleVars.shared.isSelectingTarget = false;
        BattleVars.shared.originId = null;
        DisplayPlayableGlow();
    }


    public int ReceiveCreatureDamage(Card creature)
    {
        //Get Shield in play
        Card shield = playerPassiveManager.GetShield();
        //Get list of creatures with gravity
        List<ID> creaturesWithGravity = playerCreatureField.GetCreaturesWithGravity();

        //Check if shield is valid and attacking creature does not have momentum
        if (shield != null && !creature.cardPassives.hasMomentum)
        {
            if (playerCounters.bone > 0)
            {
                playerCounters.bone--;
                if (playerCounters.bone == 0)
                {
                    RemoveCardFromFieldLogic(new ID(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Passive, 2));
                }
                return 0;
            }

            //Get damage after shield ability
            int creatureDamage = creature.cardPassives.isDiving ? creature.power * 2 : creature.power;
            if (DuelManager.GetIDOwner(new ID(isPlayer ? OwnerEnum.Opponent : OwnerEnum.Player, FieldEnum.Player, 0)).freedomCount > 0)
            {
                bool shouldIncrease = UnityEngine.Random.Range(0, 100) > 50 && creature.element.Equals(Element.Air);
                creatureDamage += shouldIncrease ? Mathf.FloorToInt(creature.power) : 0;
            }

            int damage = shield.shieldAbility.GetDamageToDeal(creatureDamage, creature.hp, creature.cardPassives.hasReach);

            //Loop damaging creatures with gravity 
            while (creaturesWithGravity.Count > 0 && damage > 0)
            {
                //Damage Creature and get leftover damage
                (Card, int) result = playerCreatureField.DamageCreature(creature.power, creaturesWithGravity[0]);

                if (result.Item2 < 0)
                {
                    //Update surviving creature display;
                    new Command_UpdateCardOnField(this, result.Item1, creaturesWithGravity[0]).AddToQueue();
                    return 0;
                }
                else
                {
                    //Remove Creature display
                    RemoveCardFromFieldLogic(creaturesWithGravity[0]);
                }
                //Update List
                creaturesWithGravity = playerCreatureField.GetCreaturesWithGravity();
                //Subtract leftover damage
                damage -= result.Item2;
            }
            //Damage player with leftover damage or direct damage
            ModifyHealthLogic(damage > 0 ? damage : 0, true, creature.cardPassives.isPsion);
            return damage > 0 ? damage : 0;
        }
        //Cache damage to deal
        int damageToDeal = creature.cardPassives.isDiving ? creature.power * 2 : creature.power;

        if (DuelManager.GetIDOwner(new ID(isPlayer ? OwnerEnum.Opponent : OwnerEnum.Player, FieldEnum.Player, 0)).freedomCount > 0)
        {
            bool shouldIncrease = UnityEngine.Random.Range(0, 100) > 50 && creature.element.Equals(Element.Air);
            damageToDeal += shouldIncrease ? Mathf.FloorToInt(creature.power) : 0;
        }
        //Loop through creatures with gravity
        while (creaturesWithGravity.Count > 0 && damageToDeal > 0)
        {
            //Damage Creature and get leftover damage
            (Card, int) result = playerCreatureField.DamageCreature(creature.power, creaturesWithGravity[0]);
            if (result.Item2 < 0)
            {
                //Update surviving creature display;
                new Command_UpdateCardOnField(this, result.Item1, creaturesWithGravity[0]).AddToQueue();
                return 0;

            }
            else
            {
                //Remove Creature display
                RemoveCardFromFieldLogic(creaturesWithGravity[0]);
            }
            //Update List
            creaturesWithGravity = playerCreatureField.GetCreaturesWithGravity();
            //Subtract leftover damage
            damageToDeal -= result.Item2;
        }
        //Damage player with leftover damage or direct damage
        ModifyHealthLogic(damageToDeal > 0 ? damageToDeal : 0, true, creature.cardPassives.isPsion);
        return damageToDeal > 0 ? damageToDeal : 0;
    }



    public bool IsCardPlayable(Card cardToCheck)
    {
        bool canAfford = playerQuantaManager.HasEnoughQuanta(cardToCheck.element, cardToCheck.cost);
        bool hasSpace = true;

        switch (cardToCheck.type)
        {
            case CardType.Pillar:
                hasSpace = playerPermanentManager.GetAllCards().Count < permanentDisplayers.Count;
                break;
            case CardType.Creature:
                hasSpace = playerCreatureField.GetAllCards().Count < creatureDisplayers.Count;
                break;
            case CardType.Artifact:
                hasSpace = playerPermanentManager.GetAllCards().Count < permanentDisplayers.Count;
                break;
            default:
                break;
        }

        return canAfford && hasSpace;
    }

    public bool IsAbilityUsable(Card cardToCheck)
    {
        if (cardToCheck.activeAbility == null) { return false; }
        bool canAfford = playerQuantaManager.HasEnoughQuanta(cardToCheck.activeAbility.AbilityElement, cardToCheck.activeAbility.AbilityCost);

        return canAfford && !cardToCheck.abilityUsed && !cardToCheck.firstTurn;
    }


    //Command Methods


    //Remove Card from field Logic and Visual Command Pair
    public void RemoveCardFromFieldLogic(ID cardID, int amount = 1)
    {
        switch (cardID.Field)
        {
            case FieldEnum.Hand:
                playerHand.PlayCardWithID(cardID);
                break;
            case FieldEnum.Creature:
                if (GetCard(cardID).cardPassives.hasAflatoxin)
                {
                    PlayCardOnFieldLogic(CardDatabase.GetCardFromResources("Malignant Cell", "Creature", true));
                }
                CheckOnPlayAbility(GetCard(cardID), cardID, false);
                playerCreatureField.DestroyCard(cardID.Index);
                DuelManager.ActivateDeathTriggers();

                break;
            case FieldEnum.Passive:
                playerPassiveManager.DestroyCard(cardID.Index);
                break;
            case FieldEnum.Permanent:
                for (int i = 0; i < amount; i++)
                {
                    playerPermanentManager.DestroyCard(cardID.Index);
                }
                break;
            default:
                break;
        }
        void Effect() => new Command_RemoveCardFromField(this, cardID).AddToQueue();
        Game_AnimationManager.PlayAnimation("CardDeath", Battlefield_ObjectIDManager.shared.GetObjectFromID(cardID), Effect);

    }

    public void FillHandWith(Card newCard)
    {
        int amountToAdd = 7 - playerHand.GetAllCards().Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            deckManager.AddCardToTop(CardDatabase.GetCardFromResources(newCard.name, newCard.type.FastCardTypeString(), !newCard.isUpgradable));
            DrawCardFromDeckLogic();
        }
    }

    public async void RemoveCardFromFieldVisual(ID cardID)
    {
        switch (cardID.Field)
        {
            case FieldEnum.Creature:
                creatureDisplayers[cardID.Index].PlayDissolveAnimation();
                break;
            case FieldEnum.Passive:
                passiveDisplayers[cardID.Index].PlayDissolveAnimation();
                break;
            case FieldEnum.Permanent:
                int newStack = playerPermanentManager.GetStackAt(cardID.Index);
                if (newStack > 0)
                {
                    permanentDisplayers[cardID.Index].ChangeStackCount(newStack);
                }
                else
                {
                    permanentDisplayers[cardID.Index].PlayDissolveAnimation();
                }
                break;
            default:
                break;
        }
        Game_SoundManager.PlayAudioClip("RemoveCardFromField");
        await new WaitForSeconds(animSpeed);
        Command.CommandExecutionComplete();
    }


    //Modify Health Logic and Visual Command Pair
    public void ModifyHealthLogic(int amount, bool isDamage, bool fromSpell)
    {
        if (isHealSwapped) { isDamage = !isDamage; }
        if (isDamage && fromSpell)
        {
            Card shield = playerPassiveManager.GetShield();
            if (shield != null)
            {
                if (shield.cardPassives.isReflect)
                {
                    DuelManager.GetIDOwner(new ID(isPlayer ? OwnerEnum.Opponent : OwnerEnum.Player, FieldEnum.Player, 0)).ModifyHealthLogic(amount, isDamage, false);
                    return;
                }
            }
        }
        int newHealth = healthManager.ModifyHealth(amount, isDamage);
        new Command_UpdateHealth(this, newHealth).AddToQueue();

    }

    public void ModifyHealthVisual(int amount)
    {
        healthDisplayer.UpdateHPView(amount, isPlayer);
        Command.CommandExecutionComplete();
    }

    public void ApplyPlayerCounterLogic(CounterEnum counterEnum, int amount)
    {
        switch (counterEnum)
        {
            case CounterEnum.Freeze:
                playerCounters.freeze += amount;
                break;
            case CounterEnum.Poison:
                poisonLabel.transform.parent.gameObject.SetActive(true);
                playerCounters.poison += amount;
                poisonLabel.text = playerCounters.poison.ToString();
                break;
            case CounterEnum.Purify:
                playerCounters.purity += amount;
                playerCounters.poison = 0;
                poisonLabel.transform.parent.gameObject.SetActive(false);
                poisonLabel.text = playerCounters.poison.ToString();
                break;
            case CounterEnum.Bone:
                playerCounters.bone += amount;
                break;
            case CounterEnum.Silence:
                playerCounters.silence += amount;
                break;
            case CounterEnum.Delay:
                playerCounters.delay += amount;
                break;
            default:
                break;
        }
    }

    //Update Hand Logic and Visual Command Pair
    public void UpdateHandLogic()
    {
        //Update Hand Information
        playerHand.UpdateHand();
        new Command_UpdateHand(this).AddToQueue();
    }

    public async void UpdateHandVisual()
    {
        //Update Hand To Remove Empty Space
        foreach (var item in handDisplayers)
        {
            item.transform.parent.gameObject.SetActive(false);
        }
        List<Card> cardList = playerHand.GetAllCards();
        for (int i = 0; i < cardList.Count; i++)
        {
            handDisplayers[i].DisplayCard(cardList[i], true);
        }
        DisplayPlayableGlow();
        await new WaitForSeconds(animSpeed);
        Command.CommandExecutionComplete();
    }



    private void DisplayNewCard(ID cardID, Card cardPlayed, bool shouldAnim = true)
    {
        switch (cardID.Field)
        {
            case FieldEnum.Hand:
                handDisplayers[cardID.Index].DisplayCard(cardPlayed, !shouldAnim);
                break;
            case FieldEnum.Creature:
                creatureDisplayers[cardID.Index].DisplayCard(cardPlayed, shouldAnim);
                break;
            case FieldEnum.Passive:
                passiveDisplayers[cardID.Index].DisplayCard(cardPlayed, shouldAnim);
                break;
            case FieldEnum.Permanent:
                permanentDisplayers[cardID.Index].DisplayCard(cardPlayed);
                break;
            default:
                break;
        }
    }

    public void PlayCardFromHandLogicWithLocation(ID originId, ID targetId)
    {
        Card card = GetCard(originId);
        switch (card.type)
        {
            case CardType.Pillar:
                if (!card.isUpgradable)
                {
                    GenerateQuantaLogic(card.element, 1);
                }
                playerPermanentManager.PlayCardAtLocation(card, targetId);
                break;
            case CardType.Creature:
                playerCreatureField.PlayCardAtLocation(card, targetId);
                break;
            case CardType.Artifact:
                playerPermanentManager.PlayCardAtLocation(card, targetId);
                break;
            case CardType.Weapon:
                playerPassiveManager.PlayCardAtLocation(card, targetId);
                break;
            case CardType.Shield:
                playerPassiveManager.PlayCardAtLocation(card, targetId);
                break;
            case CardType.Mark:
                playerPassiveManager.PlayCardAtLocation(card, targetId);
                break;
            default:
                break;
        }
        ActionManager.AddCardPlayedAction(isPlayer, card);
        new Command_PlayCardOnField(this, card, targetId).AddToQueue();
        //Remove Card From Hand
        playerHand.PlayCardWithID(originId);
        //Spend Quanta
        playerQuantaManager.SpendQuanta(card.element, card.cost);
        new Command_PlayCardFromHand(this, originId).AddToQueue();
    }

    //Play Card From Hand Logic and Visual Command Pair
    public void PlayCardFromHandLogic(ID cardID)
    {
        //Logic Side
        //Get Card SO In Hand
        Card cardPlayed = playerHand.GetCardWithID(cardID);
        //Play Card on field if it is not a spell
        if (!cardPlayed.type.Equals(CardType.Spell))
        {
            ID isNull = PlayCardOnFieldLogic(cardPlayed);
            if (isNull == null)
            {
                return;
            }
            ActionManager.AddCardPlayedAction(isPlayer, cardPlayed);
            if (BattleVars.shared.isPvp)
            {
                //Game_PvpHubConnection.shared.SendPvpAction(new PvP_Action(ActionType.PlayCardToField, cardID, isNull));
            }
        }
        //Remove Card From Hand
        playerHand.PlayCardWithID(cardID);
        //Spend Quanta
        playerQuantaManager.SpendQuanta(cardPlayed.element, cardPlayed.cost);
        new Command_PlayCardFromHand(this, cardID).AddToQueue();
    }

    public async void PlayCardFromHandVisual(ID cardID)
    {
        //Visual Side
        //Play Hand Card Dissolve Animation
        handDisplayers[cardID.Index].PlayDissolveAnimation();
        //Update Quanta Pool
        List<int> currentQuanta = playerQuantaManager.GetCurrentQuanta();
        for (int i = 0; i < quantaDisplayers.Count; i++)
        {
            quantaDisplayers[i].SetNewQuantaAmount(currentQuanta[i].ToString());
        }
        Game_SoundManager.PlayAudioClip("CardPlay");
        await new WaitForSeconds(animSpeed);
        DisplayPlayableGlow();
        Command.CommandExecutionComplete();
    }

    internal void ModifyMaxHealthLogic(int maxHPBuff, bool isIncrease)
    {
        int newMax = healthManager.ModifyMaxHealth(maxHPBuff, isIncrease);
        healthDisplayer.UpdateMaxHPView(newMax);
    }

    //Draw Card From Deck Logic and Visual Command Pair
    public void DrawCardFromDeckLogic(bool isInitialDraw = false)
    {
        if (playerHand == null) { return; }
        if (playerHand.GetAllCards().Count == 8) { return; }
        //Logic Side
        //Get Card From Deck
        Card newCard = deckManager.DrawCard();
        //Add Card To Hand
        ID cardId = playerHand.AddCardToHand(newCard);
        if (!isInitialDraw)
        {
            ActionManager.AddCardDrawAction(isPlayer, newCard);
        }
        new Command_DrawCardFromDeck(this, newCard, cardId).AddToQueue();
    }

    public async void DrawCardFromDeckVisual(Card newCard, ID cardId)
    {
        //Visual Side
        //Update Deck Count
        deckDisplayer.UpdateDeckCount(deckManager.GetDeckCount());
        //Play Hand Card Animation
        DisplayNewCard(cardId, newCard);
        await new WaitForSeconds(animSpeed);
        DisplayPlayableGlow();
        Command.CommandExecutionComplete();
    }

    void HideAllPlayableGlow()
    {
        foreach (CardDisplayer displayer in handDisplayers)
        {
            displayer.ShouldShowTarget(false);
        }

        foreach (CardDisplayer displayer in creatureDisplayers)
        {
            displayer.ShouldShowUsableGlow(false);
        }

        foreach (CardDisplayer displayer in permanentDisplayers)
        {
            displayer.ShouldShowUsableGlow(false);
        }
    }

    void DisplayPlayableGlow()
    {
        HideAllPlayableGlow();
        if (!BattleVars.shared.isPlayerTurn) { return; }
        List<Card> cards = new List<Card>(playerHand.GetAllCards());
        List<ID> cardIds = new List<ID>(playerHand.GetAllIds());

        if (cards?.Any() ?? false)
        {
            for (int i = 0; i < cardIds.Count; i++)
            {
                if (IsCardPlayable(cards[i]))
                {
                    handDisplayers[cardIds[i].Index].ShouldShowTarget(IsCardPlayable(cards[i]));
                }
            }

        }

        cards = new List<Card>(playerCreatureField.GetAllCards());
        cardIds = new List<ID>(playerCreatureField.GetAllIds());

        if (cards?.Any() ?? false)
        {
            for (int i = 0; i < cardIds.Count; i++)
            {
                creatureDisplayers[cardIds[i].Index].ShouldShowUsableGlow(IsAbilityUsable(cards[i]));
            }

        }

        cards = new List<Card>(playerPermanentManager.GetAllCards());
        cardIds = new List<ID>(playerPermanentManager.GetAllIds());

        if (cards?.Any() ?? false)
        {
            for (int i = 0; i < cardIds.Count; i++)
            {
                creatureDisplayers[cardIds[i].Index].ShouldShowUsableGlow(IsAbilityUsable(cards[i]));
            }
        }
    }

    //Modify Cards on the field
    public void ModifyCreatureLogic(ID target, CounterEnum? counter = null, int countAmount = 0, int modifyPower = 0, int modifyHP = 0, PassiveEnum? passiveToChange = null, bool newPassiveValue = false, int damage = 0)
    {
        if (counter != null)
        {
            playerCreatureField.ApplyCounter((CounterEnum)counter, countAmount, target);

        }

        if (modifyHP != 0 || modifyPower != 0)
        {
            playerCreatureField.ModifyPowerHP(modifyPower, modifyHP, target);
        }

        if (passiveToChange != null)
        {
            playerCreatureField.ChangePassiveAbility((PassiveEnum)passiveToChange, newPassiveValue, target);
        }

        if (damage != 0)
        {
            playerCreatureField.DamageCreature(damage, target);
        }

        Card creature = playerCreatureField.GetCardWithID(target);
        if (creature == null)
        {
            RemoveCardFromFieldLogic(target);
            return;
        }

        if (creature.hp <= 0)
        {
            RemoveCardFromFieldLogic(target);
            return;
        }

        new Command_UpdateCardOnField(this, creature, target).AddToQueue();
    }

    public void ModifyPermanentLogic(ID target, PassiveEnum passiveToChange, bool newPassiveValue)
    {
        if (target.Field.Equals(FieldEnum.Passive))
        {
            playerPassiveManager.ChangePassiveAbility(passiveToChange, newPassiveValue, target);
            Card passive = playerPassiveManager.GetCardWithID(target);
            new Command_UpdateCardOnField(this, passive, target).AddToQueue();
        }
        else
        {
            playerPermanentManager.ChangePassiveAbility(passiveToChange, newPassiveValue, target);
            Card passive = playerPassiveManager.GetCardWithID(target);
            new Command_UpdateCardOnField(this, passive, target).AddToQueue();
        }
    }

    public async void UpdateCardOnFieldVisual(Card card, ID cardID)
    {
        DisplayNewCard(cardID, card, false);
        await new WaitForSeconds(animSpeed);
        Command.CommandExecutionComplete();
    }

    public void ActivateDeathTriggers()
    {
        if (deathTriggers.Count == 0) { return; }
        foreach (ID triggerID in deathTriggers)
        {
            Card cardToTrigger = GetCard(triggerID);
            if (cardToTrigger.onDeathAbility == null) { return; }
            cardToTrigger.onDeathAbility.ActivateAction(triggerID);
        }
    }

    private void CheckOnPlayAbility(Card card, ID location, bool isPlayed)
    {
        if (card.onPlayAbility == null) { return; }
        if (isPlayed)
        {
            card.onPlayAbility.ActiveActionWhenPlayed(location);
            return;
        }
        card.onPlayAbility.ActiveActionWhenDestroyed(location);
    }

    private void CheckEclipseNightfall(ID card)
    {
        (int, int) modifyPwrHP = (0, 0);
        modifyPwrHP.Item1 += nightfallCount;
        modifyPwrHP.Item1 += eclipseCount * 2;
        modifyPwrHP.Item2 += nightfallCount;
        modifyPwrHP.Item2 += eclipseCount;

        ModifyCreatureLogic(card, null, 0, modifyPwrHP.Item1, modifyPwrHP.Item2);
    }

    //Play Card from anywhere Logic and Visual
    public ID PlayCardOnFieldLogic(Card card)
    {
        ID newLocationId = PlayCardOnField(card);
        if (newLocationId == null) { return null; }
        CheckOnPlayAbility(card, newLocationId, true);
        if ((card.element.Equals(Element.Darkness) || card.element.Equals(Element.Death)) && card.type.Equals(CardType.Creature))
        {
            CheckEclipseNightfall(newLocationId);
        }
        new Command_PlayCardOnField(this, card, newLocationId).AddToQueue();
        return newLocationId;
    }
    public async void PlayCardOnFieldVisual(ID cardId, Card newCard)
    {
        DisplayNewCard(cardId, newCard);
        await new WaitForSeconds(animSpeed);
        Command.CommandExecutionComplete();
    }


    public void DiscardCard(ID cardToDiscard)
    {
        playerHand.PlayCardWithID(cardToDiscard);
    }

    public List<QuantaObject> GeneratePillarQuantaLogic()
    {
        List<(QuantaObject, ID)> quantaResults = playerPermanentManager.GetQuantaToGenerate();

        List<QuantaObject> ifPvpToSend = new List<QuantaObject>();

        if (BattleVars.shared.isPvp && isPlayer)
        {
            foreach ((QuantaObject, ID) item in quantaResults)
            {
                if (!item.Item1.element.Equals(Element.Other))
                {
                    ifPvpToSend.Add(item.Item1);
                }
            }
        }

        foreach ((QuantaObject, ID) item in quantaResults)
        {
            new Command_PlayActionAnimation(this, item.Item2, item.Item1.element).AddToQueue();
            if (item.Item1.element.Equals(Element.Other))
            {
                List<QuantaObject> rndQuantas = GetOtherElementGenerator(item.Item1.count * 3);
                foreach (QuantaObject rnd in rndQuantas)
                {
                    int? index = ifPvpToSend.ContainsElement(rnd.element);

                    if (index == null)
                    {
                        ifPvpToSend.Add(rnd);
                    }
                    else
                    {
                        ifPvpToSend[(int)index].count += rnd.count;
                    }
                    GenerateQuantaLogic(rnd.element, rnd.count);
                }
            }
            else
            {
                GenerateQuantaLogic(item.Item1.element, item.Item1.count);
            }
        }

        new Command_PlayActionAnimation(this, playerPassiveManager.GetMarkID(), playerPassiveManager.GetMark().element).AddToQueue();

        if (!BattleVars.shared.isPvp && !isPlayer)
        {
            if (BattleVars.shared.enemyAiData.maxHP >= 150)
            {
                GenerateQuantaLogic(playerPassiveManager.GetMark().element, 3);
            }
            else
            {
                GenerateQuantaLogic(playerPassiveManager.GetMark().element, 1);
            }
        }
        else
        {
            int? index = ifPvpToSend.ContainsElement(playerPassiveManager.GetMark().element);

            if (index == null)
            {
                ifPvpToSend.Add(new QuantaObject(playerPassiveManager.GetMark().element, 1));
            }
            else
            {
                ifPvpToSend[(int)index].count += 1;
            }
            GenerateQuantaLogic(playerPassiveManager.GetMark().element, 1);
        }

        return ifPvpToSend;
    }

    public void SpendQuantaLogic(Element element, int amount)
    {

        if (isPlayer && BattleVars.shared.isPlayerTurn || !isPlayer && !BattleVars.shared.isPlayerTurn)
        {
            if (sanctuaryCount > 0)
            {
                return;
            }
        }
        if (element.Equals(Element.Other))
        {
            int oTotal = 0;
            List<QuantaObject> quantaObjects = GetOtherElementToSpendGenerator(amount);
            foreach (QuantaObject quantaObject in quantaObjects)
            {
                playerQuantaManager.SpendQuanta(quantaObject.element, quantaObject.count);
                oTotal = playerQuantaManager.GetQuantaForElement(quantaObject.element);
                new Command_UpdateQuantaPoolElement(this, (int)quantaObject.element, oTotal).AddToQueue();
            }
            return;
        }
        playerQuantaManager.SpendQuanta(element, amount);
        int total = playerQuantaManager.GetQuantaForElement(element);
        new Command_UpdateQuantaPoolElement(this, (int)element, total).AddToQueue();
    }

    public void GenerateQuantaLogic(Element element, int amount)
    {
        playerQuantaManager.AddQuanta(element, amount);
        int total = playerQuantaManager.GetQuantaForElement(element);
        new Command_UpdateQuantaPoolElement(this, (int)element, total).AddToQueue();
    }

    public async void UpdateQuantaPoolVisual(int element, int newTotal)
    {
        quantaDisplayers[element].SetNewQuantaAmount(newTotal.ToString());
        await new WaitForSeconds(0.1f);
        Command.CommandExecutionComplete();
    }


    public async void PlayActionAnimationVisual(ID cardID)
    {
        await new WaitForSeconds(animSpeed);
        Command.CommandExecutionComplete();
    }


    //Setup Methods
    private void SetupCreatureDisplayers()
    {
        foreach (CreatureInPlay item in creatureDisplayers)
        {
            item.SetupDisplayer(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Creature);
        }

        List<ID> idCreatureList = new List<ID>();
        for (int i = 0; i < creatureDisplayers.Count; i++)
        {
            idCreatureList.Add(creatureDisplayers[i].GetObjectID());
        }

        playerCreatureField = new CreatureManager(idCreatureList);
    }

    private void SetupHandDisplayers()
    {

        List<ID> idHandList = new List<ID>();
        for (int i = 0; i < handDisplayers.Count; i++)
        {
            handDisplayers[i].SetupDisplayer(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Hand);
            idHandList.Add(handDisplayers[i].GetObjectID());
        }

        playerHand = new HandManager(idHandList);
    }

    private void SetupPermanentDisplayers()
    {
        foreach (PermanentInPlay item in permanentDisplayers)
        {
            item.SetupDisplayer(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Permanent);
        }

        List<ID> idPermanentList = new List<ID>();
        for (int i = 0; i < permanentDisplayers.Count; i++)
        {
            idPermanentList.Add(permanentDisplayers[i].GetObjectID());
        }

        playerPermanentManager = new PermanentManager(idPermanentList);
    }

    private void SetupPassiveDisplayers()
    {
        List<ID> idPassiveList = new List<ID>();
        for (int i = 0; i < passiveDisplayers.Count; i++)
        {
            passiveDisplayers[i].SetupDisplayer(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Passive);
            idPassiveList.Add(passiveDisplayers[i].GetObjectID());
        }
        playerPassiveManager = new PassiveManager(idPassiveList);
        Element markElement;
        if (BattleVars.shared.isPvp)
        {
            markElement = isPlayer ? PlayerData.shared.markElement : Element.Aether;//Game_PvpHubConnection.shared.GetOpponentMark();
        }
        else
        {
            markElement = isPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;
        }
        Card mark = CardDatabase.GetCardFromResources($"Mark of {markElement}", "Mark", false);
        playerPassiveManager.PlayPassive(mark);
        passiveDisplayers[0].DisplayCard(mark, true);
    }


    private void SetupOtherDisplayers()
    {
        playerQuantaManager = new QuantaManager();
        cardDetailManager = new CardDetailManager();
        playerCounters = new Counters();
        playerDisplayer.SetID(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Player, 0, playerDisplayer.transform);

        if (!BattleVars.shared.isPvp)
        {
            List<Card> deck = new List<Card>(isPlayer ? PlayerData.shared.currentDeck.DeserializeCard() : BattleVars.shared.enemyAiData.deck);
            if (BattleVars.shared.enemyAiData.maxHP == 200 && !isPlayer)
            {
                deck.AddRange(new List<Card>(BattleVars.shared.enemyAiData.deck));

            }
            deck.Shuffle();
            deckManager = new DeckManager(deck);
            deckDisplayer.UpdateDeckCount(deckManager.GetDeckCount());
            for (int i = 0; i < 7; i++)
            {
                DrawCardFromDeckLogic(true);
            }

            healthManager = new HealthManager(isPlayer ? 100 : BattleVars.shared.enemyAiData.maxHP);
            healthDisplayer.SetHPStart(healthManager.GetCurrentHealth());

            if (isPlayer && PlayerData.shared.petName != "" && PlayerData.shared.petName != null)
            {
                Card petCard = CardDatabase.GetCardFromResources(PlayerData.shared.petName, "Creature", false);
                PlayerData.shared.petName = "";
                PlayCardOnFieldLogic(petCard);
            }

            if (!isPlayer)
            {
                DuelManager.allPlayersSetup = true;
            }
            return;
        }
        else
        {
            List<Card> deck = new List<Card>(isPlayer ? PlayerData.shared.currentDeck.DeserializeCard() : DuelManager.opponentShuffledDeck.DeserializeCard());
            deckManager = new DeckManager(deck);

            deckDisplayer.UpdateDeckCount(deckManager.GetDeckCount());

            for (int i = 0; i < 7; i++)
            {
                DrawCardFromDeckLogic(true);
            }

            healthManager = new HealthManager(100);
            healthDisplayer.SetHPStart(healthManager.GetCurrentHealth());
        }

        if (!isPlayer)
        {
            DuelManager.canSetupOpDeck = true;
        }
    }
    public void SetupPlayerManager(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        SetupCreatureDisplayers();
        SetupHandDisplayers();
        SetupPassiveDisplayers();
        SetupPermanentDisplayers();
        SetupOtherDisplayers();
    }

    public void SetPvPDeck(List<CardObject> opponentShuffledDeck)
    {
        List<Card> deck = new List<Card>(opponentShuffledDeck.DeserializeCard());
        deckManager = new DeckManager(deck);
        deckDisplayer.UpdateDeckCount(deckManager.GetDeckCount());

        for (int i = 0; i < 7; i++)
        {
            DrawCardFromDeckLogic(true);
        }

        DuelManager.allPlayersSetup = true;

    }
}
