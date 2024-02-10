using Elements.Duel.Manager;
using Elements.Duel.Visual;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private EventBinding<ModifyPlayerCounterEvent> _modifyPlayerCounterBinding;
    private EventBinding<ModifyPlayerHealthEvent> _modifyPlayerHealthBinding;
    private EventBinding<PlayCardFromHandEvent> _playCardOnFieldBinding;
    private EventBinding<ActivateSpellOrAbilityEvent> _activateSpellOrAbilityBinding;

    private void OnDisable() {
        EventBus<ModifyPlayerCounterEvent>.Unregister(_modifyPlayerCounterBinding);
        EventBus<ModifyPlayerHealthEvent>.Unregister(_modifyPlayerHealthBinding);
        EventBus<PlayCardFromHandEvent>.Unregister(_playCardOnFieldBinding);
        EventBus<ActivateSpellOrAbilityEvent>.Unregister(_activateSpellOrAbilityBinding);
    }

    private void OnEnable()
    {
        _modifyPlayerCounterBinding = new EventBinding<ModifyPlayerCounterEvent>(ModifyCounters);
        EventBus<ModifyPlayerCounterEvent>.Register(_modifyPlayerCounterBinding);

        _modifyPlayerHealthBinding = new EventBinding<ModifyPlayerHealthEvent>(ModifyHealthLogic);
        EventBus<ModifyPlayerHealthEvent>.Register(_modifyPlayerHealthBinding);

        _playCardOnFieldBinding = new EventBinding<PlayCardFromHandEvent>(PlayCardFromHand);
        EventBus<PlayCardFromHandEvent>.Register(_playCardOnFieldBinding);

        _activateSpellOrAbilityBinding = new EventBinding<ActivateSpellOrAbilityEvent>(ActivateAbility);
        EventBus<ActivateSpellOrAbilityEvent>.Register(_activateSpellOrAbilityBinding);
    }

    private void ModifyHealthLogic(ModifyPlayerHealthEvent modifyPlayerHealthEvent)
    {
        if (!modifyPlayerHealthEvent.Target.Equals(Owner)) return;

        if (sacrificeCount > 0) { modifyPlayerHealthEvent.IsDamage.Toggle(); }

        var damage = modifyPlayerHealthEvent.IsDamage
            ? -modifyPlayerHealthEvent.Amount
            : modifyPlayerHealthEvent.Amount;
        if (playerPassiveManager.GetShield().Item2.skill is "reflect" && modifyPlayerHealthEvent.FromSpell)
        {
            EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(damage, Owner.Not(), false));
            return;
        }

        EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(damage, Owner, false));
    }

    private void ModifyCounters(ModifyPlayerCounterEvent modifyPlayerCounterEvent)
    {
        if (!modifyPlayerCounterEvent.Owner.Equals(Owner)) return;
        var amount = modifyPlayerCounterEvent.Amount;

        switch (modifyPlayerCounterEvent.Counter)
        {
            case PlayerCounters.Bone:
                EventBus<SetBoneCountEvent>.Raise(new SetBoneCountEvent(Owner, amount));
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
        EventBus<UpdatePlayerCountersVisualEvent>.Raise(new UpdatePlayerCountersVisualEvent(playerID, playerCounters));
    }

    public void RemoveAllCloaks()
    {
        foreach (var perm in playerPermanentManager.GetAllValidCardIds().Where(perm => perm.card.iD is "5v2" or "7ti"))
        {
            ResetCloakPermParent(perm);
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(perm.Item1));
            DeactivateCloakEffect();
        }
    }

    public ID playerID;

    [SerializeField]
    private DeckDisplayer deckDisplayer;

    public HealthDisplayer healthDisplayer;
    [SerializeField]
    private GameObject cloakVisual;
    [SerializeField]
    private Transform permParent;

    public int GetPossibleDamage()
    {
        var creatures = playerCreatureField.GetAllValidCardIds();

        var value = creatures.Sum(item => item.Item2.AtkNow);
        var weapon = playerPassiveManager.GetWeapon().Item2;
        if (weapon.cardName != "Weapon") { value += weapon.AtkNow; }
        return value;
    }

    public void ManageGravityCreatures(ref int atkNow)
    {
        var gravityCreature = playerCreatureField.GetCreatureWithGravity();
        if (gravityCreature.Equals(default)) return;

        if (gravityCreature.Item2.DefNow >= atkNow)
        {
            gravityCreature.Item2.DefModify -= atkNow;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(gravityCreature.Item1, gravityCreature.Item2, true));
            atkNow = 0;
            return;
        }

        atkNow -= gravityCreature.Item2.DefNow;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(gravityCreature.Item1));
    }

    [SerializeField]
    private CardDetailView cardDetailView;

    public int ManageShield(int atkNow, (ID, Card) card)
    {
        var shield = playerPassiveManager.GetShield();
        atkNow -= shield.card.DefNow;
        if (shield.Item2.skill == "") { return atkNow; }
        var shieldSkill = shield.Item2.skill.GetShieldScript<ShieldAbility>();

        return shieldSkill.ActivateShield(atkNow, card);
    }

    public PlayerDisplayer playerDisplayer;

    public HandManager playerHand;
    public CreatureManager playerCreatureField;
    public PermanentManager playerPermanentManager;
    public PassiveManager playerPassiveManager;

    public QuantaManager PlayerQuantaManager;
    public DeckManager DeckManager;
    public HealthManager HealthManager;
    public Counters playerCounters;

    public OwnerEnum Owner;
    public void ClearFloodedArea(List<int> safeZones)
    {
        if (DuelManager.FloodCount <= 0) return;
        var idList = playerCreatureField.GetAllValidCardIds();
        foreach (var idCard in idList)
        {
            if (safeZones.Contains(idCard.Item1.index)) { continue; }
            if (idCard.Item2.costElement.Equals(Element.Other) || idCard.Item2.costElement.Equals(Element.Water)) { continue; }
            if (idCard.Item2.innateSkills.Immaterial || idCard.Item2.passiveSkills.Burrow) { continue; }
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(idCard.Item1));
        }
    }

    public bool HasSufficientQuanta(Element element, int cost)
    {
        return PlayerQuantaManager.HasEnoughQuanta(element, cost);
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
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, Owner, false));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, Owner, true));
            return;
        }

        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, Owner, false));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, Owner, true));
    }

    public void StartTurn()
    {
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner));
        EventBus<OnTurnStartEvent>.Raise(new OnTurnStartEvent(Owner));
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
        EventBus<UpdatePlayerCountersVisualEvent>.Raise(new UpdatePlayerCountersVisualEvent(playerID, playerCounters));
    }

    public int GetLightEmittingCreatures()
    {
        var allIds = playerCreatureField.GetAllValidCardIds();
        var count = 0;
        foreach (var id in allIds)
        {
            if (id.Item2.passiveSkills.Light)
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
    internal void DisplayHand()
    {
        playerHand.ShowCardsForPrecog();
    }

    private void DealPoisonDamage()
    {
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(playerCounters.poison, true, false, Owner));
    }

    public bool IsCardPlayable(Card cardToCheck)
    {
        var canAfford = PlayerQuantaManager.HasEnoughQuanta(cardToCheck.costElement, cardToCheck.cost);

        var hasSpace = cardToCheck.cardType switch
        {
            CardType.Pillar => playerPermanentManager.GetAllValidCards().Count < 14,
            CardType.Creature => playerCreatureField.GetAllValidCardIds().Count < 23,
            CardType.Artifact => playerPermanentManager.GetAllValidCards().Count < 14,
            _ => true
        };

        return canAfford && hasSpace;
    }

    public bool IsAbilityUsable(Card card)
    {
        return card.IsAbilityUsable(HasSufficientQuanta, playerHand.GetHandCount());
    }

    //Command Methods
    public void FillHandWith(Card newCard)
    {
        var amountToAdd = 8 - playerHand.GetHandCount();
        for (var i = 0; i < amountToAdd; i++)
        {
            DeckManager.AddCardToTop(CardDatabase.Instance.GetCardFromId(newCard.iD));
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner));
        }
    }

    public List<ID> cloakIndex = new();
    public int sacrificeCount;

    //Play Card From Hand Logic and Visual Command Pair

    private void PlayCardFromHand(PlayCardFromHandEvent playCardFromHandEvent)
    {
        if (!playCardFromHandEvent.Id.IsOwnedBy(Owner)) return;

        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("CardPlay"));
        if (playerCounters.neurotoxin > 0)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Neurotoxin, Owner, 1));
        }

        if (!playCardFromHandEvent.CardToPlay.cardType.Equals(CardType.Spell))
        {
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(playCardFromHandEvent.CardToPlay, playCardFromHandEvent.Id.IsOwnedBy(OwnerEnum.Player)));


            switch (playCardFromHandEvent.CardToPlay.cardType)
            {
                case CardType.Artifact:
                case CardType.Pillar:
                    EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(Owner, playCardFromHandEvent.CardToPlay));
                    break;
                case CardType.Creature:
                    EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(Owner, playCardFromHandEvent.CardToPlay));
                    break;
                case CardType.Weapon:
                case CardType.Shield:
                case CardType.Mark:
                    EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(Owner, playCardFromHandEvent.CardToPlay));
                    break;
            }
        }

        //Spend Quanta
        if (playCardFromHandEvent.CardToPlay.cost > 0)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(playCardFromHandEvent.CardToPlay.cost, playCardFromHandEvent.CardToPlay.costElement, Owner, false));
        }
        //Remove Card From Hand
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(playCardFromHandEvent.Id));
        DisplayPlayableGlow();
    }

    private void DisplayPlayableGlow()
    {
        if (Owner.Equals(OwnerEnum.Opponent)) { return; }
        EventBus<HideUsableDisplayEvent>.Raise(new HideUsableDisplayEvent());
        EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(HasSufficientQuanta, Owner, playerHand.GetHandCount()));
    }

    public void ActivateCloakEffect(Transform objectTransform)
    {
        if (Owner.Equals(OwnerEnum.Player)) { return; }
        cloakVisual.SetActive(true);
        objectTransform.parent.transform.parent = cloakVisual.transform;

    }
    public void DeactivateCloakEffect()
    {
        if (Owner.Equals(OwnerEnum.Player)) { return; }
        cloakVisual.SetActive(false);
    }

    public void ResetCloakPermParent((ID, Card) cardPair)
    {
        if (Owner.Equals(OwnerEnum.Player)) { return; }
        // cardPair.transform.parent.transform.parent = permParent.transform;
        // cardPair.transform.SetSiblingIndex(cardPair.id.index);
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
            if (!creature.Item2.costElement.Equals(Element.Darkness) &&
                !creature.Item2.costElement.Equals(Element.Death)) continue;
            creature.Item2.DefModify += defMod;
            creature.Item2.AtkModify += atkMod;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(creature.Item1, creature.Item2, true));
        }
    }

    public void DiscardCard(ID idToDiscard, Card cardToDiscard)
    {
        if (cardToDiscard.innateSkills.Obsession)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(cardToDiscard.iD.IsUpgraded() ? 13 : 10, true, false, Owner));
        }
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(idToDiscard));
        BattleVars.Shared.HasToDiscard = false;
    }

    public IEnumerator EndTurnRoutine()
    {
        EventBus<OnPermanentTurnEndEvent>.Raise(new OnPermanentTurnEndEvent(Owner, CardType.Pillar));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPassiveTurnEndEvent>.Raise(new OnPassiveTurnEndEvent(Owner, CardType.Mark));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPermanentTurnEndEvent>.Raise(new OnPermanentTurnEndEvent(Owner, CardType.Artifact));
        yield return new WaitForSeconds(0.25f);

        DuelManager.Instance.GetNotIDOwner(playerID).DealPoisonDamage();

        EventBus<OnCreatureTurnEndEvent>.Raise(new OnCreatureTurnEndEvent(Owner));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPassiveTurnEndEvent>.Raise(new OnPassiveTurnEndEvent(Owner, CardType.Weapon));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPassiveTurnEndEvent>.Raise(new OnPassiveTurnEndEvent(Owner, CardType.Shield));
        yield return new WaitForSeconds(0.25f);

        if (Owner.Equals(OwnerEnum.Player))
        {
            BattleVars.Shared.ChangePlayerTurn();
        }
    }

    private IEnumerator SetupPassiveDisplayers()
    {
        playerPassiveManager.SetOwner(Owner);
        playerCreatureField.SetOwner(Owner);
        playerPermanentManager.SetOwner(Owner);
        var markElement = Owner.Equals(OwnerEnum.Player) ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;

        var mark = CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.MarkIds[(int)markElement]);
        EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(Owner, mark));
        EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(Owner, CardDatabase.Instance.GetPlaceholderCard(2)));
        EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(Owner, CardDatabase.Instance.GetPlaceholderCard(1)));
        yield return null;
    }


    private IEnumerator SetupOtherDisplayers()
    {
        PlayerQuantaManager = new QuantaManager(Owner);
        playerHand.SetOwner(Owner);
        playerCounters = new Counters();
        playerID = new(Owner, FieldEnum.Player, 0);

        List<Card> deck = new(Owner.Equals(OwnerEnum.Player) ?
                    PlayerData.Shared.currentDeck.DeserializeCard()
                    : new List<string>(BattleVars.Shared.EnemyAiData.deck.Split(" ")).DeserializeCard());

        if (BattleVars.Shared.EnemyAiData.maxHp == 200 && Owner.Equals(OwnerEnum.Opponent))
        {
            deck.AddRange(deck);

        }
        deck.Shuffle();
        DeckManager = new DeckManager(deck, Owner);

        HealthManager = new HealthManager(Owner.Equals(OwnerEnum.Player) ? 100 : BattleVars.Shared.EnemyAiData.maxHp, Owner);
        healthDisplayer.SetHpStart(HealthManager.GetCurrentHealth());

        for (var i = 0; i < 7; i++)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(Owner));
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
        playerDisplayer.playerID = playerID;
        if (!Owner.Equals(OwnerEnum.Player))
        {
            DuelManager.Instance.allPlayersSetup = true;
        }
        yield return null;
    }

    public IEnumerator SetupPlayerManager(OwnerEnum owner)
    {
        Owner = owner;
        yield return StartCoroutine(SetupPassiveDisplayers());
        yield return StartCoroutine(SetupOtherDisplayers());
    }

    public void QuickPlay(ID targetId, Card targetCard)
    {
        if (targetId.IsFromHand())
        {
            if (!IsCardPlayable(targetCard))
            {
                EventBus<SetupCardDisplayEvent>.Raise(new SetupCardDisplayEvent(targetId, targetCard, IsCardPlayable(targetCard)));
                return;
            }

            if (!targetCard.cardType.Equals(CardType.Spell))
            {
                EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(targetCard, targetId));
            }
            else
            {
                ProcessSkillCard(targetId, targetCard);
            }
        }
        else
        {
            if (!IsAbilityUsable(targetCard))
            {
                EventBus<SetupCardDisplayEvent>.Raise(new SetupCardDisplayEvent(targetId, targetCard, IsCardPlayable(targetCard)));
                return;
            }

            ProcessSkillCard(targetId, targetCard);
        }

        DisplayPlayableGlow();
    }

    private void ProcessSkillCard(ID id, Card card)
    {
        if (!SkillManager.Instance.ShouldAskForTarget(card))
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(card.skillCost, card.skillElement, Owner, false));
            SkillManager.Instance.SkillRoutineNoTarget(this, id, card);
            if (id.IsFromHand())
            {
                EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(card, id));
            }
            else if (card.skill != "photosynthesis")
            {
                card.AbilityUsed = true;
            }
        }
        else
        {
            BattleVars.Shared.IsSelectingTarget = true;
            EventBus<SetupAbilityTargetsEvent>.Raise(new SetupAbilityTargetsEvent(this, card));
        }
    }

    private void ActivateAbility(ActivateSpellOrAbilityEvent activateSpellOrAbilityEvent)
    {
        var abilityCard = BattleVars.Shared.AbilityCardOrigin;
        if (abilityCard is null) return;
        if (!BattleVars.Shared.AbilityIDOrigin.owner.Equals(Owner)) return;
        var ability = abilityCard.skill.GetSkillScript<ActivatedAbility>();

        if (abilityCard.cardType.Equals(CardType.Spell))
        {
            if (SkillManager.Instance.ShouldAskForTarget(abilityCard))
            {
                if (abilityCard.cardType.Equals(CardType.Spell))
                {
                    EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(Owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
                }
                else
                {
                    EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(Owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
                }
                EventBus<ActivateAbilityEffectEvent>.Raise(new ActivateAbilityEffectEvent(ability.Activate, activateSpellOrAbilityEvent.TargetId));
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.Shared.AbilityIDOrigin, abilityCard);
            }
            EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(abilityCard, BattleVars.Shared.AbilityIDOrigin));
        }
        else
        {
            if (BattleVars.Shared.AbilityCardOrigin.skill != "photosynthesis")
            {
                BattleVars.Shared.AbilityCardOrigin.AbilityUsed = true;
            }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(BattleVars.Shared.AbilityCardOrigin.skillCost, BattleVars.Shared.AbilityCardOrigin.skillElement, Owner, false));

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.Shared.AbilityCardOrigin))
            {
                if (abilityCard.cardType.Equals(CardType.Spell))
                {
                    EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(Owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
                }
                else
                {
                    EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(Owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
                }
                EventBus<ActivateAbilityEffectEvent>.Raise(new ActivateAbilityEffectEvent(ability.Activate, activateSpellOrAbilityEvent.TargetId));
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.Shared.AbilityIDOrigin, BattleVars.Shared.AbilityCardOrigin);
            }

        }
        DuelManager.Instance.ResetTargeting();
        DisplayPlayableGlow();
    }

    public bool HasGravityCreatures()
    {
        return !playerCreatureField.GetCreatureWithGravity().Equals(default);
    }
}
