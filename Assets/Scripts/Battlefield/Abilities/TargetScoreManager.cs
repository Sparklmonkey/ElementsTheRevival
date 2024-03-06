using System;
using Core.Helpers;
using Random = UnityEngine.Random;

namespace Battlefield.Abilities
{
    public static class TargetScoreManager
    {
        public static float CalculateCreatureLowAtkScore(this (ID id, Card card) target, float estimate, int defineValue)
        {
            var score = target.id.owner switch
            {
                OwnerEnum.Opponent when target.card.AtkNow < defineValue => estimate *
                    (target.card.DefNow - target.card.Counters.Poison) / 10f,
                OwnerEnum.Player when target.card.AtkNow < defineValue => 0f,
                _ => 0f
            };

            return score;
        }

        public static float CalculateCreatureHighAtkScore(this (ID id, Card card) target, float estimate)
        {
            switch (target.id.owner)
            {
                case OwnerEnum.Opponent when target.card.AtkNow > target.card.DefNow:
                {
                    if (target.card.Counters.Poison > 0 || target.card.Def - target.card.DefNow > 0)
                    {
                        return estimate;
                    }

                    return 0f;
                }
                case OwnerEnum.Player when target.card.AtkNow > target.card.DefNow:
                {
                    if (target.card.Counters.Poison > 0)
                    {
                        return 0f;
                    }

                    return target.card.AtkNow / -estimate / 20f;
                }
                default:
                    return 0f;
            }
        }

        public static float CalculateImmortalScore(this (ID id, Card card) target, float estimate, bool onlyFriend,
            bool onlyFoe)
        {
            var score = 0f;

            if (onlyFoe && target.id.IsOwnedBy(OwnerEnum.Player))
            {
                score = estimate / 1f;
            }

            if (onlyFriend && target.id.IsOwnedBy(OwnerEnum.Opponent))
            {
                score = 2f * estimate / 1f;
            }

            return score;
        }

        public static float CalculateTrebuchetScore(this (ID id, Card card) target, float estimate)
        {
            var score = target.id.owner switch
            {
                OwnerEnum.Opponent => (estimate * target.card.DefNow /
                    (DuelManager.Instance.player.HealthManager.GetCurrentHealth() + 1f) + target.card.Counters.Freeze +
                    target.card.Counters.Poison * 3f - target.card.AtkNow) / 20f,
                OwnerEnum.Player => 0f,
                _ => 0f
            };

            return score;
        }

        public static float CalculateSmallerScore(this (ID id, Card card) target, float estimate)
        {
            var score = 0f;
            if (target.id.IsOwnedBy(OwnerEnum.Opponent))
            {
                if (target.card.Counters.Poison > 0 ||
                    target.card.AtkNow + target.card.DefNow <= 2 && target.card.Skill is null)
                {
                    score = (-estimate) / 10f;
                }
                else
                {
                    score = (estimate) / target.card.AtkNow;
                }
            }
            else
            {
                if (target.card.Skill is not null)
                {
                    score = 3f;
                }

                score = (score + 1f + target.card.AtkNow / (-estimate)) / 10f;
            }

            return score;
        }

        public static float CalculateSkillCreatureScore(this (ID id, Card card) target, float estimate)
        {
            switch (target.id.owner)
            {
                case OwnerEnum.Player when target.card.Skill is not null:
                    return (target.card.Cost + 1f) * -estimate;
                case OwnerEnum.Player:
                    return 0f;
                case OwnerEnum.Opponent when target.card.Skill is not null:
                {
                    if (target.card.CostElement == Element.Life)
                    {
                        return target.card.SkillCost * estimate * 3f;
                    }

                    return (target.card.SkillCost + 1f) * estimate;
                }
                case OwnerEnum.Opponent:
                    return 0f;
                default:
                    return 0f;
            }
        }

        public static float CalculatePillarScore(this (ID id, Card card) target, float estimate)
        {
            var stackCount = DuelManager.Instance.GetIDOwner(target.id).playerPermanentManager
                .GetStackCountForId(target.id);
            return target.id.owner switch
            {
                OwnerEnum.Opponent => (estimate + Random.Range(0f, 1f) / 10f) / 2f,
                OwnerEnum.Player => stackCount / (Math.Abs(stackCount - estimate) + 0.5f) * 10 *
                                          -estimate,
                _ => 0
            };
        }

        public static float CalculateDefineDefScore(this (ID id, Card card) target, float estimate, bool onlyFriend,
            bool onlyFoe, int defineValue)
        {
            if (target.id.IsOwnedBy(OwnerEnum.Opponent) && !onlyFoe)
            {
                return estimate * (target.card.DefNow - defineValue + 0f) / defineValue;
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player) && !onlyFriend)
            {
                return -estimate * 0.1f * (target.card.Cost + target.card.AtkNow) *
                             ((target.card.DefNow - defineValue - 1f) /
                              (Math.Abs(target.card.DefNow - defineValue - 1f) + 0.001f));
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player))
            {
                return -estimate * 0.1f * (target.card.Cost + target.card.AtkNow) *
                    (target.card.DefNow - defineValue - 1) / (Math.Abs(target.card.DefNow - defineValue - 1) + 0.001f);
            }

            return 0;
        }

        public static float CalculateDefineAtk(this (ID id, Card card) target, float estimate, bool onlyFoe,
            int defineValue, int defineTolerance)
        {
            if (target.id.IsOwnedBy(OwnerEnum.Opponent) && !onlyFoe)
            {
                if (target.card.passiveSkills.Adrenaline)
                {
                    return 0f;
                }

                if (target.card.Skill is Mitosis)
                {
                    return 0f;
                }

                if (target.card.TurnEndAbility is SingularityEndTurn && target.card.passiveSkills.Antimatter)
                {
                    return 0f;
                }

                return estimate * (defineTolerance - Math.Abs(defineValue - target.card.AtkNow)) / 10f;
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player))
            {
                return -estimate * (defineTolerance - Math.Abs(defineValue - target.card.AtkNow)) / 10f;
            }

            return 0f;
        }

        public static float CalculateFractalScore(this (ID id, Card card) target, float estimate)
        {
            if (target.id.IsOwnedBy(OwnerEnum.Opponent))
            {
                if (estimate > 0)
                {
                    return (DuelManager.Instance.enemy.GetAllQuantaOfElement(target.card.CostElement) + 1f) /
                                 (target.card.Cost + 0.2f) /
                                 (DuelManager.Instance.enemy.playerHand.GetHandCount() *
                                     DuelManager.Instance.enemy.playerHand.GetHandCount() * 5f + 1f);
                }

                if (target.card.innateSkills.Obsession)
                {
                    return 1f;
                }

                return (target.card.Cost /
                              (DuelManager.Instance.player.GetAllQuantaOfElement(target.card.CostElement) + 1f) +
                              0.2f) /
                             (DuelManager.Instance.player.playerHand.GetHandCount() *
                                 DuelManager.Instance.player.playerHand.GetHandCount() * 5f + 1f);
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player))
            {
                if (estimate > 0)
                {
                    return (DuelManager.Instance.enemy.GetAllQuantaOfElement(target.card.CostElement) + 1f) /
                                 (target.card.Cost + 0.2f) /
                                 (DuelManager.Instance.enemy.playerHand.GetHandCount() *
                                     DuelManager.Instance.enemy.playerHand.GetHandCount() * 5f + 1f);
                }

                return (target.card.Cost /
                              (DuelManager.Instance.player.GetAllQuantaOfElement(target.card.CostElement) + 1f) +
                              0.2f) /
                             (DuelManager.Instance.player.playerHand.GetHandCount() *
                                 DuelManager.Instance.player.playerHand.GetHandCount() * 5f + 1f);
            }

            return 0f;
        }


        public static float CalculatePermanentScore(this (ID id, Card card) target, float estimate, string skill)
        {
            if (target.id.IsOwnedBy(OwnerEnum.Opponent))
            {
                return (target.card.Cost + 1f) * estimate / 25f - DuelManager.Instance.enemy.playerHand.GetHandCount();
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player))
            {
                if (nameof(target.card.Skill) == skill) return 0f;
                if (target.card.PlayRemoveAbility is SundialPlayRemoveAbility && skill != "steal")
                {
                    return DuelManager.Instance.enemy.GetPossibleDamage() * -estimate / 20f;
                }

                return (target.card.Cost + 1f) * -estimate / 20;
            }

            return 0f;
        }

        public static float CalculateWeaponScore(this (ID id, Card card) target)
        {
            var skillScore = 0f;
            if (target.card.SkillCost > 0)
            {
                skillScore =
                    DuelManager.Instance.GetIDOwner(target.id).GetAllQuantaOfElement(target.card.SkillElement) /
                    (1f + target.card.SkillCost);
            }
            else
            {
                skillScore = 1f;
            }

            if (target.id.field.Equals(FieldEnum.Passive))
            {
                return target.card.Atk + skillScore;
            }

            if (target.id.IsCreatureField() &&
                CardDatabase.Instance.WeaponIdList.Contains(target.card.Id))
            {
                return target.card.Atk + skillScore;
            }

            return 0f;
        }

        public static float CalculateTearsScore(this (ID id, Card card) target, float estimate)
        {
            return estimate + Random.Range(0, 10) / 2f;
        }

        public static float CalculateBetaCreatureScore(this (ID id, Card card) target, float estimate, bool onlyFriend,
            bool onlyFoe)
        {
            var skillScore = target.card.Skill is not null ? 3f : 0f;

            if (target.id.IsOwnedBy(OwnerEnum.Opponent) && !onlyFoe)
            {
                return (7f * estimate - (target.card.DefNow + target.card.AtkNow + target.card.Cost + skillScore)) / 5f;
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player) && !onlyFriend)
            {
                return (-8f + (target.card.DefNow + target.card.AtkNow + skillScore)) / 100;
            }

            return 0f;
        }


        public static float CalculateAlphaCreatureScore(this (ID id, Card card) target, bool onlyFriend, bool onlyFoe)
        {
            var skillScore = target.card.Skill is not null ? 3f : 0f;
            if (target.id.IsOwnedBy(OwnerEnum.Opponent) && !onlyFoe)
            {
                if (target.card.Skill is Dejavu || target.card.TurnEndAbility is DevourerEndTurn || target.card.DefNow <= 0)
                {
                    skillScore = 10f;
                }

                if (target.card.Skill is Hatch)
                {
                    return 0f;
                }

                if (target.card.passiveSkills.Momentum && BattleVars.Shared.AbilityCardOrigin.Skill is Momentum)
                {
                    return 0f;
                }

                if (target.card.PlayRemoveAbility is ChimeraPlayRemoveAbility && BattleVars.Shared.AbilityCardOrigin.Skill is Paralleluniverse)
                {
                    return 0f;
                }

                return target.card.AtkNow + skillScore - target.card.Counters.Freeze - target.card.Counters.Delay / 10f;
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player) && !onlyFriend)
            {
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Paralleluniverse &&
                    target.card.PlayRemoveAbility is ChimeraPlayRemoveAbility)
                {
                    return DuelManager.Instance.player.playerCreatureField.GetAllValidCards().Count / 5f;
                }

                return target.card.AtkNow + skillScore - target.card.Counters.Freeze - target.card.Counters.Delay / 10f;
            }

            return 0f;
        }

        public static float CalculateCreatureAndPlayerScore(this (ID id, Card card) target, float estimate, bool onlyFriend, bool onlyFoe)
        {
            if (target.id.IsOwnedBy(OwnerEnum.Opponent) && target.card is null)
            {
                return estimate * (DuelManager.Instance.enemy.HealthManager.GetMaxHealth() -
                                         DuelManager.Instance.enemy.HealthManager.GetCurrentHealth()) * 0.001f;
            }
            if (target.id.IsOwnedBy(OwnerEnum.Player) && target.card is null)
            {
                var score = -estimate / (DuelManager.Instance.enemy.HealthManager.GetCurrentHealth() * 4f);
                if (DuelManager.Instance.player.playerPassiveManager.GetShield().Item2.ShieldPassive is ReflectSkill)
                {
                    return 0f;
                }

                if (DuelManager.Instance.player.sacrificeCount > 0)
                {
                    return -score * 50f;
                }
                
                if (DuelManager.Instance.enemy.sacrificeCount > 0)
                {
                    return -score;
                }

                return score;
            }

            return target.CalculateCreatureScore(estimate);
        }

        public static float CalculateCreatureScore(this (ID id, Card card) target, float estimate)
        {
            var score = 0f;
            if (target.id.IsOwnedBy(OwnerEnum.Opponent))
            {
                if(target.card.DefNow < target.card.Def)
                {
                    score = ((0.01f * target.card.AtkNow * target.card.Def - target.card.DefNow) * estimate);
                }
                if (target.card.AtkNow < 0)
                {
                    score = (0.1f * target.card.AtkNow * estimate);
                }
                if (target.card.Counters.Poison < 0 && BattleVars.Shared.AbilityCardOrigin.Skill is Purify)
                {
                    score = 0f;
                }
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Holylight && (target.card.CostElement is Element.Death or Element.Darkness))
                {
                    score = -estimate;
                }
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Reversetime && (target.card.innateSkills.Mummy || target.card.innateSkills.Undead))
                {
                    score = 1f;
                }

                return score;
            }

            if (target.id.IsOwnedBy(OwnerEnum.Player))
            {
                var skillScore = target.card.Skill is not null ? 3 : 0;
                score = (target.card.AtkNow + 1f + skillScore) / -estimate / 20f;
                if (target.card.Counters.Freeze > 0 || target.card.Counters.Delay > 0)
                {
                    score = 0f;
                }
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Holylight && (target.card.CostElement is Element.Death or Element.Darkness))
                {
                    score = (target.card.AtkNow + 1f + skillScore) / estimate;
                }
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Reversetime && (target.card.innateSkills.Mummy || target.card.innateSkills.Undead))
                {
                    score = 0f;
                }
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Reversetime && target.card.innateSkills.Voodoo && target.card.DefNow > 25)
                {
                    score = target.card.DefNow / 25f;
                }
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Shockwave && target.card.Counters.Freeze > 0)
                {
                    score = 1f;
                }
                if (BattleVars.Shared.AbilityCardOrigin.Skill is Web && target.card.innateSkills.Airborne)
                {
                    score = 0f;
                }

                return score;
            }

            return score;
        }
    }
}