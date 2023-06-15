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
                break;
            case PlayerCounters.Invisibility:
                playerCounters.invisibility += amount;
                break;
            case PlayerCounters.Freeze:
                break;
            case PlayerCounters.Poison:
                break;
            case PlayerCounters.Nuerotoxin:
                break;
            case PlayerCounters.Sanctuary:
                sanctuaryCount += amount;
                break;
            case PlayerCounters.Freedom:
                break;
            case PlayerCounters.Patience:
                playerCounters.patience += amount;
                break;
            case PlayerCounters.Scarab:
                break;
            case PlayerCounters.Silence:
                playerCounters.silence += amount;
                break;
            default:
                break;
        }
        OnPlayerCounterUpdate?.Invoke(playerCounters);
    }

    public (int, int) GetChimeraStats()
    {
        List<CardInPlay> creatureInPlayList = playerCreatureField.GetAllCardsInPlay();
        (int, int) chimeraPwrHP = (0, 0);

        foreach (var creatureInPlay in creatureInPlayList)
        {
            if (creatureInPlay.ActiveCard != null)
            {
                if (creatureInPlay.ActiveCard.passive.Contains("chimera")) { continue; }
                chimeraPwrHP.Item1 += creatureInPlay.ActiveCard.AtkNow;
                chimeraPwrHP.Item2 += creatureInPlay.ActiveCard.DefNow;
                StartCoroutine(creatureInPlay.RemoveCardFromPlay());
            }
        }
        return chimeraPwrHP;
    }


    public IDCardPair playerID;


    //Logic Properties
    [SerializeField]
    private List<PlayerCardInHand> handDisplayers;

    public List<CreatureInPlay> creatureDisplayers;

    public List<PermanentInPlay> permanentDisplayers;

    public List<PassiveInPlay> passiveDisplayers;
    [SerializeField]
    private List<QuantaDisplayer> quantaDisplayers;

    internal void ShowTargetHighlight(IDCardPair id)
    {
        switch (id.id.Field)
        {
            case FieldEnum.Creature:
                creatureDisplayers[id.id.Index].ShouldShowTarget(true);
                break;
            case FieldEnum.Passive:
                passiveDisplayers[id.id.Index].ShouldShowTarget(true);
                break;
            case FieldEnum.Permanent:
                permanentDisplayers[id.id.Index].ShouldShowTarget(true);
                break;
            case FieldEnum.Player:
                playerDisplayer.ShouldShowTarget(true);
                break;
            default:
                break;
        }
    }

    [SerializeField]
    private DeckDisplayer deckDisplayer;

    public HealthDisplayer healthDisplayer;
    [SerializeField]
    private GameObject cloakVisual;
    [SerializeField]
    private Transform permParent;
    private int scarabsPlayed = 0;

    public int GetPossibleDamage()
    {
        int value = 0;
        List<Card> creatures = playerCreatureField.GetAllCards();

        foreach (var item in creatures)
        {
            value += item.AtkNow;
        }
        Card weapon = playerPassiveManager.GetWeapon().card;
        if (weapon != null)
        {
            if (weapon.cardName != "Weapon") { value += weapon.AtkNow; }
        }
        return value;
    }

    internal void ManageGravityCreatures(out int atkNow, out Card card)
    {
        throw new NotImplementedException();
    }

    [SerializeField]
    private CardDetailView cardDetailView;

    internal void ManageShield(out int atkNow, out Card card)
    {
        throw new NotImplementedException();
    }

    public PlayerDisplayer playerDisplayer;

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

    public bool isPlayer;


    private float animSpeed;
    public int sanctuaryCount = 0;
    public int patienceCount = 0;
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
        foreach (var iDCard in playerCreatureField.pairList)
        {
            if(!iDCard.HasCard()) { continue; }
            if(iDCard.card.costElement.Equals(Element.Darkness) || iDCard.card.costElement.Equals(Element.Death))
            {
                iDCard.card.AtkModify += atk;
                iDCard.card.DefModify += hp;
                iDCard.UpdateCard();
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
        TurnDownTick();
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

        var idList = playerPermanentManager.GetAllValidCardIds();

        foreach (var idCard in idList)
        {
            idCard.card.AbilityUsed = false;
            idCard.UpdateCard();
        }

        Card shield = playerPassiveManager.GetShield().card;
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


    public IEnumerator ManageID(IDCardPair idCard)
    {
        if (!BattleVars.shared.isPlayerTurn) { yield break; }
        if (idCard.card == null) { yield break; }

        if (idCard.id.Owner.Equals(OwnerEnum.Opponent))
        {
            if (idCard.id.Field.Equals(FieldEnum.Hand)) { yield break; }

            if (DuelManager.Instance.enemy.playerCounters.invisibility > 0 && !DuelManager.Instance.enemy.cloakIndex.Contains(idCard.id)) { yield break; }
            cardDetailManager.SetCardOnDisplay(idCard);
            //cardDetailView.SetupCardDisplay(iD, DuelManager.GetCard(iD), false);
            yield break;
        }

        if (BattleVars.shared.hasToDiscard)
        {
            if (idCard.id.Field.Equals(FieldEnum.Hand))
            {
                DiscardCard(idCard.id);
                BattleVars.shared.hasToDiscard = false;
                DuelManager.Instance.discardText.gameObject.SetActive(false);
                yield return StartCoroutine(DuelManager.Instance.EndTurn());
                yield break;
            }
        }

        if (PlayerPrefs.GetInt("QuickPlay") == 0)
        {
            if (idCard.id.Field.Equals(FieldEnum.Player)) { yield break; }
            if (idCard.id.Field.Equals(FieldEnum.Hand) && playerQuantaManager.HasEnoughQuanta(idCard.card.costElement, idCard.card.cost))
            {
                if (playerCounters.silence > 0 && sanctuaryCount == 0) { yield break; }
                if (!idCard.card.cardType.Equals(CardType.Spell))
                {
                    PlayCardFromHandLogic(idCard.id);
                }
                else if (IsSpellPlayable(idCard.card))
                {
                    BattleVars.shared.abilityOrigin = idCard;
                    if (!SkillManager.Instance.ShouldAskForTarget(idCard))
                    {
                        SkillManager.Instance.SkillRoutineNoTarget(this, idCard);
                        if (idCard.card.cardType.Equals(CardType.Spell)) { PlayCardFromHandLogic(idCard.id); }

                        if (idCard.card.skill != "photosynthesis")
                        {
                            idCard.card.AbilityUsed = true;
                        }
                        SpendQuantaLogic(idCard.card.skillElement, idCard.card.skillCost);
                    }
                    else
                    {
                        BattleVars.shared.isSelectingTarget = true;
                        SkillManager.Instance.SetupTargetHighlights(this, idCard);
                    }
                }
            }
            else if (IsAbilityUsable(idCard.card) && !idCard.id.Field.Equals(FieldEnum.Hand))
            {
                BattleVars.shared.abilityOrigin = idCard;
                if (!SkillManager.Instance.ShouldAskForTarget(idCard))
                {
                    SkillManager.Instance.SkillRoutineNoTarget(this, idCard);
                    if (idCard.card.cardType.Equals(CardType.Spell)) { PlayCardFromHandLogic(idCard.id); }

                    if (idCard.card.skill != "photosynthesis")
                    {
                        idCard.card.AbilityUsed = true;
                    }
                    SpendQuantaLogic(idCard.card.skillElement, idCard.card.skillCost);
                }
                else
                {
                    BattleVars.shared.isSelectingTarget = true;
                    SkillManager.Instance.SetupTargetHighlights(this, idCard);
                }
            }

            yield break;
        }


        cardDetailManager.SetCardOnDisplay(idCard);
    }

    internal void DisplayHand()
    {
        List<Card> cards = playerHand.GetAllCards();

        for (int i = 0; i < cards.Count; i++)
        {
            handDisplayers[i].ShowCardForPrecog(cards[i]);
        }
        shouldHideCards = true;
    }
    internal void HideHand()
    {
        if (!shouldHideCards) { return; }
        List<Card> cards = playerHand.GetAllCards();

        for (int i = 0; i < cards.Count; i++)
        {
            handDisplayers[i].HideCardForPrecog();
        }
        shouldHideCards = false;
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
                PlayCardFromHandLogic(cardDetailManager.GetCardID().id);
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
            PlayCardFromHandLogic(BattleVars.shared.abilityOrigin.id);

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.shared.abilityOrigin))
            {
                SkillManager.Instance.SkillRoutineWithTarget(this, target);
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.shared.abilityOrigin);
            }
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
            if (playerPassiveManager.GetWeapon().card.iD == "4t2") { return false; }
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
                    //yield return DuelManager.s.Occured();
                    GetCard(cardID).cardName.Contains("Skeleton");
                    GetCard(cardID).cardName.Contains("Skeleton");
                }
                if (GetCard(cardID).passive?.Count > 0)
                {
                    if (GetCard(cardID).IsAflatoxin)
                    {
                        //DisplayNewCard(cardID, CardDatabase.Instance.GetCardFromId("6ro"));
                        yield break;
                    }
                    if (GetCard(cardID).passive.Contains("phoenix"))
                    {
                        bool shouldRebirth = true;
                        if (BattleVars.shared.abilityOrigin != null)
                        {
                            if (BattleVars.shared.abilityOrigin.card.skill == "reverse time")
                            {
                                shouldRebirth = false;
                            }
                        }
                        if (shouldRebirth)
                        {
                            //DisplayNewCard(cardID, CardDatabase.Instance.GetCardFromId(GetCard(cardID).iD.IsUpgraded() ? "7dt" : "5fd"));
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
                        PlayCardOnFieldLogic(CardDatabase.Instance.GetPlaceholderCard(cardID.Index));
                    }
                    else
                    {
                        boneShieldLabel.text = $"{playerCounters.bone}";
                    }
                }
                else
                {
                    playerPassiveManager.DestroyCard(cardID.Index);
                    PlayCardOnFieldLogic(CardDatabase.Instance.GetPlaceholderCard(cardID.Index));
                    //passiveDisplayers[cardID.Index].PlayDissolveAnimation();
                }
                break;
            case FieldEnum.Permanent:
                StartCoroutine(CheckOnPlayAbility(GetCard(cardID), cardID, false));
                for (int i = 0; i < amount; i++)
                {
                    playerPermanentManager.DestroyCard(cardID.Index);
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
            deckManager.AddCardToTop(CardDatabase.Instance.GetCardFromId(newCard.iD));
            DrawCardFromDeckLogic();
        }
    }

    //Modify Health Logic and Visual Command Pair
    public void ModifyHealthLogic(int amount, bool isDamage, bool fromSpell)
    {
        if (sacrificeCount > 0) { isDamage = !isDamage; }
        int newHealth = healthManager.ModifyHealth(amount, isDamage);
    }

    [SerializeField]
    private Sprite poisonSprite, puritySprite, nuerotoxinSprite;
    public Image poisonImg, sanctImage, silenceImage;

    public void UpdatePlayerIndicators()
    {
        if (playerCounters.poison != 0)
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

        if (playerCounters.invisibility != 0)
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
    private bool shouldHideCards;

    //public void DisplayNewCard(ID cardID, Card cardPlayed, bool shouldAnim = true, bool isPlayed = false)
    //{
    //    switch (cardID.Field)
    //    {
    //        case FieldEnum.Hand:
    //            playerHand.AddCardToHand(cardPlayed);
    //            break;
    //        case FieldEnum.Creature:
    //            playerCreatureField.pairList[cardID.Index].card = cardPlayed;
    //            creatureDisplayers[cardID.Index].DisplayCard(cardPlayed, shouldAnim);
    //            if (cardPlayed.DefNow <= 0 && !isPlayed) { StartCoroutine(RemoveCardFromFieldLogic(cardID)); return; }
    //            break;
    //        case FieldEnum.Passive:
    //            playerPassiveManager.PlayPassive(cardPlayed);
    //            if (cardPlayed.cardType.Equals(CardType.Shield))
    //            {
    //                if (cardPlayed.iD == "71b" || cardPlayed.iD == "52r")
    //                {
    //                    boneShieldLabel.text = $"{playerCounters.bone}";
    //                }
    //                else if (cardPlayed.iD == "5oo" || cardPlayed.iD == "7n8" || cardPlayed.iD == "80d" || cardPlayed.iD == "61t")
    //                {
    //                    boneShieldLabel.text = cardPlayed.TurnsInPlay.ToString();
    //                }
    //                else if (cardPlayed.iD == "5lk" || cardPlayed.iD == "7k4")
    //                {
    //                    int bioCreatures = GetLightEmittingCreatures();
    //                    bioCreatures += cardPlayed.iD == "7k4" ? 1 : 0;
    //                    boneShieldLabel.text = $"{bioCreatures}";
    //                }

    //            }
    //            break;
    //        case FieldEnum.Permanent:
    //            playerPermanentManager.PlayPermanent(cardPlayed);
    //            break;
    //        default:
    //            break;
    //    }
    //}

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
            SpendQuantaLogic(cardPlayed.costElement, cardPlayed.cost);
        }

        DisplayPlayableGlow();
    }

    internal void ModifyMaxHealthLogic(int maxHPBuff, bool isIncrease)
    {
        int newMax = healthManager.ModifyMaxHealth(maxHPBuff, isIncrease);
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

        Card weaponCard = playerPassiveManager.GetWeapon().card;
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
                PlayerManager voodooOwner = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;

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
                DuelManager.GetNotIDOwner(target).ModifyHealthLogic(-modifyHP, true, false);

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
                    DuelManager.GetNotIDOwner(target).ModifyHealthLogic(damage, true, false);
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

        //DisplayNewCard(target, creature, false);
    }

    public void ActivateDeathTriggers(Card deadCard, int notUsed = 0)
    {
        if (deadCard == null) { return; }
        if (deadCard.cardType != CardType.Creature) { return; }
        var cardsToCheck = playerCreatureField.GetAllValidCardIds();
        if (cardsToCheck.Count > 0)
        {
            foreach (var creature in cardsToCheck)
            {
                if (creature.card.passive.Contains("scavenger"))
                {
                    creature.card.AtkModify += 1;
                    creature.card.DefModify += 1;
                    creature.UpdateCard();
                }
            }
        }
        cardsToCheck = playerPermanentManager.GetAllValidCardIds();
        if (cardsToCheck.Count > 0)
        {
            foreach (var perm in cardsToCheck)
            {
                if (perm.card.skill == "soul catch")
                {
                    GenerateQuantaLogic(Element.Death, perm.card.iD.IsUpgraded() ? 3 : 2);
                }
                if (perm.card.skill == "boneyard" && deadCard.iD != "716" && deadCard.iD != "52m")
                {
                    PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId(perm.card.iD.IsUpgraded() ? "716" : "52m"));
                }
            }
        }
        if (playerPassiveManager.GetShield().card.skill == "bones" && deadCard.iD != "716" && deadCard.iD != "52m")
        {
            playerCounters.bone += 2;
            boneShieldLabel.text = $"{playerCounters.bone}";
        }
    }

    private void ActivateCloakEffect(ID location)
    {
        if (isPlayer) { return; }
        cloakVisual.SetActive(true);
        Transform cloakPerm = Battlefield_ObjectIDManager.shared.GetObjectFromID(location);
        cloakPerm.parent.transform.parent = cloakVisual.transform;

    }
    private void DeactivateCloakEffect(ID location)
    {
        if (isPlayer) { return; }
        cloakVisual.SetActive(false);
        Transform cloakPerm = Battlefield_ObjectIDManager.shared.GetObjectFromID(location);
        cloakPerm.parent.transform.parent = permParent.transform;
        cloakPerm.SetSiblingIndex(location.Index);


    }
    private IEnumerator CheckOnPlayAbility(Card card, ID location, bool isPlayed)
    {
        if (card.skill == "" || card.skill == "none") { yield break; }
        List<Card> creatures = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();
        if (isPlayed)
        {
            switch (card.iD)
            {
                case "7q9":
                case "5rp":
                    card.TurnsInPlay = 1;
                    break;
                case "7n8":
                case "5oo":
                    card.TurnsInPlay = 5;
                    break;
                case "61t":
                case "80d":
                case "5v2":
                case "7ti":
                    card.TurnsInPlay = 3;
                    break;
                default:
                    break;
            }

            switch (card.skill)
            {
                case "eclipse":
                case "nightfall":
                    DuelManager.Instance.UpdateNightFallEclipse(true, card.skill);
                    break;
                case "bones":
                    playerCounters.bone = BattleVars.shared.abilityOrigin == null ? 7 : 1;
                    boneShieldLabel.text = $"{playerCounters.bone}";
                    break;
                case "patience":
                    patienceCount++;
                    break;
                case "freedom":
                    playerCounters.freedom++;
                    break;
                case "cloak":
                    playerCounters.invisibility = 3;
                    card.TurnsInPlay = 3;
                    ActivateCloakEffect(location);
                    break;
                case "flood":
                    DuelManager.Instance.AddFloodCount(1);
                    break;
                default:
                    break;
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
            yield break;
        }


        if (card.skill == "eclipse" || card.skill == "nightfall")
        {
            DuelManager.Instance.UpdateNightFallEclipse(false, card.skill);
        }
        if (card.innate.Contains("swarm"))
        {
            scarabsPlayed--;
        }

        if (card.skill == "cloak")
        {
            if (playerPermanentManager.GetAllCards().FindAll(x => x.skill == "cloak").Count == 1)
            {
                DeactivateCloakEffect(location);
                playerCounters.invisibility = 0;
            }
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
            playerCounters.freedom--;
        }
        if (card.skill == "flood")
        {
            DuelManager.Instance.AddFloodCount(-1);
        }
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
    public ID PlayCardOnFieldLogic(Card card)
    {
        ID newLocationId = PlayCardOnField(card);
        if (newLocationId == null) { return null; }
        StartCoroutine(CheckOnPlayAbility(card, newLocationId, true));
        card.AbilityUsed = true;
        if ((card.costElement.Equals(Element.Darkness) || card.costElement.Equals(Element.Death)) && card.cardType.Equals(CardType.Creature))
        {
            if (DuelManager.IsEclipseInPlay())
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
        return newLocationId;
    }

    public void DiscardCard(IDCardPair cardToDiscard)
    {
        cardToDiscard.RemoveCard();
        playerHand.UpdateHandVisual();
        BattleVars.shared.hasToDiscard = false;
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
                    GenerateQuantaLogic(rnd.element, rnd.count);
                }
            }
            else
            {
                GenerateQuantaLogic(item.Item1.element, item.Item1.count);
            }
        }

        PlayActionAnimationVisual(playerPassiveManager.GetMarkID());
        yield return StartCoroutine(Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(playerPassiveManager.GetMarkID()), playerPassiveManager.GetMark().card.costElement));


        if (BattleVars.shared.enemyAiData.maxHP >= 150 && !isPlayer)
        {
            GenerateQuantaLogic(playerPassiveManager.GetMark().card.costElement, 3);
        }
        else
        {
            GenerateQuantaLogic(playerPassiveManager.GetMark().card.costElement, 1);
        }
        yield return StartCoroutine(CheckPermanentEndTurn());
    }

    public void SpendQuantaLogic(Element element, int amount)
    {

        if ((isPlayer && !BattleVars.shared.isPlayerTurn) || (!isPlayer && BattleVars.shared.isPlayerTurn))
        {
            if (sanctuaryCount > 0)
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
            if (sanctuaryCount > 0)
            {
                return;
            }
        }
        playerQuantaManager.ChangeQuanta(element, amount, true);
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
        playerPassiveManager = new PassiveManager(idPassiveList, passiveDisplayers);
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

        List<Card> deck = new List<Card>(isPlayer ?
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

    public void ReceivePhysicalDamage(int damage)
    {
        if (damage == 0) { return; }
        //Damage player with leftover damage or direct damage
        ModifyHealthLogic(damage, true, false);
    }

    private IEnumerator CheckPermanentEndTurn()
    {
        List<int> floodList = new List<int> { 11, 13, 9, 10, 12 };
        var playerPermanents = playerPermanentManager.GetAllCards();
        if (playerPermanents.Count == 0)
        {
            yield return StartCoroutine(CreatureCheckStep());
            yield break;
        }

        foreach (var permanent in playerPermanents)
        {
            switch (permanent.skill)
            {
                case "sactuary":
                    ModifyHealthLogic(4, false, false);
                    break;
                case "void":
                    int healthChange = playerPassiveManager.GetMark().card.costElement == Element.Darkness ? 3 : 2;
                    var targetPlayer = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
                    targetPlayer.ModifyMaxHealthLogic(healthChange, false);
                    break;
                case "gratitude":
                    int healthAmount = playerPassiveManager.GetMark().card.costElement == Element.Life ? 5 : 3;
                    ModifyHealthLogic(healthAmount, false, false);
                    break;
                case "empathy":
                    int creatureCount = playerCreatureField.GetAllCards().Count;
                    ModifyHealthLogic(creatureCount, false, false);
                    break;
                case "flood":
                    DuelManager.Instance.enemy.ClearFloodedArea(floodList);
                    DuelManager.Instance.player.ClearFloodedArea(floodList);
                    break;
                case "patience":
                    List<Card> creatures = playerCreatureField.GetAllCards();
                    List<ID> ids = playerCreatureField.GetAllIds();
                    for (int i = 0; i < creatures.Count; i++)
                    {
                        int statModifier = DuelManager.IsFloodInPlay() && floodList.Contains(ids[i].Index) ? 5 : 2;
                        creatures[i].AtkModify += statModifier;
                        creatures[i].DefModify += statModifier;
                    }
                    break;
            }
        }
        yield return StartCoroutine(CreatureCheckStep());
    }
    public void ShieldCheck(ID attackerID, ref Card attacker, ref int atknow)
    {
        PlayerManager opponent = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
        IDCardPair shield = playerPassiveManager.GetShield();
        string skill = shield.card.skill;

        if (skill == "none" || atknow == 0)
        {
            return;
        }

        if (attacker.passive.Contains("psion"))
        {
            if (skill == "reflect")
            {
                opponent.ModifyHealthLogic(atknow, true, false);
                atknow = 0;
            }
            else
            {
                return;
            }
        }

        switch (skill)
        {
            case "phaseshift":
                atknow = 0;
                break;
            case "solar":
                GenerateQuantaLogic(Element.Light, 1);
                break;
            case "weight":
                if (attacker.cardType == CardType.Creature && attacker.DefNow > 5)
                {
                    atknow = 0;
                }
                break;
            case "wings":
                if (!attacker.innate.Contains("airborne") && !attacker.innate.Contains("ranged"))
                {
                    atknow = 0;
                }
                break;
            case "delay":
                attacker.innate.Add("delay");
                break;
            case "ice":
                attacker.Freeze = UnityEngine.Random.Range(0f, 1f) <= 0.3f ? 3 : 0;
                break;
            case "spines":
                if (UnityEngine.Random.Range(0f, 1f) <= 0.75f)
                {
                    attacker.Poison++;
                }
                break;
            case "firewall":
                attacker.DefDamage++;
                break;
            case "fog":
                atknow = UnityEngine.Random.Range(0f, 1f) <= 0.4f ? 0 : atknow;
                break;
            case "dusk":
                atknow = UnityEngine.Random.Range(0f, 1f) <= 0.5f ? 0 : atknow;
                break;
            case "unholy":
                if (!attacker.IsAflatoxin && attacker.iD != "716" && attacker.iD != "52m" &&
                    UnityEngine.Random.Range(0f, 1f) <= (0.5f / attacker.DefNow) && atknow > 0 &&
                    attacker.cardType == CardType.Creature)
                {
                    opponent.StartCoroutine(opponent.RemoveCardFromFieldLogic(attackerID));
                    opponent.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId(attacker.iD.IsUpgraded() ? "716" : "52m"));
                }
                break;
            case "bones":
                atknow = 0;
                playerCounters.bone -= 1;
                if (playerCounters.bone <= 0)
                {
                    playerPassiveManager.DestroyCard(playerPassiveManager.GetShieldID().Index);
                    PlayCardOnFieldLogic(CardDatabase.Instance.GetPlaceholderCard(playerPassiveManager.GetShieldID().Index));
                }
                else
                {
                    boneShieldLabel.text = $"{playerCounters.bone}";
                }
                break;
            case "dissipation":
                if(sanctuaryCount > 0) { return; }
                int allQuanta = GetAllQuantaOfElement(Element.Other);
                if (allQuanta >= atknow)
                {
                    SpendQuantaLogic(Element.Other, atknow);
                    atknow = 0;
                }
                else
                {
                    StartCoroutine(RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID(), 1, false));
                }
                break;
            case "edissipation":
                if (sanctuaryCount > 0) { return; }
                    int quantaToUse = Mathf.CeilToInt(atknow / 3);
                    int availableEQuanta = GetAllQuantaOfElement(Element.Entropy);
                    if (availableEQuanta >= quantaToUse)
                    {
                    SpendQuantaLogic(Element.Other, quantaToUse);
                        atknow = 0;
                    }
                    else
                    {
                        StartCoroutine(RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID(), 1, false));
                    }
                break;
        }
    }
    public void NewShieldCheck(PlayerManager attackerOwner, ref Card attacker, ref int atknow)
    {
        //PlayerManager opponent = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
        IDCardPair shield = playerPassiveManager.GetShield();

        if (shield.card == null) { return; }

        string skill = shield.card.skill;

        if (skill == "none") { return; }

        if (attacker.passive.Contains("psion"))
        {
            if (skill == "reflect")
            {
                attackerOwner.ModifyHealthLogic(atknow, true, false);
                atknow = 0;
            }
            else
            {
                return;
            }
        }

        switch (skill)
        {
            case "phaseshift":
                atknow = 0;
                break;
            case "solar":
                GenerateQuantaLogic(Element.Light, 1);
                break;
            case "weight":
                if (attacker.cardType == CardType.Creature && attacker.DefNow > 5)
                {
                    atknow = 0;
                }
                break;
            case "wings":
                if (!attacker.innate.Contains("airborne") && !attacker.innate.Contains("ranged"))
                {
                    atknow = 0;
                }
                break;
            case "delay":
                attacker.innate.Add("delay");
                break;
            case "ice":
                attacker.Freeze = UnityEngine.Random.Range(0f, 1f) <= 0.3f ? 3 : 0;
                break;
            case "spines":
                if (UnityEngine.Random.Range(0f, 1f) <= 0.75f)
                {
                    attacker.Poison++;
                }
                break;
            case "firewall":
                attacker.DefDamage++;
                break;
            case "fog":
                atknow = UnityEngine.Random.Range(0f, 1f) <= 0.4f ? 0 : atknow;
                break;
            case "dusk":
                atknow = UnityEngine.Random.Range(0f, 1f) <= 0.5f ? 0 : atknow;
                break;
            case "unholy":
                if (!attacker.IsAflatoxin && attacker.iD != "716" && attacker.iD != "52m" &&
                    UnityEngine.Random.Range(0f, 1f) <= (0.5f / attacker.DefNow) && atknow > 0 &&
                    attacker.cardType == CardType.Creature)
                {
                    attackerOwner.StartCoroutine(attackerOwner.RemoveCardFromFieldLogic(new(1,1,1)));
                    attackerOwner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId(attacker.iD.IsUpgraded() ? "716" : "52m"));
                }
                break;
            case "bones":
                atknow = 0;
                playerCounters.bone -= 1;
                if (playerCounters.bone <= 0)
                {
                    playerPassiveManager.DestroyCard(playerPassiveManager.GetShieldID().Index);
                    PlayCardOnFieldLogic(CardDatabase.Instance.GetPlaceholderCard(playerPassiveManager.GetShieldID().Index));
                }
                else
                {
                    boneShieldLabel.text = $"{playerCounters.bone}";
                }
                break;
            case "dissipation":
                if (sanctuaryCount > 0) { return; }
                int allQuanta = GetAllQuantaOfElement(Element.Other);
                if (allQuanta >= atknow)
                {
                    SpendQuantaLogic(Element.Other, atknow);
                    atknow = 0;
                }
                else
                {
                    StartCoroutine(RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID(), 1, false));
                }
                break;
            case "edissipation":
                if (sanctuaryCount > 0) { return; }
                int quantaToUse = Mathf.CeilToInt(atknow / 3);
                int availableEQuanta = GetAllQuantaOfElement(Element.Entropy);
                if (availableEQuanta >= quantaToUse)
                {
                    SpendQuantaLogic(Element.Other, quantaToUse);
                    atknow = 0;
                }
                else
                {
                    StartCoroutine(RemoveCardFromFieldLogic(playerPassiveManager.GetShieldID(), 1, false));
                }
                break;
        }
    }
    public IEnumerator WeaponAttackStep()
    {
        HideHand();
        PlayerManager opponent = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
        IDCardPair weapon = playerPassiveManager.GetWeapon();

        if (weapon.card.iD == "6ri") { yield break; }
        string skill = weapon.card.skill;
        int atknow = weapon.card.AtkNow;
        if (skill == "fiery")
        {
            atknow += Mathf.FloorToInt(GetAllQuantaOfElement(Element.Fire) / 5);

        }
        if (skill == "hammer" && (playerPassiveManager.GetMark().card.costElement == Element.Earth || playerPassiveManager.GetMark().card.costElement == Element.Gravity))
        {
            atknow++;
        }
        if (skill == "dagger" && (playerPassiveManager.GetMark().card.costElement == Element.Death || playerPassiveManager.GetMark().card.costElement == Element.Darkness))
        {
            atknow++;
        }
        if (skill == "bow" && playerPassiveManager.GetMark().card.costElement == Element.Air)
        {
            atknow++;
        }

        if (weapon.card.Freeze > 0)
        {
            atknow = 0;
            weapon.card.Freeze--;
        }

        if (weapon.card.IsDelayed)
        {
            atknow = 0;
            weapon.card.innate.Remove("delay");
        }

        if (!weapon.card.passive.Contains("momentum"))
        {
            opponent.ShieldCheck(playerPassiveManager.GetWeaponID(), ref weapon.card, ref atknow);
        }

        //Send Damage
        opponent.ModifyHealthLogic(atknow, true, false);

        if (atknow > 0)
        {
            if (skill == "vampire")
            {
                ModifyHealthLogic(atknow, false, false);
            }
            if (skill == "venom")
            {
                opponent.playerCounters.poison++;
                opponent.UpdatePlayerIndicators();
            }
            if (skill == "scramble")
            {
                opponent.ScrambleQuanta();
            }
        }
        yield break;
    }

    //public IEnumerator CreatureCheckStepNew()
    //{
    //    //Deal Poison Damage to Opponent
    //    PlayerManager opponent = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
    //    if (opponent.playerCounters.poison != 0)
    //    {
    //        yield return StartCoroutine(opponent.ModifyHealthLogic(opponent.playerCounters.poison, true, false));
    //    }

    //    //Activate creatures end turn actions (Attack, end turn effects)
    //    if (_creatureEventHandler != null)
    //    {
    //        //Invokes all planned actions and waits until they're done.
    //        foreach (var @delegate in _creatureEventHandler.GetInvocationList())
    //        {
    //            yield return @delegate.DynamicInvoke();
    //        }
    //    }
    //    yield return StartCoroutine(WeaponAttackStep());
    //}
        public IEnumerator CreatureCheckStep()
    {
        List<Card> creatureCards = playerCreatureField.GetAllCards();
        List<ID> creatureIds = playerCreatureField.GetAllIds();
        PlayerManager opponent = isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;

        if (opponent.playerCounters.poison != 0)
        {
            opponent.ModifyHealthLogic(opponent.playerCounters.poison, true, false);
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

            bool isFreedomEffect = UnityEngine.Random.Range(0, 100) < (25 * playerCounters.freedom) && creature.costElement.Equals(Element.Air);

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
                            //opponent.DisplayNewCard(creaturesWithGravity[0], result, false);

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
                    if (creature.passive.Contains("vampire"))
                    {
                        ModifyHealthLogic(atkNow, false, false);
                    }
                    StartCoroutine(creatureDisplayers[iD.Index].ShowDamage(atkNow));

                    opponent.ReceivePhysicalDamage(isFreedomEffect ? Mathf.FloorToInt(atkNow * 1.5f) : atkNow);
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
                        GenerateQuantaLogic(Element.Air, 1);
                    }
                    if (creature.passive.Contains("earth"))
                    {
                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Earth);
                        GenerateQuantaLogic(Element.Earth, 1);
                    }
                    if (creature.passive.Contains("fire"))
                    {
                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Fire);
                        GenerateQuantaLogic(Element.Fire, 1);
                    }
                    if (creature.passive.Contains("light"))
                    {
                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Light);
                        GenerateQuantaLogic(Element.Light, 1);
                    }
                    if (creature.passive.Contains("devourer"))
                    {
                        if (opponent.GetAllQuantaOfElement(Element.Other) > 0 && opponent.sanctuaryCount == 0)
                        {
                            opponent.SpendQuantaLogic(Element.Other, 1);
                            Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Darkness);
                            GenerateQuantaLogic(Element.Darkness, 1);
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
                        PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4t8"));
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
                                    opponent.GenerateQuantaLogic((Element)i, 1);
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

                }
                if (creature.IsDelayed)
                {
                    creature.innate.Remove("delay");
                }

                int healthChange = creature.Poison;
                creature.DefDamage += healthChange;
                if (creature.DefDamage < 0) { creature.DefDamage = 0; }

                if (creature.DefNow <= 0)
                {
                    yield return StartCoroutine(RemoveCardFromFieldLogic(iD));
                    continue;
                }

                creatureDisplayers[iD.Index].DisplayCard(creature, false);
            }

            if (creature.AtkNow != 0)
            {
                if (creature.passive.Contains("adrenaline"))
                {
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
        }

        yield return StartCoroutine(WeaponAttackStep());
    }


    public void SetupCardDisplay(IDCardPair iDCardPair)
    {
        cardDetailManager.SetCardOnDisplay(iDCardPair);
    }

    public void QuickPlay(IDCardPair iDCardPair)
    {
        if (iDCardPair.IsFromHand())
        {
            if (playerCounters.silence > 0 && sanctuaryCount == 0) { return; }
            if (!IsCardPlayable(iDCardPair.card))
            {
                SetupCardDisplay(iDCardPair);
                return;
            }

            if (!iDCardPair.card.cardType.Equals(CardType.Spell))
            {
                PlayCardFromHandLogic(iDCardPair.id);
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
            PlayCardFromHandLogic(iDCardPair.id);
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
    Nuerotoxin,
    Sanctuary,
    Freedom,
    Patience,
    Scarab,
    Silence,
    Sacrifice,
    Purify
}