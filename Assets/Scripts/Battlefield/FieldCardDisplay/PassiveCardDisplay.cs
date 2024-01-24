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
        if (!setBoneCountEvent.Owner.Equals(Id.owner) || !Card.cardType.Equals(CardType.Shield)) return;
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
        cardImage.sprite = ImageHelper.GetCardImage(updateCardDisplayEvent.Card.imageID);

        activeAHolder.SetActive(false);
        if (updateCardDisplayEvent.Card.skill != "")
        {
            activeAHolder.SetActive(true);
            activeAName.text = updateCardDisplayEvent.Card.skill;
            if (updateCardDisplayEvent.Card.skillCost > 0)
            {
                activeACost.text = updateCardDisplayEvent.Card.skillCost.ToString();
                activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                activeAElement.sprite =
                    ImageHelper.GetElementImage(updateCardDisplayEvent.Card.skillElement.FastElementString());
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

        switch (Card.iD)
        {
            case "7n8":
            case "5oo":
                Card.TurnsInPlay = 5;
                turnsInPlay.text = "5";
                break;
            case "61t":
            case "80d":
                Card.TurnsInPlay = 3;
                turnsInPlay.text = "3";
                break;
        }

        if (!Card.innateSkills.Bones) return;
        EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, Id.owner, 7));
    }

    private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
    {
        if (!clearCardDisplayEvent.Id.Equals(Id)) return;
        DisplayCard(new UpdatePassiveDisplayEvent(Id, CardDatabase.Instance.GetPlaceholderCard(Id.index), false));

        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "CardDeath", Element.Other));
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("RemoveCardFromField"));

        if (Card.innateSkills.Bones)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, Id.owner,
                -9999));
        }

        DisplayCard(new UpdatePassiveDisplayEvent(Id, CardDatabase.Instance.GetPlaceholderCard(Id.index), false));
    }

    private void OnTurnStart(OnTurnStartEvent onTurnStartEvent)
    {
        if (!onTurnStartEvent.Owner.Equals(Id.owner)) return;
        switch (Card.iD)
        {
            case "7n8":
            case "5oo":
                Card.TurnsInPlay--;
                turnsInPlay.text = Card.TurnsInPlay.ToString();
                break;
            case "61t":
            case "80d":
                Card.TurnsInPlay--;
                turnsInPlay.text = Card.TurnsInPlay.ToString();
                break;
        }
    }

    private void DeathTrigger(OnDeathTriggerEvent onDeathTriggerEvent)
    {
        if (Card.innateSkills.Bones)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, Id.owner, 2));
        }
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
        if (!Card.cardType.Equals(CardType.Mark)) return;
        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "QuantaGenerate", Card.costElement));
        if (BattleVars.Shared.EnemyAiData.maxHp >= 150 && Id.owner == OwnerEnum.Opponent)
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(3, Card.costElement, Id.owner, true));
        }
        else
        {
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Card.costElement, Id.owner, true));
        }
    }

    private void WeaponTurnEnd()
    {
        if (!Card.cardType.Equals(CardType.Weapon)) return;
        var owner = DuelManager.Instance.GetIDOwner(Id);
        var enemy = DuelManager.Instance.GetNotIDOwner(Id);
        var atkNow = WeaponInnateSkills(Card.AtkNow, owner);

        
        if (!Card.passiveSkills.Momentum)
        {
            atkNow = enemy.ManageShield(atkNow, (Id, Card));
        }

        //Send Damage
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, true, false, enemy.Owner));

        if (atkNow > 0)
        {
            if (Card.passiveSkills.Vampire)
            {
                EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, false, false, owner.Owner));
            }

            if (Card.passiveSkills.Venom)
            {
                EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison,
                    enemy.Owner, 1));
            }

            if (Card.innateSkills.Scramble)
            {
                enemy.ScrambleQuanta();
            }
        }

        Card.AbilityUsed = false;
    }

    private int WeaponInnateSkills(int atkNow, PlayerManager owner)
    {
        if (Card.innateSkills.Regenerate)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(5, false, false, owner.Owner));
        }

        if (Card.innateSkills.Fiery)
        {
            // ReSharper disable once PossibleLossOfFraction
            atkNow += Mathf.FloorToInt(owner.GetAllQuantaOfElement(Element.Fire) / 5);
        }

        if (Card.innateSkills.Hammer)
        {
            if (owner.playerPassiveManager.GetMark().Item2.costElement == Element.Earth ||
                owner.playerPassiveManager.GetMark().Item2.costElement == Element.Gravity)
            {
                atkNow++;
            }
        }

        if (Card.innateSkills.Dagger)
        {
            if (owner.playerPassiveManager.GetMark().Item2.costElement == Element.Death ||
                owner.playerPassiveManager.GetMark().Item2.costElement == Element.Darkness)
            {
                atkNow++;
            }
        }

        if (Card.innateSkills.Bow)
        {
            if (owner.playerPassiveManager.GetMark().Item2.costElement == Element.Air)
            {
                atkNow++;
            }
        }

        if (Card.Freeze > 0)
        {
            atkNow = 0;
            Card.Freeze--;
        }

        if (Card.innateSkills.Delay <= 0) return atkNow;
        Card.innateSkills.Delay--;

        return 0;
    }
}