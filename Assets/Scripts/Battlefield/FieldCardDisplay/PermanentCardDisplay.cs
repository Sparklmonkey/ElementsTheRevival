using System.Collections.Generic;
using Battlefield.Abilities;
using Battlefield.Abstract;
using Core.Helpers;
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
        Card.AbilityUsed = true;

        immaterialIndicator.SetActive(updatePermanentCardEvent.Card.innateSkills.Immaterial);
        var isPlayer = Id.IsOwnedBy(OwnerEnum.Player);
        if (updatePermanentCardEvent.Card.CardName.Contains("Pendulum"))
        {
            var pendulumElement = updatePermanentCardEvent.Card.CostElement;
            var markElement = isPlayer ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
            cardImage.sprite = updatePermanentCardEvent.Card.IsPendulumTurn
                ? ImageHelper.GetPendulumImage(pendulumElement.FastElementString(), markElement.FastElementString())
                : ImageHelper.GetPendulumImage(markElement.FastElementString(), pendulumElement.FastElementString());
        }
        else
        {
            cardImage.sprite = updatePermanentCardEvent.Card.cardImage;
        }

        SetupActiveAbility();
        CheckOnPlayEffects();
        
        
        if (_permanentsWithCountdown.Contains(updatePermanentCardEvent.Card.Id))
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
        if (Card.Skill is null) return;
        
        activeAHolder.SetActive(true);
        activeAName.text = Card.Skill.GetType().Name;
        var hasCost = Card.SkillCost > 0;
        activeACost.text = hasCost ? Card.SkillCost.ToString() : "";
        activeAElement.color = hasCost ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);
        
        activeAElement.sprite = hasCost ?
            ImageHelper.GetElementImage(Card.SkillElement.FastElementString()) : null;
    }

    private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
    {
        if (!clearCardDisplayEvent.Id.Equals(Id)) return;

        StackCountValue--;
        CheckOnRemoveEffects();
        if (StackCountValue <= 0)
        {
            EventBus<RemoveCardFromManagerEvent>.Raise(new RemoveCardFromManagerEvent(Id));
            Destroy(gameObject);
            return;
        }

        stackCount.text = clearCardDisplayEvent.Stack > 1 ? $"{clearCardDisplayEvent.Stack}X" : "";
        
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "CardDeath", Element.Air));
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("RemoveCardFromField"));
    }
    
    private void CheckOnPlayEffects()
    {
        Card.PlayRemoveAbility?.OnPlayActivate(Id, Card);

        if (Card.PlayRemoveAbility is CloakPlayRemoveAbility)
        {
            EventBus<UpdateCloakParentEvent>.Raise(new UpdateCloakParentEvent(transform, Id, true));
        }
    }

    private void CheckOnRemoveEffects()
    {
        Card.PlayRemoveAbility?.OnRemoveActivate(Id, Card);
        if (Card.PlayRemoveAbility is CloakPlayRemoveAbility)
        {
            EventBus<UpdateCloakParentEvent>.Raise(new UpdateCloakParentEvent(transform, Id, false));
        }
    }

    private void TurnEnd(OnPermanentTurnEndEvent onTurnEndEvent)
    {
        if (!onTurnEndEvent.Owner.Equals(Id.owner)) return;
        if (Card.Type.Equals(CardType.Artifact) && onTurnEndEvent.CardType.Equals(CardType.Artifact))
        {
            Card.TurnEndAbility?.Activate(Id, Card);
        }
            
        if (Card.Type.Equals(CardType.Pillar) && onTurnEndEvent.CardType.Equals(CardType.Pillar))
        {
            PillarEndTurnAction();
        }
    }
    
    private void OnTurnStart(OnTurnStartEvent onTurnStartEvent)
    {
        if (!onTurnStartEvent.Owner.Equals(Id.owner)) return;
        Card.AbilityUsed = false;
        if (!Card.HasTurnLimit) return;
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
        Card.DeathTriggerAbility?.Activate(Id, Card);
    }
    
    private void PillarEndTurnAction()
    {
        if (Card.CardName.Contains("Pendulum"))
        {
            var markElement = Id.IsOwnedBy(OwnerEnum.Player) ? PlayerData.Shared.markElement : BattleVars.Shared.EnemyAiData.mark;
            var pendulumElement = Card.IsPendulumTurn ? markElement : Card.CostElement;
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(StackCountValue, pendulumElement, Id.owner, true));
            if (Id.IsOwnedBy(OwnerEnum.Player) || DuelManager.Instance.GetIDOwner(Id).playerCounters.invisibility <= 0)
            {
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "QuantaGenerate", pendulumElement));
            }

            Card.IsPendulumTurn = !Card.IsPendulumTurn;
            
            cardImage.sprite = Card.IsPendulumTurn
                ? ImageHelper.GetPendulumImage(Card.CostElement.FastElementString(), markElement.FastElementString())
                : ImageHelper.GetPendulumImage(markElement.FastElementString(), Card.CostElement.FastElementString());
        }
        else
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(Card.CostElement == Element.Other ? 3 * StackCountValue : StackCountValue, Card.CostElement, Id.owner, true));
            
            if (Id.IsOwnedBy(OwnerEnum.Player) || DuelManager.Instance.GetIDOwner(Id).playerCounters.invisibility <= 0)
            {
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "QuantaGenerate", Card.CostElement));
            }
        }
    }
}
