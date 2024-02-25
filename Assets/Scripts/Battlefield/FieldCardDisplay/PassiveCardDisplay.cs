using System.Collections;
using System.Collections.Generic;
using Battlefield.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveCardDisplay : CardFieldDisplay
{
    [SerializeField] private Image cardImage, activeAElement;
    [SerializeField] private TextMeshProUGUI activeAName, activeACost, turnsInPlay;
    [SerializeField] private GameObject activeAHolder, immaterialIndicator;

    private EventBinding<ClearCardDisplayEvent> _clearCardDisplayBinding;
    private EventBinding<UpdatePassiveDisplayEvent> _updateCardDisplayBinding;

    private EventBinding<OnPassiveTurnEndEvent> _onTurnEndEventBinding;
    private EventBinding<OnDeathTriggerEvent> _onDeathTriggerEventBinding;
    private EventBinding<OnTurnStartEvent> _onTurnStartEventBinding;

    private EventBinding<SetBoneCountEvent> _setBoneCountBinding;

    private void OnDisable()
    {
        EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
        EventBus<UpdatePassiveDisplayEvent>.Unregister(_updateCardDisplayBinding);

        EventBus<OnPassiveTurnEndEvent>.Unregister(_onTurnEndEventBinding);
        EventBus<OnDeathTriggerEvent>.Unregister(_onDeathTriggerEventBinding);
        EventBus<OnTurnStartEvent>.Unregister(_onTurnStartEventBinding);
        EventBus<SetBoneCountEvent>.Unregister(_setBoneCountBinding);
    }

    private void OnEnable()
    {
        _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
        EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
        _updateCardDisplayBinding = new EventBinding<UpdatePassiveDisplayEvent>(DisplayCard);
        EventBus<UpdatePassiveDisplayEvent>.Register(_updateCardDisplayBinding);

        _onTurnEndEventBinding = new EventBinding<OnPassiveTurnEndEvent>(TurnEnd);
        EventBus<OnPassiveTurnEndEvent>.Register(_onTurnEndEventBinding);
        _onDeathTriggerEventBinding = new EventBinding<OnDeathTriggerEvent>(DeathTrigger);
        EventBus<OnDeathTriggerEvent>.Register(_onDeathTriggerEventBinding);
        _onTurnStartEventBinding = new EventBinding<OnTurnStartEvent>(OnTurnStart);
        EventBus<OnTurnStartEvent>.Register(_onTurnStartEventBinding);
        _setBoneCountBinding = new EventBinding<SetBoneCountEvent>(SetBoneCount);
        EventBus<SetBoneCountEvent>.Register(_setBoneCountBinding);
    }

    private void SetBoneCount(SetBoneCountEvent setBoneCountEvent)
    {
        if (!setBoneCountEvent.Owner.Equals(Id.owner) || !Card.Type.Equals(CardType.Shield)) return;
        Card.TurnsInPlay += setBoneCountEvent.Amount;
        turnsInPlay.text = Card.TurnsInPlay.ToString();
        if (Card.TurnsInPlay <= 0)
        {
            HideCard(new ClearCardDisplayEvent(Id));
        }
    }

    private void DisplayCard(UpdatePassiveDisplayEvent updateCardDisplayEvent)
    {
        if (!updateCardDisplayEvent.Id.Equals(Id)) return;

        turnsInPlay.text = "";
        SetCard(updateCardDisplayEvent.Card);
        cardImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        cardImage.sprite = updateCardDisplayEvent.Card.cardImage;

        activeAHolder.SetActive(false);
        if (updateCardDisplayEvent.Card.Skill is not null)
        {
            activeAHolder.SetActive(true);
            activeAName.text = Card.Skill.GetType().Name;
            if (updateCardDisplayEvent.Card.SkillCost > 0)
            {
                activeACost.text = updateCardDisplayEvent.Card.SkillCost.ToString();
                activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                activeAElement.sprite =
                    ImageHelper.GetElementImage(updateCardDisplayEvent.Card.SkillElement.FastElementString());
            }
            else
            {
                activeACost.text = "";
                activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
            }
        }
        else
        {
            activeAHolder.SetActive(false);
        }

        immaterialIndicator.SetActive(updateCardDisplayEvent.Card.innateSkills.Immaterial);
        Card.PlayRemoveAbility?.OnPlayActivate(Id, Card);
        
        turnsInPlay.text = Card.HasTurnLimit ? Card.TurnsInPlay.ToString() : "";
    }

    private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
    {
        if (!clearCardDisplayEvent.Id.Equals(Id)) return;
        DisplayCard(new UpdatePassiveDisplayEvent(Id, CardDatabase.Instance.GetPlaceholderCard(Id.index), false));

        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "CardDeath", Element.Other));
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("RemoveCardFromField"));

        Card.PlayRemoveAbility?.OnRemoveActivate(Id, Card);

        DisplayCard(new UpdatePassiveDisplayEvent(Id, CardDatabase.Instance.GetPlaceholderCard(Id.index), false));
    }

    private void OnTurnStart(OnTurnStartEvent onTurnStartEvent)
    {
        if (!onTurnStartEvent.Owner.Equals(Id.owner)) return;
        if (!Card.HasTurnLimit) return;
        Card.TurnsInPlay--;
        if (Card.TurnsInPlay <= 0)
        {
            HideCard(new ClearCardDisplayEvent(Id));
        }

        turnsInPlay.text = Card.TurnsInPlay.ToString();
    }

    private void DeathTrigger(OnDeathTriggerEvent onDeathTriggerEvent)
    {
        Card.DeathTriggerAbility?.Activate(Id, Card);
    }

    private void TurnEnd(OnPassiveTurnEndEvent onTurnEndEvent)
    {
        if (!onTurnEndEvent.Owner.Equals(Id.owner)) return;

        switch (onTurnEndEvent.CardType)
        {
            case CardType.Mark:
                MarkTurnEnd();
                break;
            case CardType.Weapon:
                WeaponTurnEnd();
                break;
        }
    }

    private void MarkTurnEnd()
    {
        if (!Card.Type.Equals(CardType.Mark)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "QuantaGenerate", Card.CostElement));
        if (BattleVars.Shared.EnemyAiData.maxHp >= 150 && Id.owner == OwnerEnum.Opponent)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, Card.CostElement, Id.owner, true));
        }
        else
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Card.CostElement, Id.owner, true));
        }
    }

    private void WeaponTurnEnd()
    {
        if (!Card.Type.Equals(CardType.Weapon)) return;
        var owner = DuelManager.Instance.GetIDOwner(Id);
        var enemy = DuelManager.Instance.GetNotIDOwner(Id);
        var atkNow = Card.AtkNow;
        Card.WeaponPassive?.ModifyWeaponAtk(Id, ref atkNow);

        if (!Card.passiveSkills.Momentum)
        {
            atkNow = enemy.ManageShield(atkNow, (Id, Card));
        }

        //Send Damage
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, true, false, enemy.owner));

        if (atkNow > 0)
        {
            if (Card.passiveSkills.Vampire)
            {
                EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, false, false, owner.owner));
            }

            if (Card.passiveSkills.Venom)
            {
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison,
                    enemy.owner, atkNow));
            }

            Card.WeaponPassive?.EndTurnEffect(Id);
        }

        Card.AbilityUsed = false;
    }
}