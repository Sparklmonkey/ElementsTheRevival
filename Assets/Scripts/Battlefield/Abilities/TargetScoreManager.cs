using System;
using Random = UnityEngine.Random;

namespace Battlefield.Abilities
{
    public static class TargetScoreManager
    {
        public static int CalculateCreatureLowAtkScore(this (ID id, Card card) target, int estimate, int defineValue)
        {
            var score = target.id.owner switch
            {
                OwnerEnum.Opponent when target.card.AtkNow < defineValue => estimate *
                    (target.card.DefNow - target.card.Poison) / 10,
                OwnerEnum.Player when target.card.AtkNow < defineValue => 0,
                _ => 0
            };

            return score;
        }

        public static int CalculateCreatureHighAtkScore(this (ID id, Card card) target, int estimate)
        {
            switch (target.id.owner)
            {
                case OwnerEnum.Opponent when target.card.AtkNow > target.card.DefNow:
                {
                    if (target.card.Poison > 0 || target.card.def - target.card.DefNow > 0)
                    {
                        return estimate;
                    }

                    return 0;
                }
                case OwnerEnum.Player when target.card.AtkNow > target.card.DefNow:
                {
                    if (target.card.Poison > 0)
                    {
                        return 0;
                    }

                    return target.card.AtkNow / (-estimate) / 20;
                }
                default:
                    return 0;
            }
        }

        public static int CalculateImmortalScore(this (ID id, Card card) target, int estimate, bool onlyFriend,
            bool onlyFoe)
        {
            var score = 0;

            if (onlyFoe && target.id.owner.Equals(OwnerEnum.Player))
            {
                score = estimate / 1;
            }

            if (onlyFriend && target.id.owner.Equals(OwnerEnum.Opponent))
            {
                score = 2 * estimate / 1;
            }

            return score;
        }

        public static int CalculateTrebuchetScore(this (ID id, Card card) target, int estimate)
        {
            var score = target.id.owner switch
            {
                OwnerEnum.Opponent => (estimate * target.card.DefNow /
                    (DuelManager.Instance.player.HealthManager.GetCurrentHealth() + 1) + target.card.Freeze +
                    target.card.Poison * 3 - target.card.AtkNow) / 20,
                OwnerEnum.Player => 0,
                _ => 0
            };

            return score;
        }

        public static int CalculateSmallerScore(this (ID id, Card card) target, int estimate)
        {
            var score = 0;
            if (target.id.owner.Equals(OwnerEnum.Opponent))
            {
                if (target.card.Poison > 0 ||
                    target.card.AtkNow + target.card.DefNow <= 2 && target.card.skill == "")
                {
                    score = (-estimate) / 10;
                }
                else
                {
                    score = (estimate) / target.card.AtkNow;
                }
            }
            else
            {
                if (target.card.skill != "")
                {
                    score = 3;
                }

                score = (score + 1 + target.card.AtkNow / (-estimate)) / 10;
            }

            return score;
        }

        public static int CalculateSkillCreatureScore(this (ID id, Card card) target, int estimate)
        {
            switch (target.id.owner)
            {
                case OwnerEnum.Player when target.card.skill != "":
                    return (target.card.cost + 1) * -estimate;
                case OwnerEnum.Player:
                    return 0;
                case OwnerEnum.Opponent when target.card.skill != "":
                {
                    if (target.card.costElement == Element.Life)
                    {
                        return target.card.skillCost * estimate * 3;
                    }

                    return (target.card.skillCost + 1) * estimate;
                }
                case OwnerEnum.Opponent:
                    return 0;
                default:
                    return 0;
            }
        }

        public static int CalculatePillarScore(this (ID id, Card card) target, int estimate)
        {
            var stackCount = DuelManager.Instance.GetIDOwner(target.id).playerPermanentManager
                .GetStackCountForId(target.id);
            return target.id.owner switch
            {
                OwnerEnum.Opponent => (int)((estimate + Random.Range(0f, 1f) / 10) / 2),
                OwnerEnum.Player => (int)(stackCount / (Math.Abs(stackCount - estimate) + 0.5f) * 10 *
                                          -estimate), //(int)((estimate + 0.5) * 2 * -estimate),
                _ => 0
            };
        }

        public static int CalculateDefineDefScore(this (ID id, Card card) target, int estimate, bool onlyFriend,
            bool onlyFoe, int defineValue)
        {
            if (target.id.owner.Equals(OwnerEnum.Opponent) && !onlyFoe)
            {
                return estimate * (target.card.DefNow - defineValue) / defineValue;
            }

            if (target.id.owner.Equals(OwnerEnum.Player) && !onlyFriend)
            {
                return (int)((-estimate) * 0.1 * (target.card.cost + target.card.AtkNow) *
                             ((target.card.DefNow - defineValue - 1) /
                              (Math.Abs(target.card.DefNow - defineValue - 1) + 0.001)));
            }

            if (target.id.owner.Equals(OwnerEnum.Player))
            {
                return (int)(-estimate * 0.1f * (target.card.cost + target.card.AtkNow) *
                    (target.card.DefNow - defineValue - 1) / (Math.Abs(target.card.DefNow - defineValue - 1) + 0.001f));
            }

            return 0;
        }

        public static int CalculateDefineAtk(this (ID id, Card card) target, int estimate, bool onlyFoe,
            int defineValue, int defineTolerance)
        {
            if (target.id.owner.Equals(OwnerEnum.Opponent) && !onlyFoe)
            {
                if (target.card.passiveSkills.Adrenaline)
                {
                    return 0;
                }

                if (target.card.skill is "mitosis")
                {
                    return 0;
                }

                if (target.card.skill is "singularity" && target.card.passiveSkills.Antimatter)
                {
                    return 0;
                }

                return estimate * (defineTolerance - Math.Abs(defineValue - target.card.AtkNow)) / 10;
            }

            if (target.id.owner.Equals(OwnerEnum.Player))
            {
                return -estimate * (defineTolerance - Math.Abs(defineValue - target.card.AtkNow)) / 10;
            }

            return 0;
        }

        public static int CalculateFractalScore(this (ID id, Card card) target, int estimate)
        {
            if (target.id.owner.Equals(OwnerEnum.Opponent))
            {
                if (estimate > 0)
                {
                    return (int)((DuelManager.Instance.enemy.GetAllQuantaOfElement(target.card.costElement) + 1) /
                                 (target.card.cost + 0.2f) /
                                 (DuelManager.Instance.enemy.playerHand.GetHandCount() *
                                     DuelManager.Instance.enemy.playerHand.GetHandCount() * 5 + 1));
                }

                if (target.card.innateSkills.Obsession)
                {
                    return 1;
                }

                return (int)((target.card.cost /
                              (DuelManager.Instance.player.GetAllQuantaOfElement(target.card.costElement) + 1) +
                              0.2f) /
                             (DuelManager.Instance.player.playerHand.GetHandCount() *
                                 DuelManager.Instance.player.playerHand.GetHandCount() * 5 + 1));
            }

            if (target.id.owner.Equals(OwnerEnum.Player))
            {
                if (estimate > 0)
                {
                    return (int)((DuelManager.Instance.enemy.GetAllQuantaOfElement(target.card.costElement) + 1) /
                                 (target.card.cost + 0.2f) /
                                 (DuelManager.Instance.enemy.playerHand.GetHandCount() *
                                     DuelManager.Instance.enemy.playerHand.GetHandCount() * 5 + 1));
                }

                return (int)((target.card.cost /
                              (DuelManager.Instance.player.GetAllQuantaOfElement(target.card.costElement) + 1) +
                              0.2f) /
                             (DuelManager.Instance.player.playerHand.GetHandCount() *
                                 DuelManager.Instance.player.playerHand.GetHandCount() * 5 + 1));
            }

            return 0;
        }


        public static int CalculatePermanentScore(this (ID id, Card card) target, int estimate, string skill)
        {
            if (target.id.owner.Equals(OwnerEnum.Opponent))
            {
                return (target.card.cost + 1) * estimate / 25 - DuelManager.Instance.enemy.playerHand.GetHandCount();
            }

            if (target.id.owner.Equals(OwnerEnum.Player))
            {
                if (target.card.skill == skill) return 0;
                if (target.card.skill == "sundial" && skill != "steal")
                {
                    return DuelManager.Instance.enemy.GetPossibleDamage() * -estimate / 20;
                }

                return (target.card.cost + 1) * -estimate / 20;
            }

            return 0;
        }

        public static int CalculateWeaponScore(this (ID id, Card card) target)
        {
            var skillScore = 0;
            if (target.card.skillCost > 0)
            {
                skillScore =
                    DuelManager.Instance.GetIDOwner(target.id).GetAllQuantaOfElement(target.card.skillElement) /
                    (1 + target.card.skillCost);
            }
            else
            {
                skillScore = 1;
            }

            if (target.id.field.Equals(FieldEnum.Passive))
            {
                return target.card.atk + skillScore;
            }

            if (target.id.field.Equals(FieldEnum.Creature) &&
                CardDatabase.Instance.WeaponIdList.Contains(target.card.iD))
            {
                return target.card.atk + skillScore;
            }

            return 0;
        }

        public static int CalculateTearsScore(this (ID id, Card card) target, int estimate)
        {
            return estimate + Random.Range(0, 10) / 2;
        }

        public static int CalculateBetaCreatureScore(this (ID id, Card card) target, int estimate, bool onlyFriend,
            bool onlyFoe)
        {
            var skillScore = target.card.skill != "" ? 3 : 0;

            if (target.id.owner.Equals(OwnerEnum.Opponent) && !onlyFoe)
            {
                return (7 * estimate - (target.card.DefNow + target.card.AtkNow + target.card.cost + skillScore)) / 5;
            }

            if (target.id.owner.Equals(OwnerEnum.Player) && !onlyFriend)
            {
                return (-8 + (target.card.DefNow + target.card.AtkNow + skillScore)) / 100;
            }

            return 0;
        }


        public static int CalculateAlphaCreatureScore(this (ID id, Card card) target, bool onlyFriend, bool onlyFoe)
        {
            var skillScore = target.card.skill != "" ? 3 : 0;
            if (target.id.owner.Equals(OwnerEnum.Opponent) && !onlyFoe)
            {
                if (target.card.skill is "devour" or "dejavu" || target.card.DefNow <= 0)
                {
                    skillScore = 10;
                }

                if (target.card.skill is "hatch")
                {
                    return 0;
                }

                if (target.card.passiveSkills.Momentum && BattleVars.Shared.AbilityCardOrigin.skill is "momentum")
                {
                    return 0;
                }

                if (target.card.innateSkills.Chimera && BattleVars.Shared.AbilityCardOrigin.skill is "paralleluniverse")
                {
                    return 0;
                }

                return target.card.AtkNow + skillScore - target.card.Freeze - target.card.innateSkills.Delay / 10;
            }

            if (target.id.owner.Equals(OwnerEnum.Player) && !onlyFriend)
            {
                if (BattleVars.Shared.AbilityCardOrigin.skill is "parallel universe" &&
                    target.card.innateSkills.Chimera)
                {
                    return DuelManager.Instance.player.playerCreatureField.GetAllValidCards().Count / 5;
                }

                return target.card.AtkNow + skillScore - target.card.Freeze - target.card.innateSkills.Delay / 10;
            }

            return 0;
        }

        public static int CalculateCreatureAndPlayerScore(this (ID id, Card card) target, int estimate, bool onlyFriend, bool onlyFoe)
        {
            if (target.id.owner.Equals(OwnerEnum.Opponent) && target.card is null)
            {
                return (int)(estimate * (DuelManager.Instance.enemy.HealthManager.GetMaxHealth() -
                                         DuelManager.Instance.enemy.HealthManager.GetCurrentHealth()) * 0.001f);
            }
            if (target.id.owner.Equals(OwnerEnum.Player) && target.card is null)
            {
                var score = -estimate / (DuelManager.Instance.enemy.HealthManager.GetCurrentHealth() * 4);
                if (DuelManager.Instance.player.playerPassiveManager.GetShield().Item2.skill is "reflect")
                {
                    return 0;
                }

                if (DuelManager.Instance.player.sacrificeCount > 0)
                {
                    return -score * 50;
                }
                
                if (DuelManager.Instance.enemy.sacrificeCount > 0)
                {
                    return -score;
                }

                return score;
            }

            return target.CalculateCreatureScore(estimate);
        }

        public static int CalculateCreatureScore(this (ID id, Card card) target, int estimate)
        {
            var score = 0;
            if (target.id.owner.Equals(OwnerEnum.Opponent))
            {
                if(target.card.DefNow < target.card.def)
                {
                    score = (int)((0.01f * target.card.AtkNow * target.card.def - target.card.DefNow) * estimate);
                }
                if (target.card.AtkNow < 0)
                {
                    score = (int)(0.1f * target.card.AtkNow * estimate);
                }
                if (target.card.Poison < 0 && BattleVars.Shared.AbilityCardOrigin.skill is "purify")
                {
                    score = 0;
                }
                if (BattleVars.Shared.AbilityCardOrigin.skill is "holy light" && (target.card.costElement is Element.Death or Element.Darkness))
                {
                    score = -estimate;
                }
                if (BattleVars.Shared.AbilityCardOrigin.skill is "reverse time" && (target.card.innateSkills.Mummy || target.card.innateSkills.Undead))
                {
                    score = 1;
                }

                return score;
            }

            if (target.id.owner.Equals(OwnerEnum.Player))
            {
                var skillScore = target.card.skill != "" ? 3 : 0;
                score = (target.card.AtkNow + 1 + skillScore) / -estimate / 20;
                if (target.card.Freeze > 0 || target.card.innateSkills.Delay > 0)
                {
                    score = 0;
                }
                if (BattleVars.Shared.AbilityCardOrigin.skill is "holy light" && (target.card.costElement is Element.Death or Element.Darkness))
                {
                    score = (target.card.AtkNow + 1 + skillScore) / estimate;
                }
                if (BattleVars.Shared.AbilityCardOrigin.skill is "reverse time" && (target.card.innateSkills.Mummy || target.card.innateSkills.Undead))
                {
                    score = 0;
                }
                if (BattleVars.Shared.AbilityCardOrigin.skill is "reverse time" && target.card.innateSkills.Voodoo && target.card.DefNow > 25)
                {
                    score = target.card.DefNow / 25;
                }
                if (BattleVars.Shared.AbilityCardOrigin.skill is "shockwave" && target.card.Freeze > 0)
                {
                    score = 1;
                }
                if (BattleVars.Shared.AbilityCardOrigin.skill is "web" && target.card.innateSkills.Airborne)
                {
                    score = 0;
                }

                return score;
            }

            return score;
        }
    }
}