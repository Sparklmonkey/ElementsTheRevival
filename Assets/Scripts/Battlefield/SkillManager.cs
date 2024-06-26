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
        return card.Skill.NeedsTarget();
    }

    private void SetupTargetHighlights(SetupAbilityTargetsEvent setupAbilityTargetsEvent)
    {
        var ability = setupAbilityTargetsEvent.AbilityCard.Skill;
        EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(ability.IsCardValid, setupAbilityTargetsEvent.ShouldHideGraphic));
    }
    
    public void SkillRoutineNoTarget(PlayerManager owner, ID id, Card card)
    {
        var ability = card.Skill;
        if (card.Type.Equals(CardType.Spell))
        {
            EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(owner.owner.Equals(OwnerEnum.Player), card, null, null));
        }
        else
        {
            EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(owner.owner.Equals(OwnerEnum.Player), card, null, null));
        }
        EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(ability.IsCardValid, true));
        foreach (var target in DuelManager.Instance.ValidTargets)
        {
            EventBus<ActivateAbilityEffectEvent>.Raise(new ActivateAbilityEffectEvent(ability.Activate, target.Key));
        }
    }
}