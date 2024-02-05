using System.Collections.Generic;
using Battlefield.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermanentCardDisplay : CardFieldDisplay
{
    [SerializeField] private Image cardImage, activeAElement;
    [SerializeField] private TextMeshProUGUI stackCount, activeAName, activeACost;
    [SerializeField] private GameObject activeAHolder, immaterialIndicator;
    [SerializeField] private TMP_FontAsset underlayBlack, underlayWhite;

    private EventBinding<ClearCardDisplayEvent> _clearCardDisplayBinding;
    private EventBinding<UpdatePermanentCardEvent> _updateCardDisplayBinding;
    
    private EventBinding<OnPermanentTurnEndEvent> _onTurnEndEventBinding;
    private EventBinding<OnDeathTriggerEvent> _onDeathTriggerEventBinding;
    private EventBinding<OnTurnStartEvent> _onTurnStartEventBinding;
    
    private List<string> _permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
    public int StackCountValue { get; private set; }

    private void OnDisable()
    {
        EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
        EventBus<UpdatePermanentCardEvent>.Unregister(_updateCardDisplayBinding);
        
        EventBus<OnPermanentTurnEndEvent>.Unregister(_onTurnEndEventBinding);
        EventBus<OnDeathTriggerEvent>.Unregister(_onDeathTriggerEventBinding);
        EventBus<OnTurnStartEvent>.Unregister(_onTurnStartEventBinding);
    }

    private void OnEnable()
    {
        _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
        EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
        _updateCardDisplayBinding = new EventBinding<UpdatePermanentCardEvent>(DisplayCard);
        EventBus<UpdatePermanentCardEvent>.Register(_updateCardDisplayBinding);
        
        _onTurnEndEventBinding = new EventBinding<OnPermanentTurnEndEvent>(TurnEnd);
        EventBus<OnPermanentTurnEndEvent>.Register(_onTurnEndEventBinding);
        _onDeathTriggerEventBinding = new EventBinding<OnDeathTriggerEvent>(DeathTrigger);
        EventBus<OnDeathTriggerEvent>.Register(_onDeathTriggerEventBinding);
        _onTurnStartEventBinding = new EventBinding<OnTurnStartEvent>(OnTurnStart);
        EventBus<OnTurnStartEvent>.Register(_onTurnStartEventBinding);
    }
    
    private void DisplayCard(UpdatePermanentCardEvent updatePermanentCardEvent)
    {
        if (!updatePermanentCardEvent.Id.Equals(Id)) return;
        StackCountValue++;
        SetCard(updatePermanentCardEvent.Card);

        immaterialIndicator.SetActive(updatePermanentCardEvent.Card.innateSkills.Immaterial);
        var isPlayer = Id.owner.Equals(OwnerEnum.Player);
        if (updatePermanentCardEvent.Card.cardName.Contains("Pendulum"))
        {
            var pendulumElement = updatePermanentCardEvent.Card.costElement;
            var markElement = isPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
            cardImage.sprite = updatePermanentCardEvent.Card.costElement == updatePermanentCardEvent.Card.skillElement
                ? ImageHelper.GetPendulumImage(pendulumElement.FastElementString(), markElement.FastElementString())
                : ImageHelper.GetPendulumImage(markElement.FastElementString(), pendulumElement.FastElementString());
        }
        else
        {
            cardImage.sprite = ImageHelper.GetCardImage(updatePermanentCardEvent.Card.imageID);
        }

        SetupActiveAbility();
        CheckOnPlayEffects();
        
        List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
        if (permanentsWithCountdown.Contains(updatePermanentCardEvent.Card.iD))
        {
            stackCount.text = $"{updatePermanentCardEvent.Card.TurnsInPlay}";
        }
        else
        {
            stackCount.text = StackCountValue > 1 ? $"{StackCountValue}X" : "";
        }
    }

    private void SetupActiveAbility()
    {
        activeAHolder.SetActive(false);
        if (Card.skill == "") return;
        
        activeAHolder.SetActive(true);
        activeAName.text = Card.skill;
        var hasCost = Card.skillCost > 0;
        activeACost.text = hasCost ? Card.skillCost.ToString() : "";
        activeAElement.color = hasCost ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);
        
        activeAElement.sprite = hasCost ?
            ImageHelper.GetElementImage(Card.skillElement.FastElementString()) : null;
    }

    private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
    {
        if (!clearCardDisplayEvent.Id.Equals(Id)) return;

        StackCountValue--;
        CheckOnRemoveEffects();
        if (StackCountValue == 0)
        {
            Destroy(gameObject);
            EventBus<RemoveCardFromManagerEvent>.Raise(new RemoveCardFromManagerEvent(Id));
            return;
        }

        stackCount.text = clearCardDisplayEvent.Stack > 1 ? $"{clearCardDisplayEvent.Stack}X" : "";
        
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "CardDeath", Element.Air));
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("RemoveCardFromField"));
    }
    
    private void CheckOnPlayEffects()
    {
        switch (Card.iD)
        {
            case "7q9":
            case "5rp":
                Card.TurnsInPlay = 2;
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Delay, Id.owner.Not(), 1));
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Delay, Id.owner, 1));
                break;
            case "5v2":
            case "7ti":
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Invisibility, Id.owner, 3));
                Card.TurnsInPlay = 3;
                DuelManager.Instance.GetIDOwner(Id).ActivateCloakEffect(transform);
                break;
            case "5j2":
            case "7hi":
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Patience, Id.owner, 1));
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(true, Card.iD);
                break;
            case "5pa":
            case "7nq":
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Freedom, Id.owner, 1));
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.AddFloodCount(1);
                break;
        }
        
        if (Card.innateSkills.Sanctuary)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Sanctuary, Id.owner, 1));
        }
    }

    private void CheckOnRemoveEffects()
    {
        var owner = DuelManager.Instance.GetIDOwner(Id);
        switch (Card.iD)
        {
            case "5v2":
            case "7ti":
                owner.ResetCloakPermParent((Id, Card));
                if (owner.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.Item2.iD is "5v2" or "7ti").Count == 1)
                {
                    owner.DeactivateCloakEffect();
                    EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Invisibility, Id.owner, -3));
                }
                break;
            case "5j2":
            case "7hi":
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Patience, Id.owner, -1));
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(false, Card.iD);
                break;
            case "5pa":
            case "7nq":
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Freedom, Id.owner, -1));
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.AddFloodCount(-1);
                break;
        }
        
        if (Card.innateSkills.Sanctuary)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Sanctuary, Id.owner, -1));
        }
    }

    private void TurnEnd(OnPermanentTurnEndEvent onTurnEndEvent)
    {
        if (!onTurnEndEvent.Owner.Equals(Id.owner)) return;
        if (Card.cardType.Equals(CardType.Artifact) && onTurnEndEvent.CardType.Equals(CardType.Artifact))
        {
            ArtifactEndTurnAction();
        }
            
        if (Card.cardType.Equals(CardType.Pillar) && onTurnEndEvent.CardType.Equals(CardType.Pillar))
        {
            PillarEndTurnAction();
        }
    }
    
    private void OnTurnStart(OnTurnStartEvent onTurnStartEvent)
    {
        if (!onTurnStartEvent.Owner.Equals(Id.owner)) return;
        Card.AbilityUsed = false;
        if (!_permanentsWithCountdown.Contains(Card.iD)) return;
        Card.TurnsInPlay--;
    
        if (Card.TurnsInPlay <= 0)
        {
            HideCard(new ClearCardDisplayEvent(Id));
            return;
        }
        stackCount.text = $"{Card.TurnsInPlay}";
    }

    private void DeathTrigger(OnDeathTriggerEvent onDeathTriggerEvent)
    {
        if (Card.innateSkills.SoulCatch)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(Card.iD.IsUpgraded() ? 3 : 2, Element.Death, Id.owner, true));
        }

        if (!Card.innateSkills.Boneyard) return;
        var card = CardDatabase.Instance.GetCardFromId(Card.iD.IsUpgraded() ? "716" : "52m");
        EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(card, Id.owner.Equals(OwnerEnum.Player)));
        EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(Id.owner, card));
    }
    
    private void PillarEndTurnAction()
    {
        if (Card.cardName.Contains("Pendulum"))
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(StackCountValue, Card.skillElement, Id.owner, true));
            if (Id.owner.Equals(OwnerEnum.Player) || DuelManager.Instance.GetIDOwner(Id).playerCounters.invisibility <= 0)
            {
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "QuantaGenerate", Card.costElement));
            }
    
            Card.skillElement = Card.skillElement == Card.costElement ? DuelManager.Instance.GetIDOwner(Id).playerPassiveManager.GetMark().Item2.costElement : Card.costElement;
        }
        else
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(Card.costElement == Element.Other ? 3 * StackCountValue : StackCountValue, Card.costElement, Id.owner, true));
            
            if (Id.owner.Equals(OwnerEnum.Player) || DuelManager.Instance.GetIDOwner(Id).playerCounters.invisibility <= 0)
            {
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "QuantaGenerate", Card.costElement));
            }
        }
    }
    
    private void ArtifactEndTurnAction()
    {
        var owner = DuelManager.Instance.GetIDOwner(Id);
        if (Card.innateSkills.Empathy)
        {
            var creatureCount = owner.playerCreatureField.GetAllValidCardIds().Count;
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(creatureCount, false, false, owner.Owner));
        }
        if (Card.innateSkills.Gratitude)
        {
            var healthAmount = owner.playerPassiveManager.GetMark().Item2.costElement == Element.Life ? 5 : 3;
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(healthAmount, false, false, owner.Owner));
        }
    
        List<int> floodList = new() { 11, 13, 9, 10, 12 };
        switch (Card.iD)
        {
            case "5j2":
            case "7hi":
                var creatureList = owner.playerCreatureField.GetAllValidCardIds();
                foreach (var creature in creatureList)
                {
                    var statModifier = DuelManager.Instance.GetCardCount(new() { "5ih", "7h1" }) > 0 && floodList.Contains(Id.index) ? 5 : 2;
                    creature.Item2.AtkModify += statModifier;
                    creature.Item2.AtkModify += statModifier;
                    EventBus<UpdateCardDisplayEvent>.Raise(new UpdateCardDisplayEvent(creature.Item1, creature.Item2));
                }
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.enemy.ClearFloodedArea(floodList);
                DuelManager.Instance.player.ClearFloodedArea(floodList);
                break;
        }
    
        if (Card.innateSkills.Sanctuary)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(4, false, false, owner.Owner));
        }
    
        if (!Card.innateSkills.Void) return;
        var healthChange = owner.playerPassiveManager.GetMark().Item2.costElement == Element.Darkness ? 3 : 2;
        EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(-healthChange, owner.Owner.Not(), true));
    }
}
