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

    public List<CreatureInPlay> creatureDisplayers;

    public List<PermanentInPlay> permanentDisplayers;

    public List<PassiveInPlay> passiveDisplayers;
    [SerializeField]
    private List<QuantaDisplayer> quantaDisplayers;
    [SerializeField]
    private DeckDisplayer deckDisplayer;

    public HealthDisplayer healthDisplayer;
    [SerializeField]
    private GameObject cloakVisual;
    private int scarabsPlayed = 0;

    public int GetPossibleDamage()
    {
        int value = 0;
        List<Card> creatures = playerCreatureField.GetAllCards();

        foreach (var item in creatures)
        {
            value += item.AtkNow;
        }
        Card weapon = playerPassiveManager.GetWeapon();
        if (weapon != null)
        {
            if (weapon.cardName != "Weapon") { value += weapon.AtkNow; }
        }
        return value;
    }

    [SerializeField]
    private CardDetailView cardDetailView;

    public PlayerDisplayer playerDisplayer;
    public int freedomCount;

    public TextMeshProUGUI poisonLabel, boneShieldLabel, purityLabel;

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


    public bool isPlayer;


    private float animSpeed;
    public int sanctuaryCount = 0;
    public int patienceCount = 0;
    private void Update()
    {
        animSpeed = PlayerPrefs.GetFloat("AnimSpeed");
    }

    public IEnumerator ClearFloodedArea(List<int> safeZones)
    {
        if (DuelManager.floodCount > 0)
        {
            List<ID> creatureIDs = playerCreatureField.GetAllIds();
            List<Card> creatureCards = playerCreatureField.GetAllCards();
            for (int i = 0; i < creatureIDs.Count; i++)
            {
                if (safeZones.Contains(creatureIDs[i].Index)) { continue; }
                if (creatureCards[i].costElement.Equals(Element.Other)) { continue; }
                if (creatureCards[i].costElement.Equals(Element.Water)) { continue; }
                if (creatureCards[i].innate.Contains("immaterial")) { continue; }
                if (creatureCards[i].innate.Contains("burrow")) { continue; }

                yield return StartCoroutine(RemoveCardFromFieldLogic(creatureIDs[i]));
            }
        }
    }


    public int GetScarabCount()
    {
        List<Card> creatures = playerCreatureField.GetAllCards();
        int scarabCount = 0;

        foreach (var creature in creatures)
        {
            if (creature.cardName.Equals("Scarab") || creature.cardName.Equals("Elite Scarab"))
            {
                scarabCount++;
            }
        }

        return scarabCount;
    }


    public void UpdateEclipseNight(int atk, int hp)
    {
        List<Card> creatureCards = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();

        for (int i = 0; i < creatureCards.Count; i++)
        {
            if (creatureCards[i].costElement.Equals(Element.Darkness) || creatureCards[i].costElement.Equals(Element.Death))
            {
                ModifyCreatureLogic(creatureIds[i], null, 0, atk, hp);
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

    public IEnumerator ScrambleQuanta()
    {
        int total = playerQuantaManager.GetCurrentQuanta().GetIntTotal();

        if (total <= 9)
        {
            yield return StartCoroutine(SpendQuantaLogic(Element.Other, total));

            yield return StartCoroutine(GenerateQuantaLogic(Element.Other, total));

            yield break;
        }

        yield return StartCoroutine(SpendQuantaLogic(Element.Other, 9));
        yield return StartCoroutine(GenerateQuantaLogic(Element.Other, 9));
    }

    private IEnumerator TurnDownTick()
    {
        if(playerPassiveManager.GetShield().iD == "7n8" || playerPassiveManager.GetShield().iD == "5oo" 
            || playerPassiveManager.GetShield().iD == "61t" || playerPassiveManager.GetShield().iD == "80d")
        {
            playerPassiveManager.GetShield().TurnsInPlay--;
            DisplayNewCard(playerPassiveManager.GetShieldID(), playerPassiveManager.GetShield());
        }


        List<Card> permCards = playerPermanentManager.GetAllCards();
        List<ID> permIds = playerPermanentManager.GetAllIds();
        if(permCards.Count == 0) { yield break; }
        for (int i = 0; i < permCards.Count; i++)
        {
            if(permCards[i].iD == "7q9" || permCards[i].iD == "5rp"
                || permCards[i].iD == "5v2" || permCards[i].iD == "7ti")
            {
                permCards[i].TurnsInPlay--;
                DisplayNewCard(permIds[i], permCards[i]);
            }
        }
    }

    public void StartTurn()
    {
        if (deckManager.GetDeckCount() <= 0)
        {
            GameOverVisual.ShowGameOverScreen(!isPlayer);
            return;
        }
        StartCoroutine(TurnDownTick());
        DrawCardFromDeckLogic();
    }

    public IEnumerator UpdateCounterAndEffects()
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
        UpdatePlayerIndicators();
        //Creature Counters Last
        List<Card> creatureCards = playerPermanentManager.GetAllCards();
        List<ID> creatureIds = playerPermanentManager.GetAllIds();

        for (int i = 0; i < creatureCards.Count; i++)
        {
            creatureCards[i].AbilityUsed = false;
            if (creatureCards[i].TurnsInPlay <= 0 && creatureCards[i].iD == "5v2" && creatureCards[i].iD == "7ti")
            {
                yield return StartCoroutine(RemoveCardFromFieldLogic(creatureIds[i]));
            }
        }

        Card shield = playerPassiveManager.GetShield();
        if (shield != null)
        {
            if (!shield.iD.Equals("4t1"))
            {
                if (playerCounters.bone > 0)
                {
                    boneShieldLabel.text = $"{playerCounters.bone}";
                }
                else if (shield.iD == "5oo" || shield.iD == "7n8" || shield.iD == "80d" || shield.iD == "61t")
                {
                    boneShieldLabel.text = shield.TurnsInPlay.ToString();

                    if (shield.TurnsInPlay < 0)
                    {
                        boneShieldLabel.text = "";
                        yield return StartCoroutine(RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID()));
                    }
                }
                else if (shield.iD == "5lk" || shield.iD == "7k4")
                {
                    int bioCreatures = GetLightEmittingCreatures();
                    bioCreatures += shield.iD == "7k4" ? 1 : 0;
                    boneShieldLabel.text = $"{bioCreatures}";
                }
                else
                {
                    boneShieldLabel.text = "";
                }
            }
        }

        Card weapon = playerPassiveManager.GetWeapon();
        if (weapon != null)
        {
            if (!weapon.iD.Equals("4t2"))
            {
                weapon.AbilityUsed = false;
            }
        }
    }

    public int GetLightEmittingCreatures()
    {
        List<ID> allIds = playerCreatureField.GetAllIds();
        int count = 0;
        foreach (ID id in allIds)
        {
            Card card = GetCard(id);
            if (card.passive.Contains("light"))
            {
                count++;
            }
        }
        return count;
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
    //Logic

    public ID PlayCardOnField(Card card)
    {
        switch (card.cardType)
        {
            case CardType.Pillar:
                if (card.iD.IsUpgraded())
                {
                    if (card.costElement.Equals(Element.Other))
                    {
                        StartCoroutine(GenerateQuantaLogic(card.costElement, 3));
                    }
                    else
                    {
                        StartCoroutine(GenerateQuantaLogic(card.costElement, 1));
                    }
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


    public IEnumerator ManageID(ID iD)
    {
        if (!BattleVars.shared.isPlayerTurn) { yield break; }
        if (iD == null) { yield break; }

        if (iD.Owner.Equals(OwnerEnum.Opponent))
        {
            if (iD.Field.Equals(FieldEnum.Hand)) { yield break; }

            if (DuelManager.enemy.playerCounters.invisibility > 0 && !DuelManager.enemy.cloakIndex.Contains(iD)) { yield break; }
            cardDetailManager.SetCardOnDisplay(iD);
            cardDetailView.SetupCardDisplay(iD, DuelManager.GetCard(iD), false);
            yield break;
        }

        if (BattleVars.shared.hasToDiscard)
        {
            if (iD.Field.Equals(FieldEnum.Hand))
            {
                DiscardCard(iD);
                BattleVars.shared.hasToDiscard = false;
                DuelManager.discardTextStatic.gameObject.SetActive(false);
                yield return StartCoroutine(DuelManager.EndTurn());
                yield break;
            }
        }

        Card card = GetCard(iD);
        if (PlayerPrefs.GetInt("QuickPlay") == 0)
        {
            if (iD.Field.Equals(FieldEnum.Player)) { yield break; }
            if (iD.Field.Equals(FieldEnum.Hand) && playerQuantaManager.HasEnoughQuanta(card.costElement, card.cost))
            {
                if (playerCounters.silence > 0 && sanctuaryCount == 0) { yield break; }
                if (!card.cardType.Equals(CardType.Spell))
                {
                    PlayCardFromHandLogic(iD);
                }
                else if(IsSpellPlayable(card))
                {
                    BattleVars.shared.originId = iD;
                    BattleVars.shared.cardOnStandBy = card;
                    if (BattleVars.shared.IsFixedTarget())
                    {
                        yield return StartCoroutine(SkillManager.Instance.SkillRoutineNoTarget(this, card));
                        if (card.cardType.Equals(CardType.Spell)) { PlayCardFromHandLogic(iD); }

                        if (!(BattleVars.shared.cardOnStandBy.skill == "photosynthesis"))
                        {
                            card.AbilityUsed = true;
                        }
                        yield return StartCoroutine(SpendQuantaLogic(BattleVars.shared.cardOnStandBy.skillElement, BattleVars.shared.cardOnStandBy.skillCost));
                    }
                    else
                    {
                        BattleVars.shared.isSelectingTarget = true;
                        SkillManager.Instance.SetupTargetHighlights(this, DuelManager.enemy, BattleVars.shared.cardOnStandBy);
                    }
                }
            }
            else if (IsAbilityUsable(card) && !iD.Field.Equals(FieldEnum.Hand))
            {
                BattleVars.shared.originId = iD;
                BattleVars.shared.cardOnStandBy = card;
                if (BattleVars.shared.IsFixedTarget())
                {
                    yield return StartCoroutine(SkillManager.Instance.SkillRoutineNoTarget(this, card));
                    if (card.cardType.Equals(CardType.Spell)) { PlayCardFromHandLogic(iD); }

                    if (!(BattleVars.shared.cardOnStandBy.skill == "photosynthesis"))
                    {
                        card.AbilityUsed = true;
                    }
                    yield return StartCoroutine(SpendQuantaLogic(BattleVars.shared.cardOnStandBy.skillElement, BattleVars.shared.cardOnStandBy.skillCost));
                }
                else
                {
                    BattleVars.shared.isSelectingTarget = true;
                    SkillManager.Instance.SetupTargetHighlights(this, DuelManager.enemy, BattleVars.shared.cardOnStandBy);
                }
            }

            yield break;
        }


        cardDetailManager.SetCardOnDisplay(iD);
        cardDetailView.SetupCardDisplay(iD, GetCard(iD), playerQuantaManager.HasEnoughQuanta(card.costElement, card.cost));
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
                StartCoroutine(ActivateAbility(cardDetailManager.GetCardID()));
                cardDetailManager.ClearID();
                break;
            case "Select Target":
                BattleVars.shared.isSelectingTarget = true;
                SkillManager.Instance.SetupTargetHighlights(this, DuelManager.enemy, BattleVars.shared.cardOnStandBy);
                break;
            default:
                cardDetailManager.ClearID();
                break;
        }
    }
    public IEnumerator ActivateAbility(ID iD)
    {
        Card originCard = GetCard(BattleVars.shared.originId);
        Card targetCard = iD.Field.Equals(FieldEnum.Player) ? null : DuelManager.GetCard(iD);
        if (BattleVars.shared.cardOnStandBy.cardType.Equals(CardType.Spell))
        {
            ActionManager.AddSpellPlayedAction(isPlayer, originCard, BattleVars.shared.IsFixedTarget() ? null : targetCard);
            PlayCardFromHandLogic(BattleVars.shared.originId);
            yield return StartCoroutine(SkillManager.Instance.SkillRoutineWithTarget(DuelManager.GetIDOwner(iD), targetCard, iD));
        }
        else
        {
            ActionManager.AddAbilityActivatedAction(isPlayer, originCard, BattleVars.shared.IsFixedTarget() ? null : targetCard);
            if (!(BattleVars.shared.cardOnStandBy.skill == "photosynthesis"))
            {
                originCard.AbilityUsed = true;
            }
            yield return StartCoroutine(SpendQuantaLogic(BattleVars.shared.cardOnStandBy.skillElement, BattleVars.shared.cardOnStandBy.skillCost));

            yield return StartCoroutine(SkillManager.Instance.SkillRoutineWithTarget(DuelManager.GetIDOwner(iD), targetCard, iD));
        }
        DuelManager.ResetTargeting();
        DisplayPlayableGlow();
    }

    public bool IsCardPlayable(Card cardToCheck)
    {
        bool canAfford = playerQuantaManager.HasEnoughQuanta(cardToCheck.costElement, cardToCheck.cost);
        bool hasSpace = true;

        switch (cardToCheck.cardType)
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

        return canAfford && hasSpace && !cardToCheck.AbilityUsed;
    }

    public bool IsAbilityUsable(Card cardToCheck)
    {
        if (cardToCheck == null) { return false; }
        if (cardToCheck.skill == "" || cardToCheck.skill == "none") { return false; }
        if (cardToCheck.AbilityUsed) { return false; }
        if (cardToCheck.IsDelayed) { return false; }
        if (cardToCheck.Freeze > 0) { return false; }

        bool canAfford = playerQuantaManager.HasEnoughQuanta(cardToCheck.skillElement, cardToCheck.skillCost);
        if (cardToCheck.skill.ToString().Contains("Haste"))
        {
            if (playerHand.GetAllCards().Count == 8) { return false; }
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
            if(playerPassiveManager.GetWeapon().iD == "4t2") { return false; }
        }

        return canAfford;
    }

    //Command Methods


    //Remove Card from field Logic and Visual Command Pair
    public IEnumerator RemoveCardFromFieldLogic(ID cardID, int amount = 1, bool shouldActivateDeath = true)
    {
        if (GetCard(cardID) == null) { yield break; }
        switch (cardID.Field)
        {
            case FieldEnum.Hand:
                playerHand.PlayCardWithID(cardID);
                break;
            case FieldEnum.Creature:
                yield return StartCoroutine(CheckOnPlayAbility(GetCard(cardID), cardID, false));
                if (shouldActivateDeath)
                {
                    yield return StartCoroutine(DuelManager.player.ActivateDeathTriggers(GetCard(cardID).cardName.Contains("Skeleton")));
                    yield return StartCoroutine(DuelManager.enemy.ActivateDeathTriggers(GetCard(cardID).cardName.Contains("Skeleton")));
                }
                if (GetCard(cardID).passive?.Count > 0)
                {
                    if (GetCard(cardID).IsAflatoxin)
                    {
                        DisplayNewCard(cardID, CardDatabase.GetCardFromId("6ro"));
                        yield break;
                    }
                    if (GetCard(cardID).passive.Contains("phoenix"))
                    {
                        bool shouldRebirth = true;
                        if (BattleVars.shared.cardOnStandBy != null)
                        {
                            if (BattleVars.shared.cardOnStandBy.skill == "reverse time")
                            {
                                shouldRebirth = false;
                            }
                        }
                        if (shouldRebirth)
                        {
                            DisplayNewCard(cardID, CardDatabase.GetCardFromId(GetCard(cardID).iD.IsUpgraded() ? "7dt" : "5fd"));
                            yield break;
                        }
                    }
                }
                playerCreatureField.DestroyCard(cardID.Index);
                creatureDisplayers[cardID.Index].PlayDissolveAnimation();
                break;
            case FieldEnum.Passive:
                if (playerCounters.bone > 0)
                {
                    playerCounters.bone -= 1;
                    if (playerCounters.bone <= 0)
                    {
                        playerPassiveManager.DestroyCard(cardID.Index);
                        PlayCardOnFieldLogic(CardDatabase.GetPlaceholderCard(cardID.Index));
                    }
                    else
                    {
                        boneShieldLabel.text = $"{playerCounters.bone}";
                    }
                }
                else
                {
                    playerPassiveManager.DestroyCard(cardID.Index);
                    PlayCardOnFieldLogic(CardDatabase.GetPlaceholderCard(cardID.Index));
                    //passiveDisplayers[cardID.Index].PlayDissolveAnimation();
                }
                break;
            case FieldEnum.Permanent:
                StartCoroutine(CheckOnPlayAbility(GetCard(cardID), cardID, false));
                for (int i = 0; i < amount; i++)
                {
                    playerPermanentManager.DestroyCard(cardID.Index);
                }
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
        yield return StartCoroutine(Game_AnimationManager.shared.PlayAnimation("CardDeath", Battlefield_ObjectIDManager.shared.GetObjectFromID(cardID)));
        Game_SoundManager.shared.PlayAudioClip("RemoveCardFromField");
    }

    public void FillHandWith(Card newCard)
    {
        int amountToAdd = 8 - playerHand.GetAllCards().Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            deckManager.AddCardToTop(CardDatabase.GetCardFromId(newCard.iD));
            DrawCardFromDeckLogic();
        }
    }

    public async void RemoveCardFromFieldVisual(ID cardID)
    {
        await new WaitForSeconds(animSpeed);
        Command.CommandExecutionComplete();
    }


    //Modify Health Logic and Visual Command Pair
    public IEnumerator ModifyHealthLogic(int amount, bool isDamage, bool fromSpell)
    {
        if (sacrificeCount > 0) { isDamage = !isDamage; }
        int newHealth = healthManager.ModifyHealth(amount, isDamage);
        yield return StartCoroutine(healthDisplayer.UpdateHPView(newHealth, isPlayer));

    }

    [SerializeField]
    private Sprite poisonSprite, puritySprite, nuerotoxinSprite;
    public Image poisonImg, sanctImage, silenceImage;

    public void UpdatePlayerIndicators()
    {
        if(playerCounters.poison != 0)
        {
            poisonImg.gameObject.SetActive(true);
            if (playerCounters.poison < 0)
            {
                poisonImg.sprite = puritySprite;
                poisonLabel.text = "+" + Mathf.Abs(playerCounters.poison).ToString();
            }
            else if (playerCounters.nuerotoxin > 0)
            {
                poisonImg.sprite = nuerotoxinSprite;
                poisonLabel.text = playerCounters.poison.ToString();
            }
            else if (playerCounters.poison == 0)
            {
                poisonImg.gameObject.SetActive(false);
            }
            else
            {
                poisonImg.sprite = poisonSprite;
                poisonLabel.text = playerCounters.poison.ToString();
            }
        }

        if(playerCounters.invisibility != 0)
        {
            if (!isPlayer && playerCounters.invisibility > 0)
            {
                cloakVisual.SetActive(true);
            }
            if (playerCounters.invisibility <= 0)
            {
                playerCounters.invisibility = 0;
                if (!isPlayer)
                {
                    cloakVisual.SetActive(false);
                }
            }
        }
        silenceImage.gameObject.SetActive(playerCounters.silence > 0);
        sanctImage.gameObject.SetActive(sanctuaryCount > 0);
    }
    public List<ID> cloakIndex = new();
    public int sacrificeCount;
    public void DisplayNewCard(ID cardID, Card cardPlayed, bool shouldAnim = true, bool isPlayed = false)
    {
        switch (cardID.Field)
        {
            case FieldEnum.Hand:
                playerHand.pairList[cardID.Index].card = cardPlayed;
                handDisplayers[cardID.Index].DisplayCard(cardPlayed, !shouldAnim);
                break;
            case FieldEnum.Creature:
                playerCreatureField.pairList[cardID.Index].card = cardPlayed;
                creatureDisplayers[cardID.Index].DisplayCard(cardPlayed, shouldAnim);
                if (cardPlayed.DefNow <= 0 && !isPlayed) { StartCoroutine(RemoveCardFromFieldLogic(cardID)); return; }
                break;
            case FieldEnum.Passive:
                playerPassiveManager.pairList[cardID.Index].card = cardPlayed;
                passiveDisplayers[cardID.Index].DisplayCard(cardPlayed, shouldAnim);
                if (cardPlayed.cardType.Equals(CardType.Shield))
                {
                    if (cardPlayed.iD == "71b" || cardPlayed.iD == "52r")
                    {
                        boneShieldLabel.text = $"{playerCounters.bone}";
                    }
                    else if(cardPlayed.iD == "5oo" || cardPlayed.iD == "7n8" || cardPlayed.iD == "80d" || cardPlayed.iD == "61t")
                    {
                        boneShieldLabel.text = cardPlayed.TurnsInPlay.ToString();
                    }
                    else if (cardPlayed.iD == "5lk" || cardPlayed.iD == "7k4")
                    {
                        int bioCreatures = GetLightEmittingCreatures();
                        bioCreatures += cardPlayed.iD == "7k4" ? 1 : 0;
                        boneShieldLabel.text = $"{bioCreatures}";
                    }
                     
                }
                break;
            case FieldEnum.Permanent:
                playerPermanentManager.pairList[cardID.Index].card = cardPlayed;
                permanentDisplayers[cardID.Index].DisplayCard(cardPlayed);
                break;
            default:
                break;
        }
    }

    //Play Card From Hand Logic and Visual Command Pair
    public void PlayCardFromHandLogic(ID cardID)
    {
        //Logic Side
        //Get Card SO In Hand
        Game_SoundManager.shared.PlayAudioClip("CardPlay");
        if (playerCounters.nuerotoxin > 0)
        {
            playerCounters.poison++;
            UpdatePlayerIndicators();
        }
        Card cardPlayed = playerHand.GetCardWithID(cardID);
        //Play Card on field if it is not a spell
        if (!cardPlayed.cardType.Equals(CardType.Spell))
        {
            ID isNull = PlayCardOnFieldLogic(cardPlayed);
            if (isNull == null)
            {
                return;
            }
            ActionManager.AddCardPlayedOnFieldAction(isPlayer, cardPlayed);
        }
        //Remove Card From Hand
        playerHand.PlayCardWithID(cardID);

        //Spend Quanta
        if (cardPlayed.cost > 0)
        {
            StartCoroutine(SpendQuantaLogic(cardPlayed.costElement, cardPlayed.cost));
        }

        DisplayPlayableGlow();
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
        if (newCard == null)
        {
            Debug.Log("Game Over");
            GameOverVisual.ShowGameOverScreen(!isPlayer);
            return;
        }
        deckDisplayer.UpdateDeckCount(deckManager.GetDeckCount());
        //Add Card To Hand
        playerHand.AddCardToHand(newCard);
        if (!isInitialDraw)
        {
            ActionManager.AddCardDrawAction(isPlayer, newCard);
        }
        DisplayPlayableGlow();
    }

    public async void DrawCardFromDeckVisual(Card newCard, ID cardId)
    {
        //Visual Side
        //Play Hand Card Animation
        DisplayNewCard(cardId, newCard);
        await new WaitForSeconds(animSpeed);
        DisplayPlayableGlow();
        Command.CommandExecutionComplete();
    }

    public void HideAllPlayableGlow()
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

        passiveDisplayers[1].ShouldShowUsableGlow(false);
    }

    void DisplayPlayableGlow()
    {
        HideAllPlayableGlow();
        if (!isPlayer) { return; }
        List<Card> cards = new(playerHand.GetAllCards());
        List<ID> cardIds = new(playerHand.GetAllIds());

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

        cards = new(playerCreatureField.GetAllCards());
        cardIds = new(playerCreatureField.GetAllIds());

        if (cards?.Any() ?? false)
        {
            for (int i = 0; i < cardIds.Count; i++)
            {
                creatureDisplayers[cardIds[i].Index].ShouldShowUsableGlow(IsAbilityUsable(cards[i]));
            }

        }

        cards = new(playerPermanentManager.GetAllCards());
        cardIds = new(playerPermanentManager.GetAllIds());

        if (cards?.Any() ?? false)
        {
            for (int i = 0; i < cardIds.Count; i++)
            {
                permanentDisplayers[cardIds[i].Index].ShouldShowUsableGlow(IsAbilityUsable(cards[i]));
            }
        }

        Card weaponCard = playerPassiveManager.GetWeapon();
        if (weaponCard != null)
        {
            passiveDisplayers[1].ShouldShowUsableGlow(IsAbilityUsable(weaponCard));
        }
    }

    //Modify Cards on the field
    public void ModifyCreatureLogic(ID target, CounterEnum? counter = null, int countAmount = 0, int modifyPower = 0, int modifyHP = 0, PassiveEnum? passiveToChange = null, bool newPassiveValue = false, int damage = 0, bool isModifyPerm = true)
    {
        bool isVoodoo = GetCard(target).passive.Contains("voodoo");

        if (counter != null)
        {
            if (isVoodoo)
            {
                PlayerManager voodooOwner = isPlayer ? DuelManager.enemy : DuelManager.player;

                switch (counter)
                {
                    case CounterEnum.Freeze:
                        voodooOwner.playerCounters.freeze += countAmount;
                        break;
                    case CounterEnum.Poison:
                        voodooOwner.playerCounters.poison += countAmount;
                        break;
                    case CounterEnum.Delay:
                        voodooOwner.playerCounters.delay += countAmount;
                        break;
                    default:
                        break;
                }
                voodooOwner.UpdatePlayerIndicators();
            }
            playerCreatureField.ApplyCounter((CounterEnum)counter, countAmount, target);
        }

        if (modifyHP != 0 || modifyPower != 0)
        {
            if (isVoodoo && modifyHP < 0)
            {
                StartCoroutine(DuelManager.GetNotIDOwner(target).ModifyHealthLogic(-modifyHP, true, false));

            }
            playerCreatureField.ModifyPowerHP(modifyPower, modifyHP, target, isModifyPerm);

        }

        if (passiveToChange != null)
        {
            //playerCreatureField.ChangePassiveAbility((PassiveEnum)passiveToChange, newPassiveValue, target);
        }

        if (damage != 0)
        {
            Card damageResult = playerCreatureField.DamageCreature(damage, target);
            if (damageResult == null)
            {
                StartCoroutine(RemoveCardFromFieldLogic(target));
                return;
            }
            else
            {
                if (isVoodoo)
                {
                    StartCoroutine(DuelManager.GetNotIDOwner(target).ModifyHealthLogic(damage, true, false));
                }
            }
        }

        Card creature = playerCreatureField.GetCardWithID(target);
        if (creature == null)
        {
            StartCoroutine(RemoveCardFromFieldLogic(target));
            return;
        }

        if (creature.DefNow <= 0)
        {
            StartCoroutine(RemoveCardFromFieldLogic(target));
            return;
        }

        DisplayNewCard(target, creature, false);
    }

    public IEnumerator ActivateDeathTriggers(bool isSkeleDeath)
    {
        List<Card> cardsToCheck = playerCreatureField.GetAllCards();
        if (cardsToCheck.Count > 0)
        {
            foreach (Card creature in cardsToCheck)
            {
                if (creature.passive.Contains("scavenger"))
                {
                    creature.AtkModify += 1;
                    creature.DefModify += 1;
                }
            }
        }
        cardsToCheck = playerPermanentManager.GetAllCards();
        if (cardsToCheck.Count > 0)
        {
            foreach (Card perm in cardsToCheck)
            {
                if (perm.skill == "soul catch")
                {
                    StartCoroutine(GenerateQuantaLogic(Element.Death, perm.iD.IsUpgraded() ? 3 : 2));
                }
                if (perm.skill == "boneyard" && !isSkeleDeath)
                {
                    PlayCardOnFieldLogic(CardDatabase.GetCardFromId(perm.iD.IsUpgraded() ? "716" : "52m"));
                }
            }
        }
        if (playerPassiveManager.GetShield().skill == "bones" && !isSkeleDeath)
        {
            playerCounters.bone += 2;
            boneShieldLabel.text = $"{playerCounters.bone}";
        }
        yield break;
    }

    private IEnumerator CheckOnPlayAbility(Card card, ID location, bool isPlayed)
    {
        if (card.skill == "" || card.skill == "none") { yield break; }
        List<Card> creatures = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();
        if (isPlayed)
        {
            if(card.iD == "7q9" || card.iD == "5rp")
            {
                card.TurnsInPlay = 1;
            }
            if(card.iD == "7n8" || card.iD == "5oo")
            {
                card.TurnsInPlay = 5;
            }
            if(card.iD == "61t" || card.iD == "80d")
            {
                card.TurnsInPlay = 3;
            }
            if(card.iD == "5v2" || card.iD == "7ti")
            {
                card.TurnsInPlay = 3;
            }

            if (card.skill == "eclipse")
            {
                DuelManager.player.CheckEclipseNightfall(true, "eclipse");
                DuelManager.enemy.CheckEclipseNightfall(true, "eclipse");
            }
            if (card.skill == "nightfall")
            {
                DuelManager.player.CheckEclipseNightfall(true, "nightfall");
                DuelManager.enemy.CheckEclipseNightfall(true, "nightfall");
            }
            if (card.skill == "bones")
            {
                if (BattleVars.shared.cardOnStandBy == null)
                {
                    playerCounters.bone = 7;
                    boneShieldLabel.text = "7";
                }
                else
                {
                    if (BattleVars.shared.cardOnStandBy.skill == "steal")
                    {
                        playerCounters.bone = 1;
                        boneShieldLabel.text = "1";
                    }
                    else
                    {
                        playerCounters.bone = 7;
                        boneShieldLabel.text = "7";
                    }
                }
            }
            if (card.innate.Contains("swarm"))
            {
                scarabsPlayed++;
            }
            if (card.innate.Contains("chimera"))
            {
                (int, int) chimeraPwrHP = (0, 0);

                if (creatures != null)
                {
                    if (creatures.Count > 0)
                    {
                        for (int i = 0; i < creatures.Count; i++)
                        {
                            if (location.Index == creatureIds[i].Index) { continue; }
                            chimeraPwrHP.Item1 += creatures[i].atk;
                            chimeraPwrHP.Item2 += creatures[i].def;
                            yield return StartCoroutine(RemoveCardFromFieldLogic(creatureIds[i]));
                        }
                    }
                }
                card.AtkModify += chimeraPwrHP.Item1;
                card.DefModify += chimeraPwrHP.Item2;
            }
            if (card.passive.Contains("sanctuary"))
            {
                sanctuaryCount++;
                sanctImage.gameObject.SetActive(true);
            }
            if (card.skill == "patience")
            {
                patienceCount++;
            }
            if (card.skill == "freedom")
            {
                freedomCount++;
            }
            if (card.skill == "flood")
            {
                DuelManager.AddFloodCount();
            }
            if (card.skill == "cloak")
            {
                playerCounters.invisibility = 3;
                cloakVisual.SetActive(!isPlayer);
            }
            yield break;
        }


        if (card.skill == "eclipse")
        {
            DuelManager.player.CheckEclipseNightfall(false, "eclipse");
            DuelManager.enemy.CheckEclipseNightfall(false, "eclipse");
        }
        if (card.skill == "nightfall")
        {
            DuelManager.player.CheckEclipseNightfall(false, "nightfall");
            DuelManager.enemy.CheckEclipseNightfall(false, "nightfall");
        }
        if (card.innate.Contains("swarm"))
        {
            scarabsPlayed--;
        }

        if (card.passive.Contains("sanctuary"))
        {
            sanctuaryCount--;
            sanctImage.gameObject.SetActive(sanctuaryCount > 0);
        }
        if (card.skill == "patience")
        {
            patienceCount--;
        }
        if (card.skill == "freedom")
        {
            freedomCount--;
        }
        if (card.skill == "flood")
        {
            DuelManager.RemoveFloodCount();
        }
    }


    private void CheckEclipseNightfall(bool isAdded, string skill)
    {
        List<Card> creatures = playerCreatureField.GetAllCards();
        if (isAdded)
        {
            if (skill == "eclipse")
            {
                foreach (var creature in creatures)
                {
                    if (!DuelManager.isEclipseInPlay())
                    {
                        if (DuelManager.IsNightfallInPlay())
                        {
                            creature.AtkModify += 1;
                        }
                        else
                        {
                            creature.AtkModify += 2;
                            creature.DefModify += 1;
                        }
                    }
                }
            }
            if (skill == "nightfall")
            {
                foreach (var creature in creatures)
                {
                    if (!DuelManager.isEclipseInPlay() && !DuelManager.IsNightfallInPlay())
                    {
                        creature.AtkModify += 1;
                        creature.DefModify += 1;
                    }
                }
            }
        }
        else
        {
            if (skill == "eclipse")
            {
                foreach (var creature in creatures)
                {
                    if (!DuelManager.isEclipseInPlay())
                    {
                        if (DuelManager.IsNightfallInPlay())
                        {
                            creature.AtkModify -= 1;
                        }
                        else
                        {
                            creature.AtkModify -= 2;
                            creature.DefModify -= 1;
                        }
                    }
                }
            }
            if (skill == "nightfall")
            {
                foreach (var creature in creatures)
                {
                    if (!DuelManager.isEclipseInPlay() && !DuelManager.IsNightfallInPlay())
                    {
                        creature.AtkModify -= 1;
                        creature.DefModify -= 1;
                    }
                }
            }
        }
    }

    //Play Card from anywhere Logic and Visual
    public ID PlayCardOnFieldLogic(Card card)
    {
        ID newLocationId = PlayCardOnField(card);
        if (newLocationId == null) { return null; }
        StartCoroutine(CheckOnPlayAbility(card, newLocationId, true));
        card.AbilityUsed = true;
        if ((card.costElement.Equals(Element.Darkness) || card.costElement.Equals(Element.Death)) && card.cardType.Equals(CardType.Creature))
        {
            if (DuelManager.isEclipseInPlay())
            {
                card.DefModify += 1;
                card.AtkModify += 2;
            }
            else if (DuelManager.IsNightfallInPlay())
            {
                card.DefModify += 1;
                card.AtkModify += 1;
            }
        }
        DisplayNewCard(newLocationId, card, true, true);
        return newLocationId;
    }

    public void DiscardCard(ID cardToDiscard)
    {
        playerHand.PlayCardWithID(cardToDiscard);
    }

    public IEnumerator GeneratePillarQuantaLogic()
    {
        List<(QuantaObject, ID)> quantaResults = playerPermanentManager.GetQuantaToGenerate();

        foreach ((QuantaObject, ID) item in quantaResults)
        {
            PlayActionAnimationVisual(item.Item2);
            yield return StartCoroutine(Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(item.Item2), item.Item1.element));
            if (item.Item1.element.Equals(Element.Other))
            {
                List<QuantaObject> rndQuantas = GetOtherElementGenerator(item.Item1.count * 3);
                foreach (QuantaObject rnd in rndQuantas)
                {
                    yield return StartCoroutine(GenerateQuantaLogic(rnd.element, rnd.count));
                }
            }
            else
            {
                yield return StartCoroutine(GenerateQuantaLogic(item.Item1.element, item.Item1.count));
            }
        }

        PlayActionAnimationVisual(playerPassiveManager.GetMarkID());
        yield return StartCoroutine(Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(playerPassiveManager.GetMarkID()), playerPassiveManager.GetMark().costElement));


        if (BattleVars.shared.enemyAiData.maxHP >= 150 && !isPlayer)
        {
            yield return StartCoroutine(GenerateQuantaLogic(playerPassiveManager.GetMark().costElement, 3));
        }
        else
        {
            yield return StartCoroutine(GenerateQuantaLogic(playerPassiveManager.GetMark().costElement, 1));
        }
        yield return StartCoroutine(CheckPermanentEndTurn());
    }

    public IEnumerator SpendQuantaLogic(Element element, int amount)
    {

        if ((isPlayer && !BattleVars.shared.isPlayerTurn) || (!isPlayer && BattleVars.shared.isPlayerTurn))
        {
            if (sanctuaryCount > 0)
            {
                yield break;
            }
        }
        yield return StartCoroutine(playerQuantaManager.ChangeQuanta(element, amount, false));
    }

    public IEnumerator GenerateQuantaLogic(Element element, int amount)
    {

        if ((isPlayer && !BattleVars.shared.isPlayerTurn) || (!isPlayer && BattleVars.shared.isPlayerTurn))
        {
            if (sanctuaryCount > 0)
            {
                yield break;
            }
        }
        yield return StartCoroutine(playerQuantaManager.ChangeQuanta(element, amount, true));
    }

    public async void PlayActionAnimationVisual(ID cardID)
    {
        switch (cardID.Field)
        {
            case FieldEnum.Hand:
                break;
            case FieldEnum.Creature:
                break;
            case FieldEnum.Passive:
                break;
            case FieldEnum.Permanent:
                permanentDisplayers[cardID.Index].UpdatePendulumDisplay();
                break;
            case FieldEnum.Player:
                break;
            default:
                break;
        }
        await new WaitForSeconds(animSpeed);
        Command.CommandExecutionComplete();
    }


    //Setup Methods
    private IEnumerator SetupCreatureDisplayers()
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
        yield return null;
    }

    private IEnumerator SetupHandDisplayers()
    {

        List<ID> idHandList = new List<ID>();
        for (int i = 0; i < handDisplayers.Count; i++)
        {
            handDisplayers[i].SetupDisplayer(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Hand);
            idHandList.Add(handDisplayers[i].GetObjectID());
        }

        playerHand = new HandManager(idHandList, handDisplayers);
        yield return null;
    }

    private IEnumerator SetupPermanentDisplayers()
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
        yield return null;
    }

    private IEnumerator SetupPassiveDisplayers()
    {
        List<ID> idPassiveList = new List<ID>();
        for (int i = 0; i < passiveDisplayers.Count; i++)
        {
            passiveDisplayers[i].SetupDisplayer(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Passive);
            idPassiveList.Add(passiveDisplayers[i].GetObjectID());
        }
        playerPassiveManager = new PassiveManager(idPassiveList);
        Element markElement;
        markElement = isPlayer ? PlayerData.shared.markElement : BattleVars.shared.enemyAiData.mark;

        Card mark = CardDatabase.GetCardFromId(CardDatabase.markIds[(int)markElement]);
        playerPassiveManager.PlayPassive(mark);
        playerPassiveManager.PlayPassive(CardDatabase.GetPlaceholderCard(2));
        playerPassiveManager.PlayPassive(CardDatabase.GetPlaceholderCard(1));
        passiveDisplayers[0].DisplayCard(mark, true);
        yield return null;
    }


    private IEnumerator SetupOtherDisplayers()
    {
        playerQuantaManager = new QuantaManager(quantaDisplayers, this);
        cardDetailManager = new CardDetailManager();
        playerCounters = new Counters();
        playerDisplayer.SetID(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Player, 0, playerDisplayer.transform);

        List<Card> deck = new List<Card>(isPlayer ?
                    PlayerData.shared.currentDeck.DeserializeCard()
                    : new List<string>(BattleVars.shared.enemyAiData.deck.Split(" ")).DeserializeCard());

        if (BattleVars.shared.enemyAiData.maxHP == 200 && !isPlayer)
        {
            deck.AddRange(deck);

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
            Card petCard = CardDatabase.GetCardFromId(PlayerData.shared.petName);
            PlayerData.shared.petCount--;
            if (PlayerData.shared.petCount <= 0)
            {
                PlayerData.shared.petName = "";
            }
            PlayCardOnFieldLogic(petCard);
        }

        if (!isPlayer)
        {
            DuelManager.allPlayersSetup = true;
        }
        yield return null;
    }



    public IEnumerator SetupPlayerManager(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        yield return StartCoroutine(SetupCreatureDisplayers());
        yield return StartCoroutine(SetupHandDisplayers());
        yield return StartCoroutine(SetupPassiveDisplayers());
        yield return StartCoroutine(SetupPermanentDisplayers());
        yield return StartCoroutine(SetupOtherDisplayers());
    }

    public IEnumerator ReceivePhysicalDamage(int damage)
    {
        if (damage == 0) { yield break; }
        //Damage player with leftover damage or direct damage
        yield return StartCoroutine(ModifyHealthLogic(damage, true, false));
        yield return null;
    }

    private IEnumerator CheckPermanentEndTurn()
    {
        List<int> floodList = new List<int> { 11, 13, 9, 10, 12 };
        //Get all creatures in play
        List<Card> permCards = playerPermanentManager.GetAllCards();
        List<ID> permIds = playerPermanentManager.GetAllIds();
        //If no perms, end here
        if (permIds.Count == 0) { goto skipPermCheck; }
        for (int i = 0; i < permCards.Count; i++)
        {
            Card perm = permCards[i];
            if (perm.skill == "sactuary")
            {
                StartCoroutine(ModifyHealthLogic(4, false, false));
            }
            if (perm.skill == "void")
            {
                if (playerPassiveManager.GetMark().costElement == Element.Darkness)
                {
                    if (isPlayer)
                    {
                        DuelManager.enemy.ModifyMaxHealthLogic(3, false);
                    }
                    else
                    {
                        DuelManager.player.ModifyMaxHealthLogic(3, false);
                    }
                }
                else
                {
                    if (isPlayer)
                    {
                        DuelManager.enemy.ModifyMaxHealthLogic(2, false);
                    }
                    else
                    {
                        DuelManager.player.ModifyMaxHealthLogic(2, false);
                    }
                }
            }
            if (perm.skill == "gratitude")
            {
                if (playerPassiveManager.GetMark().costElement == Element.Life)
                {
                    StartCoroutine(ModifyHealthLogic(5, false, false));
                }
                else
                {
                    StartCoroutine(ModifyHealthLogic(3, false, false));
                }
            }
            if (perm.skill == "empathy")
            {
                StartCoroutine(ModifyHealthLogic(playerCreatureField.GetAllCards().Count, false, false));
            }
            if (perm.skill == "flood")
            {
                StartCoroutine(DuelManager.enemy.ClearFloodedArea(floodList));
                StartCoroutine(DuelManager.player.ClearFloodedArea(floodList));
            }
            if (perm.skill == "patience")
            {
                List<Card> creatures = playerCreatureField.GetAllCards();
                List<ID> iDs = playerCreatureField.GetAllIds();

                for (int y = 0; y < creatures.Count; y++)
                {
                    if (DuelManager.IsFloodInPlay() && floodList.Contains(iDs[y].Index))
                    {
                        creatures[y].AtkModify += 5;
                        creatures[y].DefModify += 5;
                    }
                    else
                    {
                        creatures[y].AtkModify += 2;
                        creatures[y].DefModify += 2;
                    }
                }
            }
        }

        skipPermCheck:
        yield return StartCoroutine(CreatureCheckStep());
    }
    public void ShieldCheck(ID attackerID, ref Card attacker, ref int atknow)
    {
        PlayerManager opponent = isPlayer ? DuelManager.enemy : DuelManager.player;
        int defnow = attacker.def;
        Card shield = playerPassiveManager.GetShield();
        string skill = shield.skill;

        if(skill == "none") { return; }

        if (skill == "reflect" && attacker.passive.Contains("psion"))
        {
            StartCoroutine(opponent.ModifyHealthLogic(atknow, true, false));
            atknow = 0;
        }
        else if (skill != "reflect" && attacker.passive.Contains("psion"))
        {
            return;
        }
        if (skill == "phaseshift")
        {
            atknow = 0;
        }
        if (skill == "solar" && atknow > 0)
        {
            StartCoroutine(GenerateQuantaLogic(Element.Light, 1));
        }
        if (atknow > 0)
        {
            atknow -= shield.def;
            if (atknow < 0)
            {
                atknow = 0;
            }
        }
        if (skill == "weight" && defnow > 5 && attacker.cardType.Equals(CardType.Creature))
        {
            atknow = 0;
        }
        if (skill == "wings" && !attacker.innate.Contains("airborne") && !attacker.innate.Contains("ranged"))
        {
            atknow = 0;
        }
        if (skill == "delay" && atknow > 0)
        {
            attacker.innate.Add("delay");
        }
        if (skill == "ice" && UnityEngine.Random.Range(0f, 1f) <= 0.3f && atknow > 0)
        {
            attacker.Freeze = 3;
        }
        if (skill == "spines" && UnityEngine.Random.Range(0f, 1f) <= 0.75f && atknow > 0)
        {
            attacker.Poison += 1;
        }
        if (skill == "firewall" && atknow > 0)
        {
            defnow -= 1;
        }
        if (skill == "fog" && UnityEngine.Random.Range(0f, 1f) <= 0.4f && atknow > 0)
        {
            atknow = 0;
        }
        if (skill == "dusk" && UnityEngine.Random.Range(0f, 1f) <= 0.5f && atknow > 0)
        {
            atknow = 0;
        }
        if (skill == "unholy" && !attacker.IsAflatoxin && attacker.iD != "716" && attacker.iD != "52m"
            && UnityEngine.Random.Range(0f, 1f) <= (0.5f / defnow) && atknow > 0
            && attacker.cardType.Equals(CardType.Creature))
        {
            opponent.StartCoroutine(opponent.RemoveCardFromFieldLogic(attackerID));
            opponent.PlayCardOnFieldLogic(CardDatabase.GetCardFromId(attacker.iD.IsUpgraded() ? "716" : "52m"));
        }
        if (skill == "bones" && atknow > 0)
        {
            atknow = 0;
            playerCounters.bone -= 1;
            if (playerCounters.bone <= 0)
            {
                    playerPassiveManager.DestroyCard(playerPassiveManager.GetShieldID().Index);
                    PlayCardOnFieldLogic(CardDatabase.GetPlaceholderCard(playerPassiveManager.GetShieldID().Index));
            }
            else
            {
                boneShieldLabel.text = $"{playerCounters.bone}";
            }
        }
        if (skill == "dissipation" && sanctuaryCount == 0 && atknow > 0)
        {
            int allQuanta = GetAllQuantaOfElement(Element.Other);
            if (allQuanta >= atknow)
            {
                StartCoroutine(SpendQuantaLogic(Element.Other, atknow));
                atknow = 0;
            }
            else
            {
                StartCoroutine(RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID(), 1, false));
            }
        }
        if (skill == "edissipation" && sanctuaryCount == 0 && atknow > 0)
        {
            int quantaToUse = Mathf.CeilToInt(atknow / 3);
            int availableEQuanta = GetAllQuantaOfElement(Element.Entropy);
            if (availableEQuanta >= quantaToUse)
            {
                StartCoroutine(SpendQuantaLogic(Element.Other, quantaToUse));
                atknow = 0;
            }
            else
            {
                StartCoroutine(RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID(), 1, false));
            }
        }
        attacker.def = defnow;
    }

    public IEnumerator WeaponAttackStep()
    {
        PlayerManager opponent = isPlayer ? DuelManager.enemy : DuelManager.player;
        Card weapon = playerPassiveManager.GetWeapon();

        if (weapon.iD == "6ri") { yield break; }
        string skill = weapon.skill;
        int atknow = weapon.atk;
        if (skill == "fiery")
        {
            atknow += Mathf.FloorToInt(GetAllQuantaOfElement(Element.Fire) / 5);

        }
        if (skill == "hammer" && (playerPassiveManager.GetMark().costElement == Element.Earth || playerPassiveManager.GetMark().costElement == Element.Gravity))
        {
            atknow++;
        }
        if (skill == "dagger" && (playerPassiveManager.GetMark().costElement == Element.Death || playerPassiveManager.GetMark().costElement == Element.Darkness))
        {
            atknow++;
        }
        if (skill == "bow" && playerPassiveManager.GetMark().costElement == Element.Air)
        {
            atknow++;
        }

        if (weapon.Freeze > 0)
        {
            atknow = 0;
            weapon.Freeze--;
        }

        if (weapon.IsDelayed)
        {
            atknow = 0;
            weapon.innate.Remove("delay");
        }

        if (!weapon.passive.Contains("momentum"))
        {
            opponent.ShieldCheck(playerPassiveManager.GetWeaponID(), ref weapon, ref atknow);
        }

        //Send Damage
        StartCoroutine(opponent.ModifyHealthLogic(atknow, true, false));

        if (atknow > 0)
        {
            if (skill == "vampire")
            {
                StartCoroutine(ModifyHealthLogic(atknow, false, false));
            }
            if (skill == "venom")
            {
                opponent.playerCounters.poison++;
                opponent.UpdatePlayerIndicators();
            }
            if (skill == "scramble")
            {
                StartCoroutine(opponent.ScrambleQuanta());
            }
        }
        yield break;
    }


    public IEnumerator CreatureCheckStep()
    {
        List<Card> creatureCards = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();
        PlayerManager opponent = isPlayer ? DuelManager.enemy : DuelManager.player;

        if(opponent.playerCounters.poison != 0)
        {
            yield return StartCoroutine(opponent.ModifyHealthLogic(opponent.playerCounters.poison, true, false));
        }
        int index = 0;
        for (int i = 0; i < creatureCards.Count; i++)
        {
            Card creature = creatureCards[i];
            ID iD = creatureIds[i];

            int atkNow = creature.passive.Contains("dive") ? creature.AtkNow * 2 : creature.AtkNow;
            int adrenalineIndex = 0;
            adrenalineCheck:

            if (DuelManager.IsSundialInPlay()
                || patienceCount > 0
                || creature.Freeze > 0
                || creature.IsDelayed
                || atkNow == 0)
            { goto skipCreatureAttack; }

            bool isFreedomEffect = UnityEngine.Random.Range(0, 100) < (25 * freedomCount) && creature.costElement.Equals(Element.Air);

            if (atkNow > 0 && !isFreedomEffect)
            {
                List<ID> creaturesWithGravity = opponent.playerCreatureField.GetCreaturesWithGravity();
                if (creaturesWithGravity.Count > 0 && !creature.passive.Contains("momentum"))
                {
                    while (creaturesWithGravity.Count > 0 && atkNow > 0)
                    {
                        int gravityHP = opponent.playerCreatureField.GetCardWithID(creaturesWithGravity[0]).DefNow;
                        //Damage Creature and get leftover damage
                        Card result = opponent.playerCreatureField.DamageCreature(atkNow, creaturesWithGravity[0]);
                        if (result != null)
                        {
                            //result.Poison += poisonDamage;
                            //Update surviving creature display;
                            opponent.DisplayNewCard(creaturesWithGravity[0], result, false);

                            StartCoroutine(creatureDisplayers[iD.Index].ShowDamage(creature.AtkNow));
                            continue; // Go to creature passive check

                        }
                        else
                        {
                            //Remove Creature display
                            yield return StartCoroutine(opponent.RemoveCardFromFieldLogic(creaturesWithGravity[0]));
                            //Update List
                            creaturesWithGravity = opponent.playerCreatureField.GetCreaturesWithGravity();
                            //Subtract leftover damage
                            atkNow -= gravityHP;
                        }
                    }
                }
                if (!creature.passive.Contains("momentum"))
                {
                    opponent.ShieldCheck(creatureIds[index], ref creature, ref atkNow);
                }

                if (creature.passive.Contains("nuerotoxin"))
                {
                    opponent.playerCounters.nuerotoxin = 1;
                    opponent.UpdatePlayerIndicators();
                }
                if (atkNow != 0)
                {
                    Game_SoundManager.shared.PlayAudioClip("CreatureDamage");
                    if (creature.passive.Contains("vampire"))
                    {
                        StartCoroutine(ModifyHealthLogic(atkNow, false, false));
                    }
                    StartCoroutine(creatureDisplayers[iD.Index].ShowDamage(atkNow));

                    yield return StartCoroutine(opponent.ReceivePhysicalDamage(isFreedomEffect ? Mathf.FloorToInt(atkNow * 1.5f) : atkNow));
                }
            }

            skipCreatureAttack:
            if (adrenalineIndex < 2)
            {

                creature.AbilityUsed = false;
                if (creature.passive.Contains("dive"))
                {
                    creature.passive.Remove("dive");
                    creature.atk /= 2;
                    creature.AtkModify /= 2;
                }

                if (!creature.IsDelayed
                    && creature.passive?.Count > 0)
                {
                    if (creature.passive.Contains("air"))
                    {
                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Air);
                        StartCoroutine(GenerateQuantaLogic(Element.Air, 1));
                    }
                    if (creature.passive.Contains("earth"))
                    {
                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Earth);
                        StartCoroutine(GenerateQuantaLogic(Element.Earth, 1));
                    }
                    if (creature.passive.Contains("fire"))
                    {
                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Fire);
                        StartCoroutine(GenerateQuantaLogic(Element.Fire, 1));
                    }
                    if (creature.passive.Contains("light"))
                    {
                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Light);
                        StartCoroutine(GenerateQuantaLogic(Element.Light, 1));
                    }
                    if (creature.passive.Contains("devourer"))
                    {
                        if (opponent.GetAllQuantaOfElement(Element.Other) > 0 && opponent.sanctuaryCount == 0)
                        {
                            StartCoroutine(opponent.SpendQuantaLogic(Element.Other, 1));
                            Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Darkness);
                            StartCoroutine(GenerateQuantaLogic(Element.Darkness, 1));
                        }
                    }
                    if (creature.passive.Contains("overdrive"))
                    {
                        creature.AtkModify += 3;
                        creature.DefModify -= 1;
                    }
                    if (creature.passive.Contains("acceleration"))
                    {
                        creature.AtkModify += 2;
                        creature.DefModify -= 1;
                    }
                    if (creature.passive.Contains("infest"))
                    {
                        PlayCardOnFieldLogic(CardDatabase.GetCardFromId("4t8"));
                    }
                    if (adrenalineIndex < 1 && creature.passive.Contains("singularity"))
                    {
                        List<string> singuEffects = new List<string>();
                        if (creature.AtkNow > 0)
                        {
                            creature.atk = -(Mathf.Abs(creature.atk));
                            creature.AtkModify = -(Mathf.Abs(creature.AtkModify));
                        }

                        if (!creature.innate.Contains("immaterial"))
                        {
                            singuEffects.Add("Immaterial");
                        }
                        if (!creature.passive.Contains("adrenaline"))
                        {
                            singuEffects.Add("Addrenaline");
                        }
                        if (!creature.passive.Contains("vampire"))
                        {
                            singuEffects.Add("Vampire");
                        }
                        singuEffects.Add("Chaos");
                        singuEffects.Add("Copy");
                        singuEffects.Add("Nova");

                        switch (singuEffects[UnityEngine.Random.Range(0, singuEffects.Count)])
                        {
                            case "Immaterial":
                                creature.innate.Add("immaterial");
                                break;
                            case "Vampire":
                                creature.passive.Add("vampire");
                                break;
                            case "Chaos":
                                int chaos = UnityEngine.Random.Range(1, 6);
                                creature.AtkModify += chaos;
                                creature.DefModify += chaos;
                                break;
                            case "Nova":
                                for (int y = 0; y < 12; y++)
                                {
                                    yield return opponent.StartCoroutine(opponent.GenerateQuantaLogic((Element)i, 1));
                                }
                                break;
                            case "Addrenaline":
                                creature.passive.Add("adrenaline");
                                break;
                            default:
                                Card duplicate = new Card(creature);
                                Game_AnimationManager.shared.StartAnimation("ParallelUniverse", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD));
                                PlayCardOnFieldLogic(duplicate);
                                break;
                        }
                        yield return null;
                    }
                    if (creature.passive.Contains("swarm"))
                    {
                        creature.DefModify += scarabsPlayed;
                    }
                }

                if (creature.Freeze > 0)
                {
                    creature.Freeze--;
                }

                if (creature.Charge > 0)
                {
                    creature.Charge--;
                    creature.AtkModify--;
                    creature.DefModify--;

                }
                if (creature.IsDelayed)
                {
                    creature.innate.Remove("delay");
                }

                int healthChange = creature.Poison;
                Card result = playerCreatureField.DamageCreature(healthChange, iD);

                if (result == null)
                {
                    yield return StartCoroutine(RemoveCardFromFieldLogic(iD));
                    yield break;
                }

                creatureDisplayers[iD.Index].DisplayCard(creature, false);
            }

            if (creature.passive.Contains("adrenaline") && creature.AtkNow != 0)
            {
                if (creature.passive.Contains("venom"))
                {
                    opponent.playerCounters.poison++;
                }
                if (creature.passive.Contains("deadly venom"))
                {
                    opponent.playerCounters.poison += 2;
                }
                if (creature.passive.Contains("nuerotoxin"))
                {
                    opponent.playerCounters.nuerotoxin = 1;
                }
                opponent.UpdatePlayerIndicators();
                adrenalineIndex++;
                if (DuelManager.AdrenalineDamageList[Mathf.Abs(creature.AtkNow) - 1].Count < adrenalineIndex)
                {
                    atkNow = DuelManager.AdrenalineDamageList[Mathf.Abs(creature.AtkNow) - 1][adrenalineIndex];
                    if (creature.passive.Contains("antimatter"))
                    {
                        atkNow = -atkNow;
                    }
                    goto adrenalineCheck;
                }
            }
        }

        yield return StartCoroutine(WeaponAttackStep());
    }
}
