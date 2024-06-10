using Elements.Duel.Manager;
using Elements.Duel.Visual;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battlefield.Abilities;
using Core.Helpers;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private EventBinding<ModifyPlayerCounterEvent> _modifyPlayerCounterBinding;
    private EventBinding<ModifyPlayerHealthEvent> _modifyPlayerHealthBinding;
    private EventBinding<PlayCardFromHandEvent> _playCardOnFieldBinding;
    private EventBinding<ActivateSpellOrAbilityEvent> _activateSpellOrAbilityBinding;
    private EventBinding<UpdateCloakParentEvent> _updateCloakParentBinding;

    private void OnDisable() {
        EventBus<ModifyPlayerCounterEvent>.Unregister(_modifyPlayerCounterBinding);
        EventBus<ModifyPlayerHealthEvent>.Unregister(_modifyPlayerHealthBinding);
        EventBus<PlayCardFromHandEvent>.Unregister(_playCardOnFieldBinding);
        EventBus<ActivateSpellOrAbilityEvent>.Unregister(_activateSpellOrAbilityBinding);
        EventBus<UpdateCloakParentEvent>.Unregister(_updateCloakParentBinding);
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

        _updateCloakParentBinding = new EventBinding<UpdateCloakParentEvent>(UpdateCloakEffect);
        EventBus<UpdateCloakParentEvent>.Register(_updateCloakParentBinding);
    }

    private void ModifyHealthLogic(ModifyPlayerHealthEvent modifyPlayerHealthEvent)
    {
        if (!modifyPlayerHealthEvent.Target.Equals(owner)) return;

        if (sacrificeCount > 0)
        {
            modifyPlayerHealthEvent.IsDamage = !modifyPlayerHealthEvent.IsDamage; 
        }

        var damage = modifyPlayerHealthEvent.IsDamage
            ? -modifyPlayerHealthEvent.Amount
            : modifyPlayerHealthEvent.Amount;
        if (playerPassiveManager.GetShield().Item2.ShieldPassive is ReflectSkill && modifyPlayerHealthEvent.FromSpell)
        {
            EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(damage, owner.Not(), false));
            return;
        }

        EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(damage, owner, false));
    }

    private void ModifyCounters(ModifyPlayerCounterEvent modifyPlayerCounterEvent)
    {
        if (!modifyPlayerCounterEvent.Owner.Equals(owner)) return;
        var amount = modifyPlayerCounterEvent.Amount;

        switch (modifyPlayerCounterEvent.Counter)
        {
            case PlayerCounters.Bone:
                EventBus<SetBoneCountEvent>.Raise(new SetBoneCountEvent(owner, amount));
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
            case PlayerCounters.Delay:
                playerCounters.delay += amount;
                if (playerCounters.delay < 0)
                {
                    playerCounters.delay = 0;
                }
                break;
        }
        EventBus<UpdatePlayerCountersVisualEvent>.Raise(new UpdatePlayerCountersVisualEvent(playerID, playerCounters));
    }

    public void RemoveAllCloaks()
    {
        foreach (var perm in playerPermanentManager.GetAllValidCardIds().Where(perm => perm.card.Id is "5v2" or "7ti"))
        {
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(perm.Item1));
        }
    }

    public ID playerID;

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
        if (weapon.CardName != "Weapon") { value += weapon.AtkNow; }
        return value;
    }

    public void ManageGravityCreatures(ref int atkNow)
    {
        var gravityCreature = playerCreatureField.GetCreatureWithGravity();
        if (gravityCreature.Equals(default)) return;

        if (gravityCreature.card.DefNow >= atkNow)
        {
            if (gravityCreature.card.innateSkills.Voodoo)
            {
                EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, true, false, gravityCreature.id.owner.Not()));
            }
            gravityCreature.Item2.DefModify -= atkNow;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(gravityCreature.id, gravityCreature.card, true));
            atkNow = 0;
            return;
        }

        atkNow -= gravityCreature.card.DefNow;
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(gravityCreature.id));
    }

    public int ManageShield(int atkNow, (ID id, Card card) card)
    {
        var shield = playerPassiveManager.GetShield();
        atkNow -= shield.card.DefNow;
        card.card.DefModify -= shield.card.AtkNow;
        if (shield.Item2.ShieldPassive is null) { return atkNow; }
        var shieldSkill = shield.Item2.ShieldPassive;

        return shieldSkill.ActivateSkill(atkNow, card);
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

    public OwnerEnum owner;
    public void ClearFloodedArea(List<int> safeZones)
    {
        if (DuelManager.FloodCount <= 0) return;
        var idList = playerCreatureField.GetAllValidCardIds();
        foreach (var idCard in idList)
        {
            if (safeZones.Contains(idCard.Item1.index)) { continue; }
            if (idCard.Item2.CostElement.Equals(Element.Other) || idCard.Item2.CostElement.Equals(Element.Water)) { continue; }
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
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, owner, false));
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(total, Element.Other, owner, true));
            return;
        }

        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, owner, false));
        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(9, Element.Other, owner, true));
    }

    public void StartTurn()
    {
        EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(owner));
        EventBus<OnTurnStartEvent>.Raise(new OnTurnStartEvent(owner));
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
            if (id.Item2.TurnEndAbility is LightEndTurn)
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
        if (owner.Equals(OwnerEnum.Player)) return;
        playerHand.ShowCardsForPrecog();
    }

    private void DealPoisonDamage()
    {
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(playerCounters.poison, true, false, owner));
    }

    public bool IsCardPlayable(Card cardToCheck)
    {
        var canAfford = PlayerQuantaManager.HasEnoughQuanta(cardToCheck.CostElement, cardToCheck.Cost);

        var hasSpace = cardToCheck.Type switch
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
        if (playerCounters.sanctuary > 0)
        {
            switch (BattleVars.Shared.IsPlayerTurn)
            {
                case true when owner.Equals(OwnerEnum.Opponent):
                case false when owner.Equals(OwnerEnum.Player):
                    return;
            }
        }
        var amountToAdd = 8 - playerHand.GetHandCount();
        for (var i = 0; i < amountToAdd; i++)
        {
            DeckManager.AddCardToTop(CardDatabase.Instance.GetCardFromId(newCard.Id));
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(owner));
        }
    }

    public int sacrificeCount;

    //Play Card From Hand Logic and Visual Command Pair

    private void PlayCardFromHand(PlayCardFromHandEvent playCardFromHandEvent)
    {
        if (!playCardFromHandEvent.Id.IsOwnedBy(owner)) return;

        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("CardPlay"));
        if (playerCounters.neurotoxin > 0)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Neurotoxin, owner, 1));
        }

        if (!playCardFromHandEvent.CardToPlay.Type.Equals(CardType.Spell))
        {
            EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(playCardFromHandEvent.CardToPlay, playCardFromHandEvent.Id.IsOwnedBy(OwnerEnum.Player)));


            switch (playCardFromHandEvent.CardToPlay.Type)
            {
                case CardType.Artifact:
                case CardType.Pillar:
                    EventBus<PlayPermanentOnFieldEvent>.Raise(new PlayPermanentOnFieldEvent(owner, playCardFromHandEvent.CardToPlay));
                    break;
                case CardType.Creature:
                    EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(owner, playCardFromHandEvent.CardToPlay));
                    break;
                case CardType.Weapon:
                case CardType.Shield:
                case CardType.Mark:
                    EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(owner, playCardFromHandEvent.CardToPlay));
                    break;
            }
        }

        //Spend Quanta
        if (playCardFromHandEvent.CardToPlay.Cost > 0)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(playCardFromHandEvent.CardToPlay.Cost, playCardFromHandEvent.CardToPlay.CostElement, owner, false));
        }
        //Remove Card From Hand
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(playCardFromHandEvent.Id));
        DisplayPlayableGlow();
    }

    private void DisplayPlayableGlow()
    {
        if (owner.Equals(OwnerEnum.Opponent)) { return; }
        EventBus<HideUsableDisplayEvent>.Raise(new HideUsableDisplayEvent());
        EventBus<ShouldShowUsableEvent>.Raise(new ShouldShowUsableEvent(HasSufficientQuanta, owner, playerHand.GetHandCount()));
    }

    private void UpdateCloakEffect(UpdateCloakParentEvent updateCloakParentEvent)
    {
        if (owner.Equals(OwnerEnum.Player)) { return; }

        if (updateCloakParentEvent.IsAdd)
        {
            cloakVisual.SetActive(true);
            updateCloakParentEvent.Transform.parent.transform.parent = cloakVisual.transform;
            return;
        }
        
        updateCloakParentEvent.Transform.parent.transform.parent = permParent.transform;
        updateCloakParentEvent.Transform.SetSiblingIndex(updateCloakParentEvent.Id.index);
        
        if(cloakVisual.transform.childCount > 2) return;
        cloakVisual.SetActive(false);
    }

    public void CheckEclipseNightfall(bool isAdded, bool isNightFall)
    {
        var creatures = playerCreatureField.GetAllValidCardIds();

        var eclipseCount = DuelManager.Instance.GetCardCount(new() { "7ta" });
        var nightfallCount = DuelManager.Instance.GetCardCount(new() { "5uq" });

        var atkMod = 0;
        var defMod = 0;
        if (isAdded)
        {
            if (isNightFall)
            {
                if (eclipseCount == 0 && nightfallCount == 1)
                {
                    atkMod = 1;
                    defMod = 1;
                }
            }
            else
            {
                switch (eclipseCount)
                {
                    case 1 when nightfallCount > 0:
                        atkMod = 1;
                        defMod = 0;
                        break;
                    case 1 when nightfallCount == 0:
                        atkMod = 2;
                        defMod = 1;
                        break;
                }
            }
        }
        else
        {

            if (isNightFall)
            {
                if (eclipseCount == 0 && nightfallCount == 0)
                {
                    atkMod = -1;
                    defMod = -1;
                }
            }
            else
            {
                switch (eclipseCount)
                {
                    case 0 when nightfallCount > 0:
                        atkMod = -1;
                        defMod = 0;
                        break;
                    case 0 when nightfallCount == 0:
                        atkMod = -2;
                        defMod = -1;
                        break;
                }
            }
        }

        var viableCreatures = creatures.FindAll(pair => pair.card.CostElement is Element.Darkness or Element.Death);
        foreach (var creature in viableCreatures)
        {
            creature.Item2.DefModify += defMod;
            creature.Item2.AtkModify += atkMod;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(creature.Item1, creature.Item2, true));
        }
    }

    public void DiscardCard(ID idToDiscard, Card cardToDiscard)
    {
        if (cardToDiscard.innateSkills.Obsession)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(cardToDiscard.Id.IsUpgraded() ? 13 : 10, true, false, owner));
        }
        EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(idToDiscard));
        BattleVars.Shared.HasToDiscard = false;
    }

    public IEnumerator EndTurnRoutine(IsGameOver gameCheck)
    {
        yield return StartCoroutine(EventBus<OnPermanentTurnEndEvent>.RaiseCoroutine(new OnPermanentTurnEndEvent(owner, CardType.Pillar)));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPassiveTurnEndEvent>.Raise(new OnPassiveTurnEndEvent(owner, CardType.Mark));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPermanentTurnEndEvent>.Raise(new OnPermanentTurnEndEvent(owner, CardType.Artifact));
        yield return new WaitForSeconds(0.25f);

        DuelManager.Instance.GetNotIDOwner(playerID).DealPoisonDamage();

        yield return StartCoroutine(EventBus<OnCreatureTurnEndEvent>.RaiseCoroutine(new OnCreatureTurnEndEvent(owner)));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPassiveTurnEndEvent>.Raise(new OnPassiveTurnEndEvent(owner, CardType.Weapon));
        yield return new WaitForSeconds(0.25f);

        EventBus<OnPassiveTurnEndEvent>.Raise(new OnPassiveTurnEndEvent(owner, CardType.Shield));
        yield return new WaitForSeconds(0.25f);
        if (gameCheck())
        {
            yield break;
        }
        if (owner.Equals(OwnerEnum.Player))
        {
            BattleVars.Shared.ChangePlayerTurn();
        }
    }

    private IEnumerator SetupPassiveDisplayers()
    {
        playerPassiveManager.SetOwner(owner);
        playerCreatureField.SetOwner(owner);
        playerPermanentManager.SetOwner(owner);
        var markElement = owner.Equals(OwnerEnum.Player) ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;

        var mark = CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.MarkIds[(int)markElement]);
        EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(owner, mark));
        EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(owner, CardDatabase.Instance.GetPlaceholderCard(2)));
        EventBus<PlayPassiveOnFieldEvent>.Raise(new PlayPassiveOnFieldEvent(owner, CardDatabase.Instance.GetPlaceholderCard(1)));
        yield return null;
    }


    private IEnumerator SetupOtherDisplayers()
    {
        PlayerQuantaManager = new QuantaManager(owner);
        playerHand.SetOwner(owner);
        playerCounters = new Counters();
        playerID = new(owner, FieldEnum.Player, 0);

        List<Card> deck = new(owner.Equals(OwnerEnum.Player) ?
                    PlayerData.Shared.GetDeck().DeserializeCard()
                    : new List<string>(BattleVars.Shared.EnemyAiData.deck.Split(" ")).DeserializeCard());

        if (BattleVars.Shared.EnemyAiData.maxHp == 200 && owner.Equals(OwnerEnum.Opponent))
        {
            deck.AddRange(deck);
        }
        deck.Shuffle();
        DeckManager = new DeckManager(deck, owner);

        HealthManager = new HealthManager(owner.Equals(OwnerEnum.Player) ? 100 : BattleVars.Shared.EnemyAiData.maxHp, owner);
        healthDisplayer.SetHpStart(HealthManager.GetCurrentHealth());

        for (var i = 0; i < 7; i++)
        {
            EventBus<DrawCardFromDeckEvent>.Raise(new DrawCardFromDeckEvent(owner));
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
        if (!owner.Equals(OwnerEnum.Player))
        {
            DuelManager.Instance.allPlayersSetup = true;
        }
        yield return null;
    }

    public IEnumerator SetupPlayerManager(OwnerEnum owner)
    {
        this.owner = owner;
        yield return StartCoroutine(SetupPassiveDisplayers());
        yield return StartCoroutine(SetupOtherDisplayers());
    }

    private void ActivateAbility(ActivateSpellOrAbilityEvent activateSpellOrAbilityEvent)
    {
        var abilityCard = BattleVars.Shared.AbilityCardOrigin;
        if (abilityCard is null) return;
        if (!BattleVars.Shared.AbilityIDOrigin.owner.Equals(owner)) return;
        var ability = abilityCard.Skill;

        if (abilityCard.Type.Equals(CardType.Spell))
        {
            if (SkillManager.Instance.ShouldAskForTarget(abilityCard))
            {
                if (abilityCard.Type.Equals(CardType.Spell))
                {
                    EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
                }
                else
                {
                    EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
                }
                EventBus<ActivateAbilityEffectEvent>.Raise(new ActivateAbilityEffectEvent(ability.Activate, activateSpellOrAbilityEvent.TargetId));
            }
            else
            {
                SkillManager.Instance.SkillRoutineNoTarget(this, BattleVars.Shared.AbilityIDOrigin, abilityCard);
            }
            EventBus<PlayCardFromHandEvent>.Raise(new PlayCardFromHandEvent(abilityCard, BattleVars.Shared.AbilityIDOrigin));

            if (BattleVars.Shared.AbilityCardOrigin.Skill is Fractal)
            {
                DuelManager.Instance.GetIDOwner(BattleVars.Shared.AbilityIDOrigin).FillHandWith(CardDatabase.Instance.GetCardFromId(activateSpellOrAbilityEvent.TargetCard.Id));
            }
        }
        else
        {
            if (BattleVars.Shared.AbilityCardOrigin.Skill is not Photosynthesis)
            {
                BattleVars.Shared.AbilityCardOrigin.AbilityUsed = true;
            }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(BattleVars.Shared.AbilityCardOrigin.SkillCost, BattleVars.Shared.AbilityCardOrigin.SkillElement, owner, false));

            if (SkillManager.Instance.ShouldAskForTarget(BattleVars.Shared.AbilityCardOrigin))
            {
                if (abilityCard.Type.Equals(CardType.Spell))
                {
                    EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
                }
                else
                {
                    EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(owner.Equals(OwnerEnum.Player), abilityCard, activateSpellOrAbilityEvent.TargetId, activateSpellOrAbilityEvent.TargetCard));
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
