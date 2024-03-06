using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Battlefield.Abilities;
using Battlefield.Abilities.Weapon;
using UnityEngine;


public class CardDatabase : SingletonMono<CardDatabase>
{
    private readonly AiDeckBuilder _deckBuilder = new();

    public List<string> RareWeaponRewards = new (){ "5ic", "5ol", "5ur", "5f7", "5lh", "4vl", "52q", "55s", "58v", "5c5", "5ro","61u"};

    public List<Card> FullCardList;

    public List<StarterDeck> StarterDecks;
    
    private readonly List<string> _illegalPets = new() { "4vr", "4t8", "4vf", "52h", "55o", "58r", "5bt", "5f2", "5id", "5la", "5of", "5rm", "5ul", "61v", "5lt", "7kd" };

    public Card GetRandomPet()
    {
        return FullCardList.Find(x => !x.Id.IsUpgraded() && !_illegalPets.Contains(x.Id) && x.Type.Equals(CardType.Creature));
    }

    public Card GetOracleCreature(Element element)
    {
        return FullCardList.Find(x => !x.Id.IsUpgraded()
                        && x.CostElement.Equals(element)
                        && !_illegalPets.Contains(x.Id)
                        && x.Type.Equals(CardType.Creature)
                        && !x.CardName.Contains("Shard of"));
    }

    public List<Card> TrainerCardList;

    public List<string> WeaponIdList = new(){ "52q", "4t3", "4vl", "5c5", "5ro", "5f7", "4t5", "61u", "5lh", "5ol", "58v", "4tb", "4t4", "55s", "5ic", "5ur", "6rj", "7n5", "80e",
        "71a", "6u5", "7q8", "7dn", "77f", "74c", "6rl", "7al", "6rr", "6rk", "7k1", "7gs", "7tb" };
    public Card GetShardOfElement(Element element)
    {
        return GetAllShards().Find(x => x.CostElement.Equals(element) && !x.Id.IsUpgraded());
    }

    private readonly List<string> _mutantActiveAList = new()
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

    public Card GetCardFromId(string id)
    {
        var baseCard = FullCardList.Find(x => x.Id == id);
        return baseCard.Clone();
    }

    internal List<Card> GetAllBazaarCards()
    {
        return new List<Card>(FullCardList.FindAll(x => !x.IsRare() && x.Id.IsBazaarLegal()));
    }

    internal List<Card> GetCardListWithID(List<string> cardRewards)
    {
        List<Card> cardObjects = new();
        foreach (var cardId in cardRewards)
        {
            cardObjects.Add(GetCardFromId(cardId));
        }
        return cardObjects;
    }

    public List<Card> GetAllShards()
    {
        List<Card> list = new(FullCardList.FindAll(x => x.CardName.Contains("Shard of")));
        return list;
    }

    public List<string> MarkIds = new() { "8pu", "8pr", "8pt", "8pq", "8pk", "8pm", "8pj", "8ps", "8po", "8pl", "8pn", "8pp" };
    private readonly List<string> _illegalHatchCards = new (){ "7qa", "7q2", "5ri", "61s", "80c", "6ro", "4t8", "6ub", "4vr", "74g", "560", "7dt", "5fd", "5rm", "7q6", "5lt", "7kd", "4t9", "6rp" };

    public Card GetRandomCard(CardType cardType, bool isUpgraded, bool shouldBeHatchLegal, Element element = Element.Aether, bool shouldBeElement = false)
    {
        List<Card> list = new(FullCardList.FindAll(x => x.Type.Equals(cardType) && x.Id.IsUpgraded() == isUpgraded && x.Id.IsDeckLegal()));
        if (shouldBeHatchLegal)
        {
            list = list.FindAll(x => !_illegalHatchCards.Contains(x.Id));
        }
        if (shouldBeElement)
        {
            list = list.FindAll(x => x.CostElement.Equals(element));
        }
        var card = list[Random.Range(0, list.Count)];
        return card.Clone();
    }

    public List<string> GetRandomDeck()
    {
        List<string> deckToReturn = new();
        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomCard(CardType.Pillar, false, true).Id);
        }

        for (var i = 0; i < 20; i++)
        {
            deckToReturn.Add(GetRandomCard(CardType.Creature, false, true).Id);
        }

        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomCard(CardType.Spell, false, true).Id);
        }

        return deckToReturn;
    }

    public Card GetRandomCardOfTypeWithElement(CardType type, Element element, bool shouldBeUpgraded)
    {
        var reducedList = FullCardList.FindAll(x => x.CostElement.Equals(element)
                                                    && x.Type.Equals(type)
                                                    && !x.CardName.Contains("Shard of")
                                                    && !x.CardName.Contains(" Nymph")
                                                    && x.Id.IsUpgraded() == shouldBeUpgraded);
        if (reducedList.Count > 0)
        {
            var card = reducedList[Random.Range(0, reducedList.Count)];
            var cardToReturn = card.Clone();
            return cardToReturn;
        }
        return GetRandomCardOfTypeWithElement((CardType)Random.Range(0, 6), element, shouldBeUpgraded);
    }

    public List<Card> GetHalfBloodDeck(Element primary, Element secondary) => _deckBuilder.GetHalfBloodDeck(primary, secondary);

    public Card GetMutant(bool isUpgraded, Card fromCard = null)
    {
        var card = fromCard == null ? GetRandomCard(CardType.Creature, isUpgraded, true) : fromCard;
        card.Atk += Random.Range(0, 4);
        card.Def += Random.Range(0, 4);
        card.passiveSkills.Mutant = true;
        card.SkillCost = Random.Range(1, 3);
        card.SkillElement = card.CostElement;
        var index = Random.Range(0, _mutantActiveAList.Count);
        var abilityName = _mutantActiveAList[index];

        switch (abilityName)
        {
            case "immaterial":
                card.SkillCost = 0;
                card.innateSkills.Immaterial = true;
                break;
            case "momentum":
            case "scavenger":
                card.SkillCost = 0;
                card.DeathTriggerAbility = new ScavengerDeathTrigger();
                break;
            default:
                // card.skill = abilityName;
                break;
        }
        var skillCost = card.SkillCost == 0 ? "" : card.SkillCost == 1 ? $"<sprite={(int)card.CostElement}>" : $"<sprite={(int)card.CostElement}><sprite={(int)card.CostElement}>";
        card.Desc = $"{AddSpacesToSentence(abilityName)} {skillCost} : \n {_mutantActiveADescList[index]}";

        return card;
    }

    private readonly List<string> _mutantActiveADescList = new()
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

    private string AddSpacesToSentence(string text)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(text);
    }

    private Dictionary<Element, string> _regularNymphNames = new(){
        { Element.Gravity, "568" },
        {Element.Earth, "59c" },
        {Element.Darkness, "5v8" },
        {Element.Air, "5p0" },
        {Element.Time, "5s4" },
        {Element.Life, "5cg" },
        {Element.Death, "534"},
        {Element.Water, "5io" },
        {Element.Entropy,  "500"},
        {Element.Fire, "5fk" },
        {Element.Aether,  "62c"},
        {Element.Light, "5ls"}
    };


    private Dictionary<Element, string> _eliteNymphNames = new(){
        { Element.Gravity, "74o" },
        {Element.Earth, "77s" },
        {Element.Darkness, "7to" },
        {Element.Air, "7ng" },
        {Element.Time, "7qk" },
        {Element.Life, "7b0" },
        {Element.Death, "71k"},
        {Element.Water, "7h8" },
        {Element.Entropy,  "6ug"},
        {Element.Fire, "7e4" },
        {Element.Aether,  "80s"},
        {Element.Light, "7kc"}
    };

    public Card GetRandomRegularNymph(Element element)
    {
        if (element.Equals(Element.Other))
        {
            return GetCardFromId(_regularNymphNames[(Element)Random.Range(0, 12)]);
        }
        return GetCardFromId(_regularNymphNames[element]);
    }
    public Card GetRandomEliteNymph(Element element)
    {
        if (element.Equals(Element.Other))
        {
            return GetCardFromId(_eliteNymphNames[(Element)Random.Range(0, 12)]);
        }
        return GetCardFromId(_eliteNymphNames[element]);
    }

    public Card GetGolemAbility(List<(ID id, Card card)> shardList)
    {
        var golem = GetCardFromId("597");
        Dictionary<Element, int> elementCount = new()
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
        foreach (var item in shardList)
        {
            elementCount[item.card.CostElement]++;
            switch (item.card.CostElement)
            {
                case Element.Earth:
                    golem.Atk += item.card.Id.IsUpgraded() ? 2 : 1;
                    golem.Def += item.card.Id.IsUpgraded() ? 5 : 4;
                    break;
                case Element.Fire:
                    golem.Atk += item.card.Id.IsUpgraded() ? 4 : 3;
                    golem.Def += item.card.Id.IsUpgraded() ? 1 : 0;
                    break;
                case Element.Gravity:
                    golem.Atk += item.card.Id.IsUpgraded() ? 1 : 0;
                    golem.Def += item.card.Id.IsUpgraded() ? 7 : 6;
                    break;
                default:
                    golem.Atk += item.card.Id.IsUpgraded() ? 3 : 2;
                    golem.Def += item.card.Id.IsUpgraded() ? 3 : 2;
                    break;
            }
            EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(item.id));
        }

        if (elementCount[Element.Air] > 0)
        {
            golem.innateSkills.Airborne = true;
        }

        if (elementCount[Element.Darkness] > 0)
        {
            golem.TurnEndAbility = new DevourerEndTurn();
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

        golem.SkillElement = maxValueKey;
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
                        golem.Desc = "<sprite=2><sprite=2> : : Liquid Shadow\nThe target creature is poisoned and its skill switched to \"vampire\".";
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
                        golem.Desc = "<sprite=3><sprite=3> : Endow\nGain the target weapon's ability and +X|+2. X is the weapon's attack.";
                        golem.SkillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.Skill = new Luciferin();
                        golem.Desc = "<sprite=3><sprite=3><sprite=3><sprite=3> : Luciferin\nAll your creatures without a skill gain \"bioluminescence\". Heal yourself for up to 10 HP";
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
                        golem.Desc = "<sprite=4><sprite=4> : Poison the target creature. If the target creature dies, it turns into a malignant cell.";
                        golem.SkillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.passiveSkills.DeadlyVenom = true;
                        golem.Desc = "Deadly Venom: \nAdd 2 poison damage to each successful attack. Cause poisoning if ingested.";
                        break;
                }
                break;
            case Element.Earth:
                switch (elementCount[maxValueKey])
                {
                    case 1:
                        golem.Skill = new Burrow();
                        golem.Desc = "<sprite=5> : Burrow\nThe Shard Golem can not be targeted, but its damage is halved.";
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
                        golem.Desc = "<sprite=5> : Guard\n(Do not attack) Delay the target creature for 1 turn (cumulative) and attack it unless it is airborne.";
                        golem.SkillCost = 1;
                        break;
                    case 6:
                    case 7:
                        golem.Skill = new Petrify();
                        golem.Desc = "<sprite=5><sprite=5> : Petrify\nThe target creature gains +0/+20 but can not attack or use skills for 6 turns.";
                        golem.SkillCost = 2;
                        break;
                }
                break;
            case Element.Entropy:
                switch (elementCount[maxValueKey])
                {
                    case 1:
                        golem.Skill = new Deadalive();
                        golem.Desc = "<sprite=6> : Dead and Alive\nKill this creature; death effects are triggered. This creature is still alive.";
                        golem.SkillCost = 1;
                        break;
                    case 2:
                        golem.Skill = new Mutation();
                        golem.Desc = "<sprite=6><sprite=6> : Mutation\nThe target creature might turn into an abomination, a mutant, or die.";
                        golem.SkillCost = 2;
                        break;
                    case 3:
                        golem.Skill = new Paradox();
                        golem.Desc = "<sprite=6><sprite=6> : Paradox\nKill the target creature if its attack is higher than its defence";
                        golem.SkillCost = 2;
                        break;
                    case 4:
                        golem.Skill = new Improve();
                        golem.Desc = "<sprite=6><sprite=6> : Improved Mutation\nThe target creature might turn into an abomination, a mutant, or die";
                        golem.SkillCost = 2;
                        break;
                    case 5:
                        golem.WeaponPassive = new ScrambleSkill();
                        golem.Desc = "Randomly convert some of the opponent's quantums into other elements.";
                        break;
                    case 6:
                    case 7:
                        golem.Skill = new Antimatter();
                        golem.Desc = "<sprite=6><sprite=6><sprite=6><sprite=6> : Antimatter\nInvert the attack power of the target creature (the creature inflict heals instead of damage)";
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
                        golem.Desc = "<sprite=7><sprite=7><sprite=7><sprite=7> : Deja Vu\nShard Golem creates a copy of itself";
                        golem.SkillCost = 4;
                        break;
                    case 4:
                    case 5:
                        golem.passiveSkills.Neurotoxin = true;
                        golem.Desc = "Neurotoxin: Add 1 poison damage to each successful attack, 1 extra poison for each card played by afflicted player.";
                        break;
                    case 6:
                    case 7:
                        golem.Skill = new Precognition();
                        golem.Desc = "<sprite=7><sprite=7> : Precognition\nYou can see your opponent's hand. Draw a card.";
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
                        golem.Desc = "Deal X damages at the end of every turn. X is the number of <sprite=8> you own, divided by 5.";
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
                        golem.Desc = "<sprite=9><sprite=9><sprite=9> : Devour\nSwallow a smaller (less HP's) creature and gain +1/+1";
                        golem.SkillCost = 3;
                        break;
                    case 6:
                    case 7:
                        golem.Skill = new Blackhole();
                        golem.Desc = "<sprite=9><sprite=9><sprite=9><sprite=9> : Black Hole\nAbsorb 3 quanta per element from the opponent. Gain 1 HP per absorbed quantum.";
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
                        golem.Desc = "<sprite=10><sprite=10> : Adrenaline\nThe target creature attacks multiple times per turn.";
                        golem.SkillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.Skill = new Mitosiss();
                        golem.Desc = "<sprite=10><sprite=10><sprite=10><sprite=10> Mitosis: \n Generate a daughter creature";
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
                        golem.Desc = "<sprite=11><sprite=11> : Steam\nGain 5 charges (+5|+0). Remove 1 charge per turn.";
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
                        golem.Desc = "<sprite=10><sprite=10><sprite=10><sprite=10> : Nymph's tears\nTurn one of your pillars into a Nymph";
                        golem.SkillCost = 4;
                        break;
                }
                break;
        }
        return golem;
    }

    internal Card GetPlaceholderCard(int index)
    {
        if (index == 1)
        {
            return GetCardFromId("4t2");
        }
        return GetCardFromId("4t1");

    }
}

public class AiDeckBuilder
{

    public List<Card> GetHalfBloodDeck(Element primary, Element secondary)
    {
        List<Card> deckToReturn = new();
        var intance = CardDatabase.Instance;
        //Get Quantum Pillars
        for (var i = 0; i < 4; i++)
        {
            deckToReturn.Add(intance.GetCardFromId("4sa"));
        }

        var shouldBeUpgraded = Random.Range(0, 100) < 30;
        //Get Major Element Pillars
        for (var i = 0; i < 20; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Pillar, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        //Get Major Element Cards

        //Weapon
        for (var i = 0; i < 3; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Weapon, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Shield
        for (var i = 0; i < 3; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Shield, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        // Creatures
        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Creature, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Spells
        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Spell, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Artifacts
        if (!primary.Equals(Element.Entropy) && !primary.Equals(Element.Fire) && !primary.Equals(Element.Other))
        {
            for (var i = 0; i < 4; i++)
            {
                deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Artifact, primary, shouldBeUpgraded));
                shouldBeUpgraded = Random.Range(0, 100) < 30;
            }
        }

        //Get Minor Element Cards
        // Creatures
        for (var i = 0; i < 5; i++)
        {
            deckToReturn.Add(intance.GetRandomCard(CardType.Creature, shouldBeUpgraded, true, secondary, true));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Spells
        for (var i = 0; i < 4; i++)
        {
            deckToReturn.Add(intance.GetRandomCard(CardType.Spell, shouldBeUpgraded, true, secondary, true));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        
        //Artifacts
        if (!primary.Equals(Element.Entropy) && !primary.Equals(Element.Fire) && !primary.Equals(Element.Other))
        {
            for (var i = 0; i < 1; i++)
            {
                deckToReturn.Add(intance.GetRandomCard(CardType.Artifact, shouldBeUpgraded, true, secondary, true));
                shouldBeUpgraded = Random.Range(0, 100) < 30;
            }
        }

        return deckToReturn;
    }
}