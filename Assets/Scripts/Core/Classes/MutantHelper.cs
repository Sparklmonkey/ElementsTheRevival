using System.Collections.Generic;
using System.Globalization;
using Battlefield.Abilities;
using UnityEngine;

namespace Core.Classes
{
    public static class MutantHelper
    {
        public static Card GetMutant(Card fromCard)
        {
            fromCard.Atk += Random.Range(0, 4);
            fromCard.Def += Random.Range(0, 4);
            fromCard.passiveSkills.Mutant = true;
            fromCard.SkillCost = Random.Range(1, 3);
            fromCard.SkillElement = fromCard.CostElement;
            var index = Random.Range(0, mutantActiveAList.Count);
            var abilityName = mutantActiveAList[index];

            switch (abilityName)
            {
                case "immaterial":
                    fromCard.SkillCost = 0;
                    fromCard.innateSkills.Immaterial = true;
                    break;
                case "momentum":
                case "scavenger":
                    fromCard.SkillCost = 0;
                    fromCard.DeathTriggerAbility = new ScavengerDeathTrigger();
                    break;
            }

            var skillCost = fromCard.SkillCost switch
            {
                0 => "",
                1 => $"<sprite={(int)fromCard.CostElement}>",
                _ => $"<sprite={(int)fromCard.CostElement}><sprite={(int)fromCard.CostElement}>"
            };
            fromCard.Desc = $"{AddSpacesToSentence(abilityName)} {skillCost} : \n {mutantActiveADescList[index]}";

            return fromCard;
        }

        private static readonly List<string> mutantActiveADescList = new()
        {
            "The Mutant turns into a random creature",
            "Destroy the targeted permanent",
            "Freeze the target creature for 3 turns. Frozen creatures can not attack or use skills.",
            "Steal a permanent",
            "The damage dealt is doubled for 1 turn.",
            "Heal the target creature up to 5 HP's",
            "The Mutant gains +2/+0",
            "Kill the target creature if its attack is higher than its defense",
            "Inflict 1 damage per turn to a target creature",
            "The Mutant gains +5/+5 permanently.",
            "Inflicts 2 poison damage (to your opponent) at the end of every turn. Poison damage is cumulative.",
            "Swallow a smaller (less HP's) creature and gain +1/+1",
            "The Mutant gains +2/+2",
            "Mutate the target creature into an abomination, unless it dies... or turn into something weird.",
            "Mutant creates a copy of itself",
            "The creature enchanted with gravity pull will absorb all the damage directed against its owner.",
            "Gain the target weapon's ability and +X|+2. X is the weapon's attack.",
            "Generate a daughter creature.",
            "Poison the target creature. If the target creature dies, it turns into a malignant cell.",
            "The Mutant can not be targeted.",
            "The Mutant ignores shield effects",
            "Every time a creature dies, Mutant gains +1/+1"
        };

        private static string AddSpacesToSentence(string text)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(text);
        }

        private static readonly List<string> mutantActiveAList = new()
        {
            "hatch",
            "destroy",
            "freeze",
            "steal",
            "dive",
            "heal",
            "ablaze",
            "paradox",
            "infection",
            "lycanthropy",
            "poison",
            "devour",
            "growth",
            "mutation",
            "deja vu",
            "gravity pull",
            "endow",
            "mitosis",
            "aflatoxin",
            "immaterial",
            "momentum",
            "scavenger"
        };
    }
}