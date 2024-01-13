using System.Collections;
using System.Linq;
using Battlefield.Abstract;
using Elements.Duel.Visual;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreatureCardDisplay : CardFieldDisplay, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image cardImage,
        upgradeShine,
        rareIndicator,
        cardHeadBack,
        activeAElement;

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
        cardImage.sprite = ImageHelper.GetCardImage(updateCardDisplayEvent.Card.imageID);
        var creatureAtk = updateCardDisplayEvent.Card.passiveSkills.Antimatter
            ? -updateCardDisplayEvent.Card.AtkNow
            : updateCardDisplayEvent.Card.AtkNow;
        creatureValue.text = $"{creatureAtk}|{updateCardDisplayEvent.Card.DefNow}";
        cardName.text = updateCardDisplayEvent.Card.cardName;
        cardHeadBack.sprite =
            ImageHelper.GetCardHeadBackground(updateCardDisplayEvent.Card.costElement.FastElementString());
        
        var isRareCard = updateCardDisplayEvent.Card.IsRare();
        upgradeShine.gameObject.SetActive(isRareCard);

        cardName.font = isRareCard ? underlayWhite : underlayBlack;
        cardName.color = isRareCard ? new Color32(0, 0, 0, 255) : new Color32(255, 255, 255, 255);

        rareIndicator.gameObject.SetActive(updateCardDisplayEvent.Card.IsRare());

        SetupActiveAbility();
        if (updateCardDisplayEvent.IsUpdate) return;
        CheckOnPlayEffects();
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

        if (Card.skill == "") return;
        
        activeAHolder.SetActive(true);
        activeAName.text = Card.skill;
        var hasCost = Card.skillCost > 0;
        activeACost.text = hasCost ? Card.skillCost.ToString() : "";
        activeAElement.color = hasCost ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);
        
        activeAElement.sprite = hasCost ?
            ImageHelper.GetElementImage(Card.skillElement.FastElementString()) : null;
    }
    
    private void CreatureAttack(DisplayCreatureAttackEvent onDisplayCreatureAttackEvent)
    {
        if (!onDisplayCreatureAttackEvent.AttackingId.Equals(Id)) return;
        
        StartCoroutine(ShowDamage(onDisplayCreatureAttackEvent.AttackValue));
    }
    private IEnumerator ShowDamage(int damage)
    {
        if (damage <= 0) { yield break; }
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
        
        EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Scarab, Id.owner, Card.innateSkills.Swarm ? -1 : 0));
        
        var shouldDeath = ShouldActivateDeathTriggers();
        if (shouldDeath)
        {
            EventBus<OnDeathTriggerEvent>.Raise(new OnDeathTriggerEvent());
        }
        if (Card.IsAflatoxin)
        {
            var card = CardDatabase.Instance.GetCardFromId("6ro");
            DisplayCard(new UpdateCreatureCardEvent(Id, card, false));
        } 
        else if (Card.passiveSkills.Phoenix && !shouldDeath)
        {
            var card = CardDatabase.Instance.GetCardFromId(Card.iD.IsUpgraded() ? "7dt" : "5fd");
            DisplayCard(new UpdateCreatureCardEvent(Id, card, false));
        }
        else
        {
            EventBus<RemoveCardFromManagerEvent>.Raise(new RemoveCardFromManagerEvent(Id));
            Destroy(gameObject);
        }
        
    }

    private void CheckOnPlayEffects()
    {
        if (Card.innateSkills.Swarm)
        {
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Scarab, Id.owner, 1));
        }

        if (Card.innateSkills.Integrity)
        {
            var shardList = DuelManager.Instance.GetIDOwner(Id).playerHand.GetAllValidCardIds().FindAll(x => x.card.cardName.Contains("Shard of"));
            SetCard(CardDatabase.Instance.GetGolemAbility(shardList));
        }

        if (Card.innateSkills.Chimera)
        {
            var creatureList = DuelManager.Instance.GetIDOwner(Id).playerCreatureField.GetAllValidCardIds();
            var chimeraPwrHp = (0, 0);

            if (creatureList.Count > 0)
            {
                foreach (var creature in creatureList.Where(creature => !creature.Item1.Equals(Id)))
                {
                    chimeraPwrHp.Item1 += creature.Item2.AtkNow;
                    chimeraPwrHp.Item2 += creature.Item2.DefNow;
                    EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(creature.Item1));
                }
            }

            Card.atk = chimeraPwrHp.Item1;
            Card.def = chimeraPwrHp.Item2;
        }

        if (!Card.costElement.Equals(Element.Darkness)
            && !Card.costElement.Equals(Element.Death)) return;
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

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var rectTransform = GetComponent<RectTransform>();
        Vector2 objectSize = new(rectTransform.rect.height, rectTransform.rect.width);
        ToolTipCanvas.Instance.SetupToolTip(new Vector2(transform.position.x, transform.position.y), objectSize, Card, Id.index + 1, Id.field == FieldEnum.Creature);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipCanvas.Instance.HideToolTip();
    }
    
    private bool ShouldActivateDeathTriggers()
    {
        if (BattleVars.Shared.AbilityCardOrigin is not null)
        {
            return BattleVars.Shared.AbilityCardOrigin.skill is not "reversetime";
        }
        return Card.cardName.Contains("Skeleton");
    }

    private void DeathTrigger(OnDeathTriggerEvent onDeathTriggerEvent)
    {
        if (!Card.passiveSkills.Scavenger) return;
        Card.AtkModify += 1;
        Card.DefModify += 1;
        DisplayCard(new UpdateCreatureCardEvent(Id, Card, true));
    }

    private void OnTurnEnd(OnCreatureTurnEndEvent onTurnEndEvent)
    {
        if (!onTurnEndEvent.Owner.Equals(Id.owner)) return;

        var owner = DuelManager.Instance.GetIDOwner(Id);
        var enemy = DuelManager.Instance.GetNotIDOwner(Id);
        var adrenalineIndex = 0;
        var hasAdrenaline = Card.passiveSkills.Adrenaline;
        var isFirstAttack = true;
        var atkNow = Card.AtkNow;

        var shouldSkip = DuelManager.Instance.GetCardCount(new() { "5rp", "7q9" }) > 0 || 
                         owner.playerCounters.patience > 0 || 
                         Card.Freeze > 0 || 
                         Card.innateSkills.Delay > 0;
        Card.AbilityUsed = false;

        while (isFirstAttack || hasAdrenaline)
        {
            isFirstAttack = false;
            if (!shouldSkip)
            {
                (Id, Card).CardInnateEffects(ref atkNow);
                var isFreedomEffect = Random.Range(0, 100) < 25 * owner.playerCounters.freedom && Card.costElement.Equals(Element.Air);
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
                        if (Card.DefNow <= 0) 
                        {
                            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(Id)); 
                            return;
                        }
                        
                    }
                }

                if (adrenalineIndex < 2)
                {
                    (Id, Card).EndTurnPassiveEffect();

                    if (atkNow > 0)
                    {
                        if (Card.passiveSkills.Venom)
                        {
                            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, enemy.Owner, 1));
                        }

                        if (Card.passiveSkills.DeadlyVenom)
                        {
                            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Poison, enemy.Owner, 2));
                        }

                        if (Card.passiveSkills.Neurotoxin)
                        {
                            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Neurotoxin, enemy.Owner, 1));
                        }
                    }
                }

                if (atkNow != 0)
                {
                    if (Card.passiveSkills.Vampire)
                    {
                        EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, false, false, owner.Owner));
                    }
                    EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(atkNow, true, Card.passiveSkills.Psion, enemy.Owner));
                    EventBus<DisplayCreatureAttackEvent>.Raise(new DisplayCreatureAttackEvent(Id, atkNow));
                }

                (Id, Card).SingularityEffect();
                DisplayCard(new UpdateCreatureCardEvent(Id, Card, true));
                if (Card.AtkNow != 0 && hasAdrenaline)
                {
                    adrenalineIndex++;
                    if (DuelManager.AdrenalineDamageList[Mathf.Abs(Card.AtkNow) - 1].Count <= adrenalineIndex)
                    {
                        hasAdrenaline = false;
                    }
                    else
                    {
                        atkNow = DuelManager.AdrenalineDamageList[Mathf.Abs(Card.AtkNow) - 1][adrenalineIndex];
                        if (Card.passiveSkills.Antimatter)
                        {
                            atkNow = -atkNow;
                        }
                    }
                }
            }
            (Id, Card).CreatureTurnDownTick();
            DisplayCard(new UpdateCreatureCardEvent(Id, Card, true));
        }
    }
}