using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class CardDatabase : MonoBehaviour
{
    private static List<string> mutantActiveAList = new List<string>
        {
            "Burrow",
            "Paradox",
            "Lycanthropy",
            "Infection",
            "Devour",
            "Ablaze",
            "Growth",
            "Heal",
            "Steal",
            "Freeze",
            "PoisonPlayer",
            "Mutation",
            "Hatch",
            "Destroy",
            "Dive",
            "DejaVu",
            "GravityPull",
            "Endow",
            "Mitosis",
            "Aflatoxin",
            "Immaterial",
            "Momentum",
            "Scavenger"
        };

    public static List<Card> GetAllCards()
    {
        List<Card> list = new List<Card>();
        List<Card> listToReturn = new List<Card>();

        list.AddRange(Resources.LoadAll<Card>(baseSOpath + "/Spells/Regular/"));
        list.AddRange(Resources.LoadAll<Card>(baseSOpath + "/Creatures/Regular/"));
        list.AddRange(Resources.LoadAll<Card>(baseSOpath + "/Weapons/Regular/"));
        list.AddRange(Resources.LoadAll<Card>(baseSOpath + "/Shields/Regular/"));
        list.AddRange(Resources.LoadAll<Card>(baseSOpath + "/Artifacts/Regular/"));
        list.AddRange(Resources.LoadAll<Card>(baseSOpath + "/Pillars/Regular/"));
        foreach (Card card in list)
        {
            if(card.isRare) { continue; }
            listToReturn.Add(card);
        }
        return listToReturn;
    }

    public static List<Card> GetAllSpells()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Spells/"));
        return list;

    }
    public static List<Card> GetAllCreatures()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Creatures/"));
        return list;
    }
    public static List<Card> GetAllWeapons()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Weapons/"));
        return list;
    }
    public static List<Card> GetAllShields()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Shields/"));
        return list;
    }
    public static List<Card> GetAllArtifacts()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Artifacts/"));
        return list;
    }

    private static List<string> mutantActiveADescList = new List<string>
        {
            "The Mutant can not be targeted, but its damage is halved.",
            "Kill the target creature if its attack is higher than its defense",
            "The Mutant gains +5/+5 permanently.",
            "Inflict 1 damage per turn to a target creature",
            "Swallow a smaller (less HP's) creature and gain +1/+1",
            "The Mutant gains +2/+0",
            "The Mutant gains +2/+2",
            "Heal the target creature up to 5 HP's",
            "Steal a permanent",
            "Freeze the target creature for 3 turns. Frozen creatures can not attack or use skills.",
            "Inflicts 2 poison damage (to your opponent) at the end of every turn. Poison damage is cumulative.",
            "Mutate the target creature into an abomination, unless it dies... or turn into something weird.",
            "The Mutant turns into a random creature",
            "Destroy the targeted permanent",
            "The damage dealt is doubled for 1 turn.",
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
        if (string.IsNullOrWhiteSpace(text))
        {
            return "";
        }
        StringBuilder stringBuilder = new StringBuilder(text.Length * 2);
        stringBuilder.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
            {
                stringBuilder.Append(' ');
            }
            stringBuilder.Append(text[i]);
        }
        return stringBuilder.ToString();
    }


    public static Card GetCardFromResources(string name, string type, bool isUpgraded)
    {
        string upgradedFolder = isUpgraded ? "Upgraded" : "Regular";
        Card cardToReturn = Instantiate(Resources.Load<Card>($"{baseSOpath}/{type}s/{upgradedFolder}/{name}"));
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }
    public static Card GetCardFromResources(CardObject cardObject)
    {
        string upgradedFolder = cardObject.isUpgraded ? "Upgraded" : "Regular";
        Card cardToReturn = Instantiate(Resources.Load<Card>(@$"{baseSOpath}/{cardObject.cardType}s/{upgradedFolder}/{cardObject.cardName}"));
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }

    private static string baseSOpath = @"Cards";
    internal static Card GetRandomSpell()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Spells/Regular/"));
        Card card = list[Random.Range(0, list.Count)];

        Card cardToReturn = Instantiate(card);
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }
    public static Card GetRandomEliteSpell()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Spells/Upgraded/"));
        Card card = list[Random.Range(0, list.Count)];

        Card cardToReturn = Instantiate(card);
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }

    public static Card GetRandomEliteCreature()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Creatures/Upgraded/"));
        Card card = list[Random.Range(0, list.Count)];

        Card cardToReturn = Instantiate(card);
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }

    public static Card GetRandomCreature()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Creatures/Regular/"));
        Card card = list[Random.Range(0, list.Count)];

        Card cardToReturn = Instantiate(card);
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }

    public static Card GetRandomPillar()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Pillars/Regular/"));
        Card card = list[Random.Range(0, list.Count)];

        Card cardToReturn = Instantiate(card);
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }

    public static Card GetRandomTower()
    {
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + "/Pillars/Upgraded/"));
        Card card = list[Random.Range(0, list.Count)];

        Card cardToReturn = Instantiate(card);
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }

    public static List<Card> GetRandomDeck()
    {
        List<Card> deckToReturn = new List<Card>();
        for (int i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomPillar());
        }

        for (int i = 0; i < 20; i++)
        {
            deckToReturn.Add(GetRandomCreature());
        }

        for (int i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomSpell());
        }

        return deckToReturn;
    }

    public static Card GetRandomCardOfTypeWithElement(CardType type, Element element, bool shouldBeUpgraded)
    {
        string upgradedFolder = shouldBeUpgraded ? "Upgraded" : "Regular";
        List<Card> list = new List<Card>(Resources.LoadAll<Card>(baseSOpath + $"/{type.FastCardTypeString()}s/{upgradedFolder}/"));
        Card card = list[Random.Range(0, list.Count)];
        

        while (!card.element.Equals(element))
        {
            card = list[Random.Range(0, list.Count)];
        }
        Card cardToReturn = Instantiate(card);
        cardToReturn.name = cardToReturn.name.Replace("(Clone)", "");
        return cardToReturn;
    }

    public static List<Card> GetHalfBloodDeck(Element primary, Element secondary)
    {
        int upgradedCount = 21;
        List<Card> deckToReturn = new List<Card>();

        bool shouldBeUpgraded = Random.Range(0, 100) < 30 && upgradedCount > 0;
        for (int i = 0; i < 4; i++)
        {
            deckToReturn.Add(GetCardFromResources("Quantum Pillar", CardType.Pillar.FastCardTypeString(), false));
        }

        for (int i = 0; i < 20; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Pillar, primary, shouldBeUpgraded));
            if (shouldBeUpgraded) { upgradedCount--; }

            shouldBeUpgraded = Random.Range(0, 100) < 30 && upgradedCount > 0;
        }

        for (int i = 0; i < 30; i++)
        {
            CardType typeToAdd = (CardType)Random.Range(1, 6);
            while(primary.Equals(Element.Earth) && typeToAdd.Equals(CardType.Artifact))
            {
                typeToAdd = (CardType)Random.Range(1, 6);
            }

            deckToReturn.Add(GetRandomCardOfTypeWithElement(typeToAdd, primary, shouldBeUpgraded));
            if (shouldBeUpgraded) { upgradedCount--; }

            shouldBeUpgraded = Random.Range(0, 100) < 30 && upgradedCount > 0;
        }

        for (int i = 0; i < 10; i++)
        {
            CardType typeToAdd = (CardType)Random.Range(1, 6);
            while (secondary.Equals(Element.Earth) && typeToAdd.Equals(CardType.Artifact))
            {
                typeToAdd = (CardType)Random.Range(1, 6);
            }


            deckToReturn.Add(GetRandomCardOfTypeWithElement(typeToAdd, secondary, shouldBeUpgraded));
            if (shouldBeUpgraded) { upgradedCount--; }

            shouldBeUpgraded = Random.Range(0, 100) < 30 && upgradedCount > 0;
        }

        return deckToReturn;
    }

    public static Card GetMutant()
    {
        Card card = GetRandomCreature();
        card.cardPassives = new CardPassives();
        card.cardAbilities = new CardAbilities();
        card.power += Random.Range(0, 4);
        card.hp += Random.Range(0, 4);
        card.maxHP = card.hp;
        card.cardPassives.isMutant = true;

        int index = Random.Range(0, mutantActiveAList.Count);
        string abilityName = mutantActiveAList[index];

        switch (abilityName)
        {
            case "Immaterial":
                card.cardPassives.isImmaterial = true;
                break;
            case "Momentum":
                card.cardPassives.hasMomentum = true;
                break;
            case "Scavenger":
                card.onDeathAbility = "OnDeathScavenger".GetScriptFromName<IOnDeathAbility>();
                card.onPlayAbility = "OnPlayAddDeathTrigger".GetScriptFromName<IOnPlayAbility>();
                break;
            default:
                card.activeAbility = $"ActiveA{abilityName}".GetScriptFromName<IActivateAbility>();
                break;
        }

        card.description = $"{AddSpacesToSentence(abilityName)} : \n {mutantActiveADescList[index]}";

        return card;
    }

    private static Dictionary<Element, string> regularNymphNames = new Dictionary<Element, string> {
        { Element.Gravity, "Amber Nymph" },
        {Element.Earth, "Auburn Nymph" },
        {Element.Darkness, "Black Nymph" },
        {Element.Air, "Blue Nymph" },
        {Element.Time, "Golden Nymph" },
        {Element.Life, "Green Nymph" },
        {Element.Death, "Grey Nymph"},
        {Element.Water, "Nymph Queen" },
        {Element.Entropy,  "Purple Nymph"},
        {Element.Fire, "Red Nymph" },
        {Element.Aether,  "Turquoise Nymph"},
        {Element.Light, "White Nymph"}
    };


    private static Dictionary<Element, string> eliteNymphNames = new Dictionary<Element, string> {
        { Element.Gravity, "Gravity Nymph" },
        {Element.Earth, "Earth Nymph" },
        {Element.Darkness, "Dark Nymph" },
        {Element.Air, "Air Nymph" },
        {Element.Time, "Elite Golden Nymph" },
        {Element.Life, "Life Nymph" },
        {Element.Death, "Death Nymph"},
        {Element.Water, "Water Nymph" },
        {Element.Entropy,  "Elite Purple Nymph"},
        {Element.Fire, "Fire Nymph" },
        {Element.Aether,  "Aether Nymph"},
        {Element.Light, "Light Nymph"}
    };

    public static Card GetRandomRegularNymph(Element element)
    {
        return GetCardFromResources(regularNymphNames[element], "Creature", false);
    }
public static Card GetRandomEliteNymph(Element element)
{
    return GetCardFromResources(regularNymphNames[element], "Creature", false);
}

public static Card GetGolemAbility(QuantaObject shardElementLast, Card golem)
    {
        switch (shardElementLast.element)
        {
            case Element.Aether:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.cardPassives.isImmaterial = true;
                        golem.description = "Immaterial: \n Golem can not be targeted.";
                        break;
                    case 2:
                        golem.cardPassives.isImmaterial = true;
                        golem.description = "Immaterial: \n Golem can not be targeted.";
                        break;
                    case 3:
                        golem.activeAbility = "ActiveALobotomizer".GetScriptFromName<IActivateAbility>();
                        golem.description = "<sprite=0><sprite=0> : Lobotomize \n Remove any skill from the target creature.";
                        break;
                    case 4:
                        golem.activeAbility = "ActiveALobotomizer".GetScriptFromName<IActivateAbility>();
                        golem.description = "<sprite=0><sprite=0> : Lobotomize \n Remove any skill from the target creature.";
                        break;
                    case 5:
                        golem.activeAbility = "ActiveALobotomizer".GetScriptFromName<IActivateAbility>();
                        golem.description = "<sprite=0><sprite=0> : Lobotomize \n Remove any skill from the target creature.";
                        break;
                    default:
                        golem.activeAbility = "ActiveAImmortality".GetScriptFromName<IActivateAbility>();
                        golem.description = "<sprite=0><sprite=0> : Immortality \n The target creature is now immortal (untargetable).";
                        break;
                }
                break;
            case Element.Air:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.cardPassives.isAirborne = true;
                        break;
                    case 2:
                        golem.activeAbility = "ActiveASpawnFirefly".GetScriptFromName<IActivateAbility>();
                        break;
                    case 3:
                        golem.activeAbility = "ActiveASniper".GetScriptFromName<IActivateAbility>();
                        break;
                    case 4:
                        golem.activeAbility = "ActiveADive".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.activeAbility = "ActiveASpawnUnstableGas".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Darkness:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.endTurnAbility = "EndTurnDevourer".GetScriptFromName<IEndTurnAbility>();
                        break;
                    case 2:
                        golem.cardPassives.isVoodoo = true;
                        break;
                    case 3:
                        golem.cardPassives.isVampire = true;
                        break;
                    case 4:
                        golem.cardPassives.isVampire = true;
                        break;
                    case 5:
                        golem.activeAbility = "ActiveALiquidShadow".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.activeAbility = "ActiveASteal".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Light:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveAHeal".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.activeAbility = "ActiveAEndow".GetScriptFromName<IActivateAbility>();
                        break;
                    case 3:
                        golem.activeAbility = "ActiveAEndow".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.activeAbility = "ActiveALuciferinFour".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Death:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveAInfection".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.cardAbilities.onDeathAbilityScript = "OnDeathScavenger";
                        golem.cardAbilities.onPlayAbilityScript = "OnPlayAddDeathTrigger";
                        break;
                    case 3:
                        golem.cardPassives.isVenemous = true;
                        break;
                    case 4:
                        golem.cardPassives.isVenemous = true;
                        break;
                    case 5:
                        golem.activeAbility = "ActiveAAflatoxin".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.cardPassives.isDeadlyVenemous = true;
                        break;
                }
                break;
            case Element.Earth:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveABurrow".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.activeAbility = "ActiveAStoneForm".GetScriptFromName<IActivateAbility>();
                        break;
                    case 3:
                        golem.activeAbility = "ActiveAGuard".GetScriptFromName<IActivateAbility>();
                        break;
                    case 4:
                        golem.activeAbility = "ActiveAGuard".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.activeAbility = "ActiveAPetrify".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Entropy:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveADeadAndAlive".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.activeAbility = "ActiveAMutation".GetScriptFromName<IActivateAbility>();
                        break;
                    case 3:
                        golem.activeAbility = "ActiveAParadox".GetScriptFromName<IActivateAbility>();
                        break;
                    case 4:
                        golem.activeAbility = "ActiveAImprovedMutation".GetScriptFromName<IActivateAbility>();
                        break;
                    //case 5:
                    //    return "ActiveAScramble";
                    default:
                        golem.activeAbility = "ActiveAAntimatter".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Time:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveASpawnScarab".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.activeAbility = "ActiveASpawnScarab".GetScriptFromName<IActivateAbility>();
                        break;
                    case 3:
                        golem.activeAbility = "ActiveADejaVu".GetScriptFromName<IActivateAbility>();
                        break;
                    case 4:
                        golem.cardPassives.hasNuerotoxin = true;
                        break;
                    case 5:
                        golem.cardPassives.hasNuerotoxin = true;
                        break;
                    default:
                        golem.activeAbility = "ActiveAPrecognition".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Fire:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveAAblaze".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.activeAbility = "ActiveAAblaze".GetScriptFromName<IActivateAbility>();
                        break;
                    case 3:
                        golem.cardPassives.isFiery = true;
                        break;
                    case 4:
                        golem.cardPassives.isFiery = true;
                        break;
                    case 5:
                        golem.cardPassives.isFiery = true;
                        break;
                    default:
                        golem.activeAbility = "ActiveARageThree".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Gravity:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.cardPassives.hasMomentum = true;
                        break;
                    case 2:
                        golem.cardPassives.hasMomentum = true;
                        break;
                    case 3:
                        golem.activeAbility = "ActiveADevour".GetScriptFromName<IActivateAbility>();
                        break;
                    case 4:
                        golem.activeAbility = "ActiveADevour".GetScriptFromName<IActivateAbility>();
                        break;
                    case 5:
                        golem.activeAbility = "ActiveADevour".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.activeAbility = "ActiveABlackHole".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Life:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveAGrowth".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.cardPassives.hasAdrenaline = true;
                        break;
                    case 3:
                        golem.cardPassives.hasAdrenaline = true;
                        break;
                    case 4:
                        golem.cardPassives.hasAdrenaline = true;
                        break;
                    case 5:
                        golem.activeAbility = "ActiveAAdrenaline".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.activeAbility = "ActiveAMitosis".GetScriptFromName<IActivateAbility>();
                        golem.description = "Mitosis: \n Generate a daughter creature";
                        break;
                }
                break;
            case Element.Water:
                switch (shardElementLast.count)
                {
                    case 1:
                        golem.activeAbility = "ActiveASteam".GetScriptFromName<IActivateAbility>();
                        break;
                    case 2:
                        golem.activeAbility = "ActiveASteam".GetScriptFromName<IActivateAbility>();
                        break;
                    case 3:
                        golem.activeAbility = "ActiveASteam".GetScriptFromName<IActivateAbility>();
                        break;
                    case 4:
                        golem.activeAbility = "ActiveAFreeze".GetScriptFromName<IActivateAbility>();
                        break;
                    case 5:
                        golem.activeAbility = "ActiveAFreeze".GetScriptFromName<IActivateAbility>();
                        break;
                    default:
                        golem.activeAbility = "ActiveANymphTear".GetScriptFromName<IActivateAbility>();
                        break;
                }
                break;
            case Element.Other:
                break;
            default:
                break;
        }
        return golem;
    }
}

