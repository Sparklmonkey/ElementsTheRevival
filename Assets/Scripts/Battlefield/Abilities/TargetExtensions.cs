using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class TargetExtensions
{
    public static AiTargetType GetAITargetType(this string skillName)
    {
        var prge = DuelManager.Instance.enemy.HealthManager.GetCurrentHealth() /
                   (Math.Abs(DuelManager.Instance.GetPossibleDamage(true)) + 1);
        var aiTarget = new AiTargetType(false, false, false, TargetType.Creature, 1, 0, 0);
        switch (skillName)
        {
            case "reversetime" when DuelManager.Instance.player.DeckManager.GetDeckCount() < 5:
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 8;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "infect" or "infection" or "reversetime" or "sniper" or "aflatoxin":
                aiTarget.Targeting = TargetType.Creature;
                aiTarget.Estimate = -1;
                break;
            case "petrify" when DuelManager.Instance.GetCardCount(new List<string> { "74h", "561" }) > 0:
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "gravitypull" when prge < 5 && DuelManager.Instance.player.playerCreatureField.GetCreatureWithGravity()
                .Equals(default):
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 5;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "paradox":
                aiTarget.Targeting = TargetType.CreatureHighAtk;
                aiTarget.Estimate = -1;
                break;
            case "lightning" or "shockwave":
                aiTarget.Estimate = -5;
                aiTarget.Targeting = TargetType.CreatureAndPlayer;
                break;
            case "purify":
                aiTarget.Estimate = 2;
                aiTarget.Targeting = TargetType.CreatureAndPlayer;
                break;
            case "firebolt":
                aiTarget.Estimate = -3;
                aiTarget.Targeting = TargetType.CreatureAndPlayer;
                break;
            case "icebolt":
                aiTarget.Estimate = -2;
                aiTarget.Targeting = TargetType.CreatureAndPlayer;
                break;
            case "drainlife":
                aiTarget.Estimate = -2;
                aiTarget.Targeting = TargetType.CreatureAndPlayer;
                break;
            case "guard" or "freeze" or "congeal" or "petrify":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "paralleluniverse":
                aiTarget.Estimate = 0;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "holylight":
                aiTarget.Estimate = 10;
                aiTarget.Targeting = TargetType.CreatureAndPlayer;
                break;
            case "destroy" or "steal":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Permanent;
                break;
            case "accretion":
                aiTarget.Estimate = -10;
                aiTarget.Targeting = TargetType.Permanent;
                break;
            case "enchant":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Permanent;
                break;
            case "momentum":
                aiTarget.Estimate = 1;
                aiTarget.OnlyFriend = true;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "blessing" or "armor" or "heavyarmor" or "immortality" or "chaospower":
                aiTarget.Estimate = 3;
                aiTarget.OnlyFriend = true;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "chaos":
                aiTarget.Estimate = -1;
                aiTarget.OnlyFoe = true;
                aiTarget.Targeting = TargetType.AlphaCreature;
                break;
            case "adrenaline":
                aiTarget.Estimate = 1;
                aiTarget.OnlyFriend = true;
                aiTarget.DefineValue = 3;
                aiTarget.DefTolerance = 7;
                aiTarget.Targeting = TargetType.DefineAtk;
                break;
            case "antimatter":
                aiTarget.Estimate = -1;
                aiTarget.DefineValue = 25;
                aiTarget.DefTolerance = 25;
                aiTarget.Targeting = TargetType.DefineAtk;
                break;
            case "liquidshadow":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 3;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "mitosiss":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 25;
                aiTarget.DefTolerance = 25;
                aiTarget.Targeting = TargetType.DefineAtk;
                break;
            case "butterfly":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 3;
                aiTarget.Targeting = TargetType.CreatureLowAtk;
                break;
            case "readiness":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "catapult":
                aiTarget.Estimate = 20;
                aiTarget.Targeting = TargetType.Trebuchet;
                break;
            case "rage":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 5;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "berserk":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 6;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "acceleration" or "overdrive":
                aiTarget.Estimate = 1;
                aiTarget.DefineValue = 3;
                aiTarget.Targeting = TargetType.DefineDef;
                break;
            case "heal":
                aiTarget.Estimate = 5;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "devour":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Smaller;
                break;
            case "mutation" or "improve":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "immolate" or "cremation":
                aiTarget.OnlyFriend = true;
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.BetaCreature;
                break;
            case "lobotomize" or "liquidshadow" when Random.Range(0.0f, 1.0f) > 0.5f:
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.SkillCreature;
                break;
            case "tsunami" or "earthquake":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Pillar;
                break;
            case "nymph":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Tears;
                break;
            case "fractal":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Fractal;
                break;
            case "nightmare":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Fractal;
                break;
            case "web":
                aiTarget.Estimate = -1;
                aiTarget.Targeting = TargetType.Creature;
                break;
            case "endow":
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Weapon;
                break;
            case "wisdom":
            {
                aiTarget.Estimate = 1;
                aiTarget.Targeting = TargetType.Immortals;
                if (DuelManager.Instance.enemy.playerPassiveManager.GetShield().Item2.skill != "reflect")
                {
                    aiTarget.OnlyFriend = true;
                }

                if (DuelManager.Instance.player.playerPassiveManager.GetShield().Item2.skill == "reflect")
                {
                    aiTarget.OnlyFoe = true;
                }

                break;
            }
        }

        return aiTarget;
    }
}