using System.Collections;
using System.Linq;
using Battlefield.Abilities;
using Battlefield.Abstract;
using Elements.Duel.Visual;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreatureCardDisplay : CardFieldDisplay
{
    [SerializeField] private Image cardImage,
        upgradeShine,
        rareIndicator,
        cardHeadBack,
        activeAElement;

    [SerializeField] private EffectDisplayManager effectDisplayManager;
    [SerializeField] private TextMeshProUGUI creatureValue, cardName, activeAName, activeACost;
    [SerializeField] private GameObject upMovingText, activeAHolder;

    public TMP_FontAsset underlayBlack, underlayWhite;

    private EventBinding<ClearCardDisplayEvent> _clearCardDisplayBinding;
    private EventBinding<UpdateCreatureCardEvent> _updateCardDisplayBinding;
    private EventBinding<DisplayCreatureAttackEvent> _onCreatureAttackBinding;

    private EventBinding<OnCreatureTurnEndEvent> _onTurnEndEventBinding;
    private EventBinding<OnDeathTriggerEvent> _onDeathTriggerEventBinding;

    private void OnDisable()
    {
        EventBus<ClearCardDisplayEvent>.Unregister(_clearCardDisplayBinding);
        EventBus<UpdateCreatureCardEvent>.Unregister(_updateCardDisplayBinding);
        EventBus<DisplayCreatureAttackEvent>.Unregister(_onCreatureAttackBinding);

        EventBus<OnCreatureTurnEndEvent>.Unregister(_onTurnEndEventBinding);
        EventBus<OnDeathTriggerEvent>.Unregister(_onDeathTriggerEventBinding);
    }

    private void OnEnable()
    {
        _clearCardDisplayBinding = new EventBinding<ClearCardDisplayEvent>(HideCard);
        EventBus<ClearCardDisplayEvent>.Register(_clearCardDisplayBinding);
        _updateCardDisplayBinding = new EventBinding<UpdateCreatureCardEvent>(DisplayCard);
        EventBus<UpdateCreatureCardEvent>.Register(_updateCardDisplayBinding);

        _onCreatureAttackBinding = new EventBinding<DisplayCreatureAttackEvent>(CreatureAttack);
        EventBus<DisplayCreatureAttackEvent>.Register(_onCreatureAttackBinding);

        _onTurnEndEventBinding = new EventBinding<OnCreatureTurnEndEvent>(OnTurnEnd);
        EventBus<OnCreatureTurnEndEvent>.Register(_onTurnEndEventBinding);
        _onDeathTriggerEventBinding = new EventBinding<OnDeathTriggerEvent>(DeathTrigger);
        EventBus<OnDeathTriggerEvent>.Register(_onDeathTriggerEventBinding);
    }

    private void DisplayCard(UpdateCreatureCardEvent updateCardDisplayEvent)
    {
        if (!updateCardDisplayEvent.Id.Equals(Id)) return;
        SetCard(updateCardDisplayEvent.Card);
        if (updateCardDisplayEvent.IsUpdate)
        {
            if (updateCardDisplayEvent.Card.DefNow < 1)
            {
                HideCard(new ClearCardDisplayEvent(Id));
                return;
            }
        }
        else
        {
            Card.AbilityUsed = true;
        }

        cardImage.sprite = updateCardDisplayEvent.Card.cardImage;
        var creatureAtk = updateCardDisplayEvent.Card.AtkNow;
        creatureValue.text = $"{creatureAtk}|{updateCardDisplayEvent.Card.DefNow}";
        cardName.text = updateCardDisplayEvent.Card.CardName;
        cardHeadBack.sprite =
            ImageHelper.GetCardHeadBackground(updateCardDisplayEvent.Card.CostElement.FastElementString());

        var isRareCard = updateCardDisplayEvent.Card.IsRare();
        upgradeShine.gameObject.SetActive(isRareCard);

        cardName.font = isRareCard ? underlayWhite : underlayBlack;
        cardName.color = isRareCard ? new Color32(0, 0, 0, 255) : new Color32(255, 255, 255, 255);

        rareIndicator.gameObject.SetActive(updateCardDisplayEvent.Card.IsRare());

        SetupActiveAbility();
        effectDisplayManager.UpdateEffectDisplay(Card);
        if (updateCardDisplayEvent.IsUpdate) return;
        CheckOnPlayEffects();
        creatureAtk = updateCardDisplayEvent.Card.AtkNow;
        creatureValue.text = $"{creatureAtk}|{updateCardDisplayEvent.Card.DefNow}";
        EventBus<UpdatePossibleDamageEvent>.Raise(new UpdatePossibleDamageEvent());
    }

    private void SetupActiveAbility()
    {
        activeAHolder.SetActive(false);
        if (Card.passiveSkills.Vampire)
        {
            activeAHolder.SetActive(true);
            activeAName.text = "Vamprire";
            activeAElement.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
            activeACost.text = "";
            return;
        }

        if (Card.Skill is null) return;

        activeAHolder.SetActive(true);
        activeAName.text = Card.Skill.GetType().Name;
        var hasCost = Card.SkillCost > 0;
        activeACost.text = hasCost ? Card.SkillCost.ToString() : "";
        activeAElement.color = hasCost ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);

        activeAElement.sprite = hasCost ? ImageHelper.GetElementImage(Card.SkillElement.FastElementString()) : null;
    }

    private void CreatureAttack(DisplayCreatureAttackEvent onDisplayCreatureAttackEvent)
    {
        if (!onDisplayCreatureAttackEvent.AttackingId.Equals(Id)) return;

        StartCoroutine(ShowDamage(onDisplayCreatureAttackEvent.AttackValue));
    }

    private IEnumerator ShowDamage(int damage)
    {
        if (damage <= 0)
        {
            yield break;
        }

        var sText = Instantiate(upMovingText, transform);
        var destination = sText.GetComponent<MovingText>().SetupObject(damage.ToString(), TextDirection.Up);
        for (var i = 0; i < 15; i++)
        {
            sText.transform.position = Vector3.MoveTowards(sText.transform.position, destination, Time.deltaTime * 50f);
            yield return null;
        }

        Destroy(sText);
    }

    private void HideCard(ClearCardDisplayEvent clearCardDisplayEvent)
    {
        if (!clearCardDisplayEvent.Id.Equals(Id)) return;

        EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(Id, "CardDeath", Element.Other));
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("RemoveCardFromField"));
        var shouldDeath = ShouldActivateDeathTriggers();
        if (shouldDeath)
        {
            EventBus<OnDeathTriggerEvent>.Raise(new OnDeathTriggerEvent());
        }

        if (Card.Counters.Aflatoxin > 0)
        {
            var card = CardDatabase.Instance.GetCardFromId("6ro");
            DisplayCard(new UpdateCreatureCardEvent(Id, card, false));
        }
        else if (Card.PlayRemoveAbility is PhoenixPlayRemoveAbility && shouldDeath)
        {
            Card.PlayRemoveAbility?.OnRemoveActivate(Id, Card);
        }
        else
        {
            Card.PlayRemoveAbility?.OnRemoveActivate(Id, Card);

            EventBus<RemoveCardFromManagerEvent>.Raise(new RemoveCardFromManagerEvent(Id));
            Destroy(gameObject);
        }
    }

    private void CheckOnPlayEffects()
    {
        Card.PlayRemoveAbility?.OnPlayActivate(Id, Card);

        if (!Card.CostElement.Equals(Element.Darkness)
            && !Card.CostElement.Equals(Element.Death)) return;
        if (DuelManager.Instance.GetCardCount(new() { "7ta" }) > 0)
        {
            Card.DefModify += 1;
            Card.AtkModify += 2;
        }
        else if (DuelManager.Instance.GetCardCount(new() { "5uq" }) > 0)
        {
            Card.DefModify += 1;
            Card.AtkModify += 1;
        }

        if (Card.PlayRemoveAbility is SwarmPlayRemoveAbility)
        {
            
            var hp = DuelManager.Instance.GetIDOwner(Id).playerCounters.scarabOld + DuelManager.Instance.GetIDOwner(Id).playerCounters.scarab;

        }
    }

    private bool ShouldActivateDeathTriggers()
    {
        if (BattleVars.Shared.AbilityCardOrigin is not null)
        {
            return BattleVars.Shared.AbilityCardOrigin.Skill is not Reversetime;
        }

        return !Card.CardName.Contains("Skeleton");
    }

    private void DeathTrigger(OnDeathTriggerEvent onDeathTriggerEvent)
    {
        Card.DeathTriggerAbility?.Activate(Id, Card);
    }

    private void OnTurnEnd(OnCreatureTurnEndEvent onTurnEndEvent)
    {
        if (!onTurnEndEvent.Owner.Equals(Id.owner)) return;

        var owner = DuelManager.Instance.GetIDOwner(Id);
        var enemy = DuelManager.Instance.GetNotIDOwner(Id);
        var atkNow = Card.AtkNow;

        var shouldSkip = owner.playerCounters.delay > 0 ||
                         owner.playerCounters.patience > 0 || 
                         Card.Counters.Delay > 0 ||
                         Card.Counters.Freeze > 0;
        Card.AbilityUsed = false;
        if (owner.playerCounters.patience > 0)
        {
            if (Id.index is < 9 or > 13 && DuelManager.FloodCount > 0)
            {
                Card.AtkModify += 5;
                Card.DefModify += 5;
            }
            else
            {
                Card.AtkModify += 2;
                Card.DefModify += 2;
            }
        }
        if (Card.passiveSkills.Adrenaline)
        {
            ManageAdrenalineAttackPhase(ref atkNow, owner, enemy, shouldSkip);
        }
        else
        {
            if (!shouldSkip)
            {
                AttackPhase(ref atkNow, owner, enemy, false);
            }
            (Id, Card).CreatureTurnDownTick();
        } 
        DisplayCard(new UpdateCreatureCardEvent(Id, Card, true));
    }

    private void ManageAdrenalineAttackPhase(ref int atkNow, PlayerManager owner, PlayerManager enemy, bool shouldSkip)
    {
        var initialAtk = Mathf.Abs(Card.AtkNow);
        var adrenalineIndex = 0;
        var shouldContinue = true;
        while (shouldContinue)
        {
            if (!shouldSkip)
            {
                AttackPhase(ref atkNow, owner, enemy, adrenalineIndex < 2);
            }
            (Id, Card).CreatureTurnDownTick();
        
            adrenalineIndex++;
            shouldContinue = SetupAdrenaline(ref atkNow, initialAtk, adrenalineIndex);
            shouldSkip = owner.playerCounters.delay > 0 ||
                         owner.playerCounters.patience > 0;
        }
    }
    private void AttackPhase(ref int atkNow, PlayerManager owner, PlayerManager enemy, bool skipAdr)
    {
        Card.WeaponPassive?.ModifyWeaponAtk(Id, ref atkNow);
        var isFreedomEffect = Random.Range(0, 100) < 25 * owner.playerCounters.freedom &&
                              Card.CostElement.Equals(Element.Air);
        atkNow = Mathf.FloorToInt(isFreedomEffect ? atkNow * 1.5f : atkNow);
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("CreatureDamage"));
        if (atkNow > 0)
        {
            if (enemy.HasGravityCreatures())
            {
                enemy.ManageGravityCreatures(ref atkNow);
            }

            if (!Card.passiveSkills.Momentum)
            {
                atkNow = enemy.ManageShield(atkNow, (Id, Card));
            }
        }
        if (Card.Counters.Delay <= 0 && !skipAdr)
        {
            Card.TurnEndAbility?.Activate(Id, Card);
        }
        
        if (atkNow == 0) return;
        if (Card.passiveSkills.Vampire)
        {
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, false, false, owner.owner));
        }
        
        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, true, Card.passiveSkills.Psion,
            enemy.owner));
        EventBus<DisplayCreatureAttackEvent>.Raise(new DisplayCreatureAttackEvent(Id, atkNow));

        if (atkNow <= 0) return;
        if (Card.passiveSkills.Venom && !skipAdr)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(
                new ModifyPlayerCounterEvent(PlayerCounters.Poison, enemy.owner, 1));
        }

        if (Card.passiveSkills.DeadlyVenom && !skipAdr)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(
                new ModifyPlayerCounterEvent(PlayerCounters.Poison, enemy.owner, 2));
        }

        if (Card.passiveSkills.Neurotoxin && !skipAdr)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(
                new ModifyPlayerCounterEvent(PlayerCounters.Neurotoxin, enemy.owner, 1));
        }
    }

    private bool SetupAdrenaline(ref int atkNow, int initialAtk, int adrIndex)
    {
        var adrAtkList = DuelManager.AdrenalineDamageList[initialAtk];
        if (atkNow == 0) return false;
        if (adrAtkList.Count <= adrIndex) return false;
        
        atkNow = adrAtkList[adrIndex];
        if (Card.passiveSkills.Antimatter) 
        {
            atkNow = -atkNow;
        }

        return true;
    }
}

public struct UpdateCloakParentEvent : IEvent
{
    public Transform Transform;
    public ID Id;
    public bool IsAdd;

    public UpdateCloakParentEvent(Transform transform, ID id, bool isAdd)
    {
        Transform = transform;
        Id = id;
        IsAdd = isAdd;
    }
}