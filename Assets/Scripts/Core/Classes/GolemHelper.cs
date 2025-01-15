using System.Collections.Generic;
using System.Linq;
using Battlefield.Abilities;
using Battlefield.Abilities.Weapon;

namespace Core.Classes
{
    public static class GolemHelper
    {
        private static Dictionary<Element, int> GetShardDict()
        {
            return new Dictionary<Element, int>
            {
                { Element.Aether, 0 },
                { Element.Air, 0 },
                { Element.Darkness, 0 },
                { Element.Death, 0 },
                { Element.Earth, 0 },
                { Element.Entropy, 0 },
                { Element.Fire, 0 },
                { Element.Gravity, 0 },
                { Element.Life, 0 },
                { Element.Light, 0 },
                { Element.Time, 0 },
                { Element.Water, 0 }
            };
        }

        private static Dictionary<Element, int> SetValue(this Dictionary<Element, int> shardDict,
            ref (int atk, int def) stats, List<(ID id, Card card)> shardList)
        {
            foreach (var item in shardList)
            {
                shardDict[item.card.CostElement]++;
                switch (item.card.CostElement)
                {
                    case Element.Earth:
                        stats.atk += item.card.Id.IsUpgraded() ? 2 : 1;
                        stats.def += item.card.Id.IsUpgraded() ? 5 : 4;
                        break;
                    case Element.Fire:
                        stats.atk += item.card.Id.IsUpgraded() ? 4 : 3;
                        stats.def += item.card.Id.IsUpgraded() ? 1 : 0;
                        break;
                    case Element.Gravity:
                        stats.atk += item.card.Id.IsUpgraded() ? 1 : 0;
                        stats.def += item.card.Id.IsUpgraded() ? 7 : 6;
                        break;
                    default:
                        stats.atk += item.card.Id.IsUpgraded() ? 3 : 2;
                        stats.def += item.card.Id.IsUpgraded() ? 3 : 2;
                        break;
                }

                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(item.id));
            }

            return shardDict;
        }
        
        public static Card GetGolemAbility(Card golem, List<(ID id, Card card)> shardList)
        {
            (int atk, int def) golemStats = (0,0);
            var elementCount = GetShardDict().SetValue(ref golemStats, shardList);
              
            golem.AtkModify = golemStats.atk;
            golem.DefModify = golemStats.def;
            if (elementCount[Element.Air] > 0)
            {
                golem.innateSkills.Airborne = true;
            }

            if (elementCount[Element.Darkness] > 0)
            {
                golem.PreAttackAbility = new DevourerEndTurn();
            }

            if (elementCount[Element.Darkness] > 1)
            {
                golem.innateSkills.Voodoo = true;
            }

            if (elementCount[Element.Gravity] > 1)
            {
                golem.passiveSkills.Momentum = true;
            }

            if (elementCount[Element.Life] > 1)
            {
                golem.passiveSkills.Adrenaline = true;
            }

            if (elementCount[Element.Aether] > 1)
            {
                golem.innateSkills.Immaterial = true;
            }

            var maxValueKey = elementCount.Aggregate((x, y) => x.Value >= y.Value ? x : y).Key;

            golem.SkillElement = Element.Earth;
            switch (maxValueKey)
            {
                case Element.Aether:
                    switch (elementCount[maxValueKey])
                    {
                        case 3:
                        case 4:
                        case 5:
                            golem.Skill = new Lobotomize();
                            golem.SkillCost = 2;
                            golem.Desc = "<sprite=0><sprite=0> : Remove any skill from the target creature.";
                            break;
                        case 6:
                        case 7:
                            golem.innateSkills.Immaterial = true;
                            golem.Desc = "Immaterial: \n Golem can not be targeted.";
                            break;
                    }

                    break;
                case Element.Air:
                    switch (elementCount[maxValueKey])
                    {
                        case 2:
                            golem.Skill = new Queen();
                            golem.SkillCost = 2;
                            golem.Desc = "<sprite=1><sprite=1> : Firefly\nGenerate a firefly.";
                            break;
                        case 3:
                            golem.Skill = new Sniper();
                            golem.Desc = "<sprite=1><sprite=1> : Sniper\nDeal 3 damage to the target creature.";
                            golem.SkillCost = 2;
                            break;
                        case 4:
                        case 5:
                            golem.Skill = new Dive();
                            golem.Desc = "<sprite=1><sprite=1> : Dive\nThe damage dealt is doubled for 1 turn.";
                            golem.SkillCost = 2;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Unstablegas();
                            golem.Desc = "<sprite=1><sprite=1> : Unstable gas\n Generate unstable gas";
                            golem.SkillCost = 2;
                            break;
                    }

                    break;
                case Element.Darkness:
                    switch (elementCount[maxValueKey])
                    {
                        case 3:
                        case 4:
                            golem.passiveSkills.Vampire = true;
                            break;
                        case 5:
                            golem.Skill = new Liquidshadow();
                            golem.Desc =
                                "<sprite=2><sprite=2> : : Liquid Shadow\nThe target creature is poisoned and its skill switched to \"vampire\".";
                            golem.SkillCost = 2;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Steal();
                            golem.Desc = "<sprite=2><sprite=2><sprite=2> : : Steal\nSteal a permanent";
                            golem.SkillCost = 3;
                            break;
                    }

                    break;
                case Element.Light:
                    switch (elementCount[maxValueKey])
                    {
                        case 1:
                        case 2:
                            golem.Skill = new Heal();
                            golem.Desc = "<sprite=3> : Heal\nHeal the target creature up to 5 HP's";
                            golem.SkillCost = 1;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            golem.Skill = new Endow();
                            golem.Desc =
                                "<sprite=3><sprite=3> : Endow\nGain the target weapon's ability and +X|+2. X is the weapon's attack.";
                            golem.SkillCost = 2;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Luciferin();
                            golem.Desc =
                                "<sprite=3><sprite=3><sprite=3><sprite=3> : Luciferin\nAll your creatures without a skill gain \"bioluminescence\". Heal yourself for up to 10 HP";
                            golem.SkillCost = 4;
                            break;
                    }

                    break;
                case Element.Death:
                    switch (elementCount[maxValueKey])
                    {
                        case 1:
                            golem.Skill = new Infection();
                            golem.Desc = "<sprite=4> : Infection\nInflict 1 damage per turn to a target creature.";
                            golem.SkillCost = 1;
                            break;
                        case 2:
                        case 3:
                            golem.DeathTriggerAbility = new ScavengerDeathTrigger();
                            golem.Desc = "Scavenger:\nEvery time a creature dies, Shard Golem gains +1/+1";
                            break;
                        case 4:
                            golem.passiveSkills.Venom = true;
                            golem.Desc = "Deal 1 poison damage at the end of every turn.\nPoison damage is cumulative.";
                            break;
                        case 5:
                            golem.Skill = new Aflatoxin();
                            golem.Desc =
                                "<sprite=4><sprite=4> : Poison the target creature. If the target creature dies, it turns into a malignant cell.";
                            golem.SkillCost = 2;
                            break;
                        case 6:
                        case 7:
                            golem.passiveSkills.DeadlyVenom = true;
                            golem.Desc =
                                "Deadly Venom: \nAdd 2 poison damage to each successful attack. Cause poisoning if ingested.";
                            break;
                    }

                    break;
                case Element.Earth:
                    switch (elementCount[maxValueKey])
                    {
                        case 1:
                            golem.Skill = new Burrow();
                            golem.Desc =
                                "<sprite=5> : Burrow\nThe Shard Golem can not be targeted, but its damage is halved.";
                            golem.SkillCost = 1;
                            break;
                        case 2:
                        case 3:
                            golem.Skill = new Stoneform();
                            golem.Desc = "<sprite=5> : Stone form\nShard Golem gains +0 / +20";
                            golem.SkillCost = 1;
                            break;
                        case 4:
                        case 5:
                            golem.Skill = new Guard();
                            golem.Desc =
                                "<sprite=5> : Guard\n(Do not attack) Delay the target creature for 1 turn (cumulative) and attack it unless it is airborne.";
                            golem.SkillCost = 1;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Petrify();
                            golem.Desc =
                                "<sprite=5><sprite=5> : Petrify\nThe target creature gains +0/+20 but can not attack or use skills for 6 turns.";
                            golem.SkillCost = 2;
                            break;
                    }

                    break;
                case Element.Entropy:
                    switch (elementCount[maxValueKey])
                    {
                        case 1:
                            golem.Skill = new Deadalive();
                            golem.Desc =
                                "<sprite=6> : Dead and Alive\nKill this creature; death effects are triggered. This creature is still alive.";
                            golem.SkillCost = 1;
                            break;
                        case 2:
                            golem.Skill = new Mutation();
                            golem.Desc =
                                "<sprite=6><sprite=6> : Mutation\nThe target creature might turn into an abomination, a mutant, or die.";
                            golem.SkillCost = 2;
                            break;
                        case 3:
                            golem.Skill = new Paradox();
                            golem.Desc =
                                "<sprite=6><sprite=6> : Paradox\nKill the target creature if its attack is higher than its defence";
                            golem.SkillCost = 2;
                            break;
                        case 4:
                            golem.Skill = new Improve();
                            golem.Desc =
                                "<sprite=6><sprite=6> : Improved Mutation\nThe target creature might turn into an abomination, a mutant, or die";
                            golem.SkillCost = 2;
                            break;
                        case 5:
                            golem.WeaponPassive = new ScrambleSkill();
                            golem.Desc = "Randomly convert some of the opponent's quantums into other elements.";
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Antimatter();
                            golem.Desc =
                                "<sprite=6><sprite=6><sprite=6><sprite=6> : Antimatter\nInvert the attack power of the target creature (the creature inflict heals instead of damage)";
                            golem.SkillCost = 4;
                            break;
                    }

                    break;
                case Element.Time:
                    switch (elementCount[maxValueKey])
                    {
                        case 2:
                            golem.Skill = new Scarab();
                            golem.Desc = "<sprite=7><sprite=7> : Scarab\nGenerate a Scarab.";
                            golem.SkillCost = 2;
                            break;
                        case 3:
                            golem.Skill = new Dejavu();
                            golem.Desc =
                                "<sprite=7><sprite=7><sprite=7><sprite=7> : Deja Vu\nShard Golem creates a copy of itself";
                            golem.SkillCost = 4;
                            break;
                        case 4:
                        case 5:
                            golem.passiveSkills.Neurotoxin = true;
                            golem.Desc =
                                "Neurotoxin: Add 1 poison damage to each successful attack, 1 extra poison for each card played by afflicted player.";
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Precognition();
                            golem.Desc =
                                "<sprite=7><sprite=7> : Precognition\nYou can see your opponent's hand. Draw a card.";
                            golem.SkillCost = 2;
                            break;
                    }

                    break;
                case Element.Fire:
                    switch (elementCount[maxValueKey])
                    {
                        case 2:
                            golem.Skill = new Ablaze();
                            golem.Desc = "<sprite=8> : Ablaze\nShard Golem gains +2/+0";
                            golem.SkillCost = 1;
                            break;
                        case 3:
                        case 4:
                            golem.WeaponPassive = new FierySkill();
                            golem.Desc =
                                "Deal X damages at the end of every turn. X is the number of <sprite=8> you own, divided by 5.";
                            break;
                        case 5:
                            golem.Skill = new Destroy();
                            golem.Desc = "<sprite=8><sprite=8><sprite=8>: Destroy\nShatter the target permanent.";
                            golem.SkillCost = 3;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Rage();
                            golem.Desc = "<sprite=8><sprite=8> : Rage\nThe target creature gains +5/-5";
                            golem.SkillCost = 2;
                            break;
                    }

                    break;
                case Element.Gravity:
                    switch (elementCount[maxValueKey])
                    {
                        case 5:
                            golem.Skill = new Devour();
                            golem.Desc =
                                "<sprite=9><sprite=9><sprite=9> : Devour\nSwallow a smaller (less HP's) creature and gain +1/+1";
                            golem.SkillCost = 3;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Blackhole();
                            golem.Desc =
                                "<sprite=9><sprite=9><sprite=9><sprite=9> : Black Hole\nAbsorb 3 quanta per element from the opponent. Gain 1 HP per absorbed quantum.";
                            golem.SkillCost = 4;
                            break;
                    }

                    break;
                case Element.Life:
                    switch (elementCount[maxValueKey])
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            golem.Skill = new Growth();
                            golem.Desc = "<sprite=10><sprite=10> : Growth\nThe Shard Golem gains +2/+2";
                            golem.SkillCost = 2;
                            break;
                        case 5:
                            golem.Skill = new Adrenaline();
                            golem.Desc =
                                "<sprite=10><sprite=10> : Adrenaline\nThe target creature attacks multiple times per turn.";
                            golem.SkillCost = 2;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Mitosiss();
                            golem.Desc =
                                "<sprite=10><sprite=10><sprite=10><sprite=10> Mitosis: \n Generate a daughter creature";
                            golem.SkillCost = 4;
                            break;
                    }

                    break;
                case Element.Water:
                    switch (elementCount[maxValueKey])
                    {
                        case 2:
                        case 3:
                            golem.Skill = new Steam();
                            golem.Desc =
                                "<sprite=11><sprite=11> : Steam\nGain 5 charges (+5|+0). Remove 1 charge per turn.";
                            golem.SkillCost = 2;
                            break;
                        case 4:
                        case 5:
                            golem.Skill = new Freeze();
                            golem.Desc = "<sprite=11><sprite=11><sprite=11> : Freeze\nFreeze the target creature";
                            golem.SkillCost = 3;
                            break;
                        case 6:
                        case 7:
                            golem.Skill = new Nymph();
                            golem.Desc =
                                "<sprite=10><sprite=10><sprite=10><sprite=10> : Nymph's tears\nTurn one of your pillars into a Nymph";
                            golem.SkillCost = 4;
                            break;
                    }

                    break;
            }

            return golem;
        }
    }
}