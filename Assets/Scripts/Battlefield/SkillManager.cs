using System.Collections.Generic;

public class SkillManager
{
    static SkillManager()
    {
    }

    private SkillManager()
    {
    }

    public static SkillManager Instance { get; } = new();

    public bool ShouldAskForTarget(IDCardPair idCard)
    {
        var ability = idCard.card.skill.GetSkillScript<AbilityEffect>();
        return ability.NeedsTarget();
    }

    public bool HasEnoughTargets(PlayerManager owner, IDCardPair card)
    {
        var ability = card.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        ability.Origin = card;
        var enemy = DuelManager.Instance.GetNotIDOwner(owner.playerID.id);
        var targets = ability.GetPossibleTargets(enemy);
        if (ability.NeedsTarget())
        {
            return targets.Count > 0;
        }

        return true;
    }

    public void SetupTargetHighlights(PlayerManager owner, IDCardPair card)
    {
        var ability = card.card.skill.GetSkillScript<AbilityEffect>();
        ability.Owner = owner;
        ability.Origin = card;
        var enemy = DuelManager.Instance.GetNotIDOwner(owner.playerID.id);
        DuelManager.Instance.SetupHighlights(ability.GetPossibleTargets(enemy));
    }

    public void SkillRoutineWithTarget(PlayerManager owner, IDCardPair target)
    {
        var ability = BattleVars.Shared.AbilityOrigin.card.skill.GetSkillScript<AbilityEffect>();
        if (BattleVars.Shared.AbilityOrigin.card.cardType.Equals(CardType.Spell))
        {
            EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(owner.isPlayer, BattleVars.Shared.AbilityOrigin.card, target));
        }
        else
        {
            EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(owner.isPlayer, BattleVars.Shared.AbilityOrigin.card, target));
        }
        ability.Owner = owner;
        ability.Origin = BattleVars.Shared.AbilityOrigin;

        ability.Activate(target);
    }

    public void SkillRoutineNoTarget(PlayerManager owner, IDCardPair idCard)
    {
        var ability = idCard.card.skill.GetSkillScript<AbilityEffect>();
        if (idCard.card.cardType.Equals(CardType.Spell))
        {
            EventBus<AddSpellActivatedActionEvent>.Raise(new AddSpellActivatedActionEvent(owner.isPlayer, idCard.card, null));
        }
        else
        {
            EventBus<AddAbilityActivatedActionEvent>.Raise(new AddAbilityActivatedActionEvent(owner.isPlayer, idCard.card, null));
        }
        ability.Owner = owner;
        ability.Activate(idCard);
    }

    public IDCardPair GetRandomTarget(PlayerManager owner, IDCardPair iDCard)
    {
        var ability = iDCard.card.skill.GetSkillScript<AbilityEffect>();
        if (ability == null)
        {
            return null;
        }
        ability.Owner = owner;
        ability.Origin = iDCard;
        var targets = ability.GetPossibleTargets(DuelManager.Instance.GetNotIDOwner(owner.playerID.id));
        if (targets.Count == 0)
        {
            return null;
        }

        return GetHighestPriorityTarget(targets, ability.GetPriority());
    }

    public IDCardPair GetHighestPriorityTarget(List<IDCardPair> targetList, TargetPriority priority)
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
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 0;
                    score += possibleTarget.card.AtkNow * 5;
                    score += possibleTarget.card.DefNow;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }
                    break;
                }
                case TargetPriority.SelfLowAtk:
                {
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 0;
                    score -= possibleTarget.card.AtkNow;
                    score += possibleTarget.card.DefNow;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.OpHighAtk:
                {
                    score += possibleTarget.id.owner == OwnerEnum.Player ? 75 : 0;
                    if (possibleTarget.id.field == FieldEnum.Player)
                    {
                        score += 20;
                    }
                    else
                    {
                        score += possibleTarget.card.AtkNow * 5;
                        score += possibleTarget.card.DefNow;
                        score += possibleTarget.card.skill != "" ? 15 : 5;
                    }
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.OpLowAtk:
                {
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 0;
                    score -= possibleTarget.card.AtkNow;
                    score += possibleTarget.card.DefNow;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.Pillar:
                {
                    score += possibleTarget.id.owner == OwnerEnum.Player ? 75 : 0;
                    score += possibleTarget.card.cardType == CardType.Pillar ? 50 : 0;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.OwnPillar:
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 0;
                    score += possibleTarget.card.cardType == CardType.Pillar ? 50 : 0;
                    if (score < 125)
                    {
                        score = 0;
                    }
                    break;
                case TargetPriority.Permanent:
                    score += possibleTarget.card.cardType == CardType.Artifact ? 75 : 0;
                    score += possibleTarget.card.cardType == CardType.Weapon ? 75 : 0;
                    score += possibleTarget.card.cardType == CardType.Shield ? 75 : 0;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.Any:
                    score += possibleTarget.card.AtkNow;
                    score += possibleTarget.card.DefNow;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.AnyHighAtk:
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 50;
                    score += possibleTarget.card.AtkNow * 5;
                    score += possibleTarget.card.DefNow;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.IsPoisoned:
                {
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 100 : 0;
                    if (possibleTarget.id.field == FieldEnum.Creature)
                    {
                        score += possibleTarget.card.IsAflatoxin ? 25 : 0;
                        score += possibleTarget.card.Poison > 0 ? 25 : 0;
                        score += possibleTarget.card.AtkNow;
                        score += possibleTarget.card.skill != "" ? 15 : 5;
                    }
                    if (score < 100)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.IsFrozen:
                {
                    score += possibleTarget.id.owner == OwnerEnum.Player ? 75 : 0;
                    score += possibleTarget.card.Freeze > 0 ? 75 : 0;
                    score += possibleTarget.card.AtkNow;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    if (score < 75)
                    {
                        score = 0;
                    }

                    break;
                }
                case TargetPriority.HighestHp:
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 50;
                    score += possibleTarget.card.DefNow * 5;
                    score += possibleTarget.card.AtkNow;
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    break;
                case TargetPriority.LowestHp:
                {
                    score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 50;
                    if (possibleTarget.card.cardType == CardType.Creature)
                    {
                        score += possibleTarget.card.DefNow * 5;
                        score += possibleTarget.card.AtkNow;
                    }
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    break;
                }
                case TargetPriority.HighestCost:
                    score += possibleTarget.card.cost * 5;
                    score += possibleTarget.card.skill == "" ? 15 : 5;
                    score -= possibleTarget.card.AtkNow;
                    score -= possibleTarget.card.DefNow;
                    break;
                case TargetPriority.HasSkill:
                    score += possibleTarget.card.skill != "" ? 15 : 5;
                    score += possibleTarget.card.AtkNow;
                    score += possibleTarget.card.DefNow;
                    break;
            }

            if (score <= currentScore) continue;
            currentTarget = possibleTarget;
            currentScore = score;
        }

        return currentScore == 0 ? null : currentTarget;
    }
}