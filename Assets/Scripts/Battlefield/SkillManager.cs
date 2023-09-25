using System;
using System.Collections.Generic;

public class SkillManager
{
    private static readonly SkillManager _instance = new();

    static SkillManager()
    {
    }

    private SkillManager()
    {
    }

    public static SkillManager Instance => _instance;

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
        ActionManager.AddAbilityActivatedAction(owner.isPlayer, BattleVars.Shared.AbilityOrigin, target);
        ability.Owner = owner;
        ability.Origin = BattleVars.Shared.AbilityOrigin;

        ability.Activate(target);
    }

    public void SkillRoutineNoTarget(PlayerManager owner, IDCardPair idCard)
    {
        var ability = idCard.card.skill.GetSkillScript<AbilityEffect>();
        ActionManager.AddAbilityActivatedAction(owner.isPlayer, idCard, owner.playerID);
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
            if (priority == TargetPriority.SelfHighAtk)
            {
                score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 0;
                score += possibleTarget.card.AtkNow * 5;
                score += possibleTarget.card.DefNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
                if (score < 75)
                {
                    score = 0;
                }
            }
            else if (priority == TargetPriority.SelfLowAtk)
            {
                score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 0;
                score -= possibleTarget.card.AtkNow;
                score += possibleTarget.card.DefNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
                if (score < 75)
                {
                    score = 0;
                }
            }
            else if (priority == TargetPriority.OpHighAtk)
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
            }
            else if (priority == TargetPriority.OpLowAtk)
            {
                score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 0;
                score -= possibleTarget.card.AtkNow;
                score += possibleTarget.card.DefNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
                if (score < 75)
                {
                    score = 0;
                }
            }
            else if (priority == TargetPriority.Pillar)
            {
                score += possibleTarget.id.owner == OwnerEnum.Player ? 75 : 0;
                score += possibleTarget.card.cardType == CardType.Pillar ? 50 : 0;
                score += possibleTarget.card.skill != "" ? 15 : 5;
                if (score < 75)
                {
                    score = 0;
                }
            }
            else if (priority == TargetPriority.OwnPillar)
            {
                score = 75;
            }
            else if (priority == TargetPriority.Permanent)
            {
                score += possibleTarget.card.cardType == CardType.Artifact ? 75 : 0;
                score += possibleTarget.card.cardType == CardType.Weapon ? 75 : 0;
                score += possibleTarget.card.cardType == CardType.Shield ? 75 : 0;
                score += possibleTarget.card.skill != "" ? 15 : 5;
            }
            else if (priority == TargetPriority.Any)
            {
                score += possibleTarget.card.AtkNow;
                score += possibleTarget.card.DefNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
            }
            else if (priority == TargetPriority.AnyHighAtk)
            {
                score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 50;
                score += possibleTarget.card.AtkNow * 5;
                score += possibleTarget.card.DefNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
            }
            else if (priority == TargetPriority.IsPoisoned)
            {
                score += possibleTarget.id.owner == OwnerEnum.Opponent ? 100 : 0;
                score += possibleTarget.card.IsAflatoxin ? 25 : 0;
                score += possibleTarget.card.Poison > 0 ? 25 : 0;
                score += possibleTarget.card.AtkNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
                if (score < 100)
                {
                    score = 0;
                }
            }
            else if (priority == TargetPriority.IsFrozen)
            {
                score += possibleTarget.id.owner == OwnerEnum.Player ? 75 : 0;
                score += possibleTarget.card.Freeze > 0 ? 75 : 0;
                score += possibleTarget.card.AtkNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
                if (score < 75)
                {
                    score = 0;
                }
            }
            else if (priority == TargetPriority.HighestHp)
            {
                score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 50;
                score += possibleTarget.card.DefNow * 5;
                score += possibleTarget.card.AtkNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
            }
            else if (priority == TargetPriority.LowestHp)
            {
                score += possibleTarget.id.owner == OwnerEnum.Opponent ? 75 : 50;
                score += possibleTarget.card.DefNow * 5;
                score += possibleTarget.card.AtkNow;
                score += possibleTarget.card.skill != "" ? 15 : 5;
            }
            else if (priority == TargetPriority.HighestCost)
            {
                score += possibleTarget.card.cost * 5;
                score += possibleTarget.card.skill == "" ? 15 : 5;
                score -= possibleTarget.card.AtkNow;
                score -= possibleTarget.card.DefNow;
            }
            else if (priority == TargetPriority.HasSkill)
            {
                score += possibleTarget.card.skill != "" ? 15 : 5;
                score += possibleTarget.card.AtkNow;
                score += possibleTarget.card.DefNow;
            }

            if (score <= currentScore) continue;
            currentTarget = possibleTarget;
            currentScore = score;
        }

        return currentScore == 0 ? null : currentTarget;
    }
}