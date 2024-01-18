using System.Collections.Generic;

public delegate bool IsCardValidTarget(ID id, Card card);
public delegate void ActivateAbilityEffect(ID id, Card card);
public class SkillManager
{
    static SkillManager()
    {
    }

    private SkillManager()
    {
        _setupAbilityTargetsEventBinding = new EventBinding<SetupAbilityTargetsEvent>(SetupTargetHighlights);
        EventBus<SetupAbilityTargetsEvent>.Register(_setupAbilityTargetsEventBinding);
    }
    
    ~SkillManager()
    {
        EventBus<SetupAbilityTargetsEvent>.Unregister(_setupAbilityTargetsEventBinding);
    }
    
    private EventBinding<SetupAbilityTargetsEvent> _setupAbilityTargetsEventBinding;

    public static SkillManager Instance { get; } = new();

    public bool ShouldAskForTarget(Card card)
    {
        var ability = card.skill.GetSkillScript<AbilityEffect>();
        return ability.NeedsTarget();
    }

    public bool HasEnoughTargets(PlayerManager owner, Card card)
    {
        var ability = card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        ability.Origin = card;
        var enemy = DuelManager.Instance.GetNotIDOwner(owner.playerID);
        var targets = ability.GetPossibleTargets(enemy);
        if (ability.NeedsTarget())
        {
            return targets.Count > 0;
        }

        return true;
    }

    private void SetupTargetHighlights(SetupAbilityTargetsEvent setupAbilityTargetsEvent)
    {
        var ability = setupAbilityTargetsEvent.AbilityCard.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = setupAbilityTargetsEvent.AbilityOwner;
        ability.Origin = setupAbilityTargetsEvent.AbilityCard;
        EventBus<ShouldShowTargetableEvent>.Raise(new ShouldShowTargetableEvent(ability.IsCardValid));
    }

    public void SkillRoutineWithTarget(PlayerManager owner, ID targetId, Card targetCard)
    {
        var ability = BattleVars.Shared.AbilityCardOrigin.skill.GetSkillScript<AbilityEffect>();
        if (BattleVars.Shared.AbilityCardOrigin.cardType.Equals(CardType.Spell))
        {
            EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(owner.Owner.Equals(OwnerEnum.Player), BattleVars.Shared.AbilityCardOrigin, targetId, targetCard));
        }
        else
        {
            EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(owner.Owner.Equals(OwnerEnum.Player), BattleVars.Shared.AbilityCardOrigin, targetId, targetCard));
        }
        ability.Owner = owner;
        ability.Origin = BattleVars.Shared.AbilityCardOrigin;

        ability.Activate(targetId, targetCard);
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

    public (ID id, Card card) GetRandomTarget(PlayerManager owner, Card card)
    {
        var ability = card.skill.GetSkillScript<AbilityEffect>();
        if (ability == null)
        {
            return default;
        }
        ability.Owner = owner;
        ability.Origin = card;
        var targets = ability.GetPossibleTargets(DuelManager.Instance.GetNotIDOwner(owner.playerID));
        return targets.Count == 0 ? default : GetHighestPriorityTarget(targets, ability.GetPriority());
    }

    public (ID, Card) GetHighestPriorityTarget(List<(ID, Card)> targetList, TargetPriority priority)
    {
        var currentScore = 0;
        var currentTarget = targetList[0];

        foreach (var possibleTarget in targetList)
        {
            var score = 0;
            switch (priority)
            {
                case TargetPriority.SelfHighAtk:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 75 : 0;
                    score += possibleTarget.Item2.AtkNow * 5;
                    score += possibleTarget.Item2.DefNow;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }
                    break;
                }
                case TargetPriority.SelfLowAtk:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 75 : 0;
                    score -= possibleTarget.Item2.AtkNow;
                    score += possibleTarget.Item2.DefNow;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.OpHighAtk:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Player ? 75 : 0;
                    if (possibleTarget.Item1.field == FieldEnum.Player)
                    {
                        score += 20;
                    }
                    else
                    {
                        score += possibleTarget.Item2.AtkNow * 5;
                        score += possibleTarget.Item2.DefNow;
                        score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    }
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.OpLowAtk:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 75 : 0;
                    score -= possibleTarget.Item2.AtkNow;
                    score += possibleTarget.Item2.DefNow;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.Pillar:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Player ? 75 : 0;
                    score += possibleTarget.Item2.cardType == CardType.Pillar ? 50 : 0;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.OwnPillar:
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 75 : 0;
                    score += possibleTarget.Item2.cardType == CardType.Pillar ? 50 : 0;
                    if (score < 125)
                    {
                        score = 0;
                    }
                    break;
                case TargetPriority.Permanent:
                    score += possibleTarget.Item2.cardType == CardType.Artifact ? 75 : 0;
                    score += possibleTarget.Item2.cardType == CardType.Weapon ? 75 : 0;
                    score += possibleTarget.Item2.cardType == CardType.Shield ? 75 : 0;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.Any:
                    score += possibleTarget.Item2.AtkNow;
                    score += possibleTarget.Item2.DefNow;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.AnyHighAtk:
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 75 : 50;
                    score += possibleTarget.Item2.AtkNow * 5;
                    score += possibleTarget.Item2.DefNow;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.IsPoisoned:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 100 : 0;
                    if (possibleTarget.Item1.field == FieldEnum.Creature)
                    {
                        score += possibleTarget.Item2.IsAflatoxin ? 25 : 0;
                        score += possibleTarget.Item2.Poison > 0 ? 25 : 0;
                        score += possibleTarget.Item2.AtkNow;
                        score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    }
                    if (score < 100)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.IsFrozen:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Player ? 75 : 0;
                    score += possibleTarget.Item2.Freeze > 0 ? 75 : 0;
                    score += possibleTarget.Item2.AtkNow;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.HighestHp:
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 75 : 50;
                    score += possibleTarget.Item2.DefNow * 5;
                    score += possibleTarget.Item2.AtkNow;
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.LowestHp:
                {
                    score += possibleTarget.Item1.owner == OwnerEnum.Opponent ? 75 : 50;
                    if (possibleTarget.Item2.cardType == CardType.Creature)
                    {
                        score += possibleTarget.Item2.DefNow * 5;
                        score += possibleTarget.Item2.AtkNow;
                    }
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    break;
                }
                case TargetPriority.HighestCost:
                    score += possibleTarget.Item2.cost * 5;
                    score += possibleTarget.Item2.skill == "" ? 15 : 5;
                    score -= possibleTarget.Item2.AtkNow;
                    score -= possibleTarget.Item2.DefNow;
                    break;
                case TargetPriority.HasSkill:
                    score += possibleTarget.Item2.skill != "" ? 15 : 5;
                    score += possibleTarget.Item2.AtkNow;
                    score += possibleTarget.Item2.DefNow;
                    break;
            }

            if (score <= currentScore) continue;
            currentTarget = possibleTarget;
            currentScore = score;
        }

        return currentScore == 0 ? default : currentTarget;
    }
}