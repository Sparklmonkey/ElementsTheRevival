using System.Collections.Generic;
using UnityEngine;

public delegate bool IsCardValidTarget(ID id, Card card);
public delegate void ActivateAbilityEffect(ID id, Card card);
public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    private void OnEnable()
    {
        _setupAbilityTargetsEventBinding = new EventBinding<SetupAbilityTargetsEvent>(SetupTargetHighlights);
        EventBus<SetupAbilityTargetsEvent>.Register(_setupAbilityTargetsEventBinding);
    }

    private void OnDisable()
    {
        EventBus<SetupAbilityTargetsEvent>.Unregister(_setupAbilityTargetsEventBinding);
    }
    private EventBinding<SetupAbilityTargetsEvent> _setupAbilityTargetsEventBinding;

    private void Awake()
    {
        Instance = this;
    }

    public bool ShouldAskForTarget(Card card)
    {
        var ability = card.skill.GetSkillScript<AbilityEffect>();
        return ability.NeedsTarget();
    }

    private void SetupTargetHighlights(SetupAbilityTargetsEvent setupAbilityTargetsEvent)
    {
        var ability = setupAbilityTargetsEvent.AbilityCard.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = setupAbilityTargetsEvent.AbilityOwner;
        ability.Origin = setupAbilityTargetsEvent.AbilityCard;
        EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(ability.IsCardValid));
    }
    
    public void SkillRoutineNoTarget(PlayerManager owner, ID id, Card card)
    {
        var ability = card.skill.GetSkillScript<AbilityEffect>();
        if (card.cardType.Equals(CardType.Spell))
        {
            EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(owner.Owner.Equals(OwnerEnum.Player), card, null, null));
        }
        else
        {
            EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(owner.Owner.Equals(OwnerEnum.Player), card, null, null));
        }
        ability.Owner = owner;
        ability.Activate(id, card);
    }
}