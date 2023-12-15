using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;


public class CardDatabase
{
    static CardDatabase()
    {
    }

    public CardDatabase()
    {
        FullCardList = SetupNewCardBase();
    }

    public static CardDatabase Instance { get; } = new();

    public Dictionary<string, string> CardNameToBackGroundString = new()
    {
        {"Animate Weapon", "Air"},
        {"Luciferin", "Light"},
        {"Luciferase", "Light"}
    };

    private readonly AiDeckBuilder _deckBuilder = new();

    public List<string> RareWeaponRewards = new (){ "5ic", "5ol", "5ur", "5f7", "5lh", "4vl", "52q", "55s", "58v", "5c5", "5ro","61u"};

    public List<Card> FullCardList = new();

    private readonly List<string> _illegalPets = new() { "4vr", "4t8", "4vf", "52h", "55o", "58r", "5bt", "5f2", "5id", "5la", "5of", "5rm", "5ul", "61v", "5lt", "7kd" };

    public Card GetRandomPet()
    {
        return FullCardList.Find(x => !x.iD.IsUpgraded() && !_illegalPets.Contains(x.iD) && x.cardType.Equals(CardType.Creature));
    }

    public Card GetOracleCreature(Element element)
    {
        return FullCardList.Find(x => !x.iD.IsUpgraded()
                        && x.costElement.Equals(element)
                        && !_illegalPets.Contains(x.iD)
                        && x.cardType.Equals(CardType.Creature)
                        && !x.cardName.Contains("Shard of"));
    }

    public List<string> TrainerCardList = new(){ "562", "5c7", "52s", "4vn", "595", "55v", "5lf", "4vo", "4vi", "5f6", "5us", "593", "592", "5f4", "5oi", "622", "5i7", "55t",
        "5c2", "5lc", "5i8", "5f9", "61q", "5uu", "5lj", "5li", "5c9", "55q", "4vk", "5v1", "4vj", "5ig", "4vp", "61r", "52p", "58t", "52o", "5rr", "5ia", "621", "5fb", "5f8", "5rk", "5on", "624",
        "5op", "5up", "594", "5oh", "7n2", "6u2", "7gn", "7dp", "718", "71c", "77l", "74f", "6u8", "80i", "7te", "7ap", "7th", "7h0", "6u9", "7qb", "7gq", "80h", "7n7", "80k", "7n9", "7an", "7dm",
        "7dk", "7do", "77k", "74d", "77d", "7js", "7go", "6u7", "7jv", "7ai", "7k2", "6u4", "719", "7t9", "7n1", "7k3", "74i", "77i", "77j", "7dr", "7q4", "7tc", "6u3", "80a", "80b", "74a", "52n",
        "561", "5v2", "5c6", "5ih", "5rl", "623", "5uq", "5lm", "52v", "5rp", "5om", "7ta", "7q5", "7ti", "80j", "7k6", "71f", "7q9", "7n6", "7am", "717", "7h1", "74h", "4ve","58p","5rn","5ib","55m",
        "5fd","5f1","5of","5ul","5i6","52h","560","5i5","5c0","55o","5f2","5ll","52u","5rh","5rm","5um","5od","5rt","5bt","4vm","5ri","5f3","5ok","5oj","52j","5c8","5c1","5uv","5ru","58u","5la","590",
        "55u","55n","5le","58q","5bu","5id","61s","596","5fa","4vh","4vd","5if","5ut","52t","55r","5un","5lb","5rs","61v","620","5fc","5l9","625","4vf","5bv","55l","5rq","4vq","5fe","591","4vr","52m",
        "61p","5ii","58r","5ie","52i","5v0","52k","5oe","7gm","6tv","7ju","7gt","7gr","809","77b","7t7","7dh","714","7mt","779","7q7","746","7dt","745","74g","7ag","7k5","71e","7q1","7qd","7q2","7n4",
        "7tf","7qe","77g","80c","6tt","71d","74b","7jr","7qc","80f","80l","7n3","7qa","6ua","7du","77h","6ub","716","7h2","7tg","7mu","6u6","7dj","713","7ah","7ae","77e","74e","747","711","7ad","7dq",
        "7af","7jq","748","6tu","7ds","7t5","7t6","80g","7gl","7gu","7jp","712","7di","7ao","7q6","7mv","77a","7gv","7td","77m","6u1","52q","4t3","4vl","5c5","5ro","5f7","4t5","61u","5lh","5ol",
        "58v","4tb","4t4","55s","5ic","5ur","6rj","7n5","80e","71a","6u5","7q8","7dn","77f","74c","6rl","7al","6rr","6rk","7k1","7gs","7tb","52r","61t","4vg","5uo","5c4","5f5","5og","55p","5lk",
        "5i9","5rj","5lg","4tc","52l","5ld","5c3","58s","5oo","77c","6u0","71b","749","7k4","7n8","7dl","7t8","7n0","7ak","7k0","7gp","80d","715","7jt","7aj","6rs","7q3","63a","61o","5pu","4vc",
        "52g","5f0","606","542","5aa","5bs","50u","5gi","576","55k","5de","5mq","5l8","5uk","4sa","58o","5rg","5t2","5jm","5i4","5oc","808","6ts","710","7dg","81q","7oe","7um","72i","78q","6ve","7f2",
        "75m","7bu","7la","7ri","7i6","7ac","744","7jo","7t4","6qq","778","7q0","7gk","7ms","63a","61o","5pu","4vc","52g","5f0","606","542","5aa","5bs","50u","5gi","576","55k","5de","5mq","5l8","5uk",
        "4sa","58o","5rg","5t2","5jm","5i4","5oc","808","6ts","710","7dg","81q","7oe","7um","72i","78q","6ve","7f2","75m","7bu","7la","7ri","7i6","7ac","744","7jo","7t4","6qq","778","7q0","7gk","7ms"};

    public List<string> WeaponIdList = new(){ "52q", "4t3", "4vl", "5c5", "5ro", "5f7", "4t5", "61u", "5lh", "5ol", "58v", "4tb", "4t4", "55s", "5ic", "5ur", "6rj", "7n5", "80e",
        "71a", "6u5", "7q8", "7dn", "77f", "74c", "6rl", "7al", "6rr", "6rk", "7k1", "7gs", "7tb" };
    public Card GetShardOfElement(Element element)
    {
        return GetAllShards().Find(x => x.costElement.Equals(element) && !x.iD.IsUpgraded());
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

    public List<Card> SetupNewCardBase()
    {
        var jsonString = Resources.Load<TextAsset>("Cards/CardDatabase");
        var newDc = JsonConvert.DeserializeObject<CardDB>(jsonString.text);
        return newDc.cardDb;
    }
    public Card GetCardFromId(string id)
    {
        var baseCard = FullCardList.Find(x => x.iD == id);

        return baseCard.Clone();
    }

    internal List<Card> GetAllBazaarCards()
    {
        return new List<Card>(FullCardList.FindAll(x => !x.IsRare() && x.iD.IsBazaarLegal()));
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
        List<Card> list = new(FullCardList.FindAll(x => x.cardName.Contains("Shard of")));
        return list;
    }

    public List<string> MarkIds = new() { "8pu", "8pr", "8pt", "8pq", "8pk", "8pm", "8pj", "8ps", "8po", "8pl", "8pn", "8pp" };
    private readonly List<string> _illegalHatchCards = new (){ "7qa", "7q2", "5ri", "61s", "80c", "6ro", "4t8", "6ub", "4vr", "74g", "560", "7dt", "5fd", "5rm", "7q6", "5lt", "7kd", "4t9", "6rp" };

    public Card GetRandomCard(CardType cardType, bool isUpgraded, bool shouldBeHatchLegal, Element element = Element.Aether, bool shouldBeElement = false)
    {
        if(FullCardList.Count == 0)
        {
            FullCardList = SetupNewCardBase();
        }
        List<Card> list = new(FullCardList.FindAll(x => x.cardType.Equals(cardType) && x.iD.IsUpgraded() == isUpgraded && x.iD.IsDeckLegal()));
        if (shouldBeHatchLegal)
        {
            list = list.FindAll(x => !_illegalHatchCards.Contains(x.iD));
        }
        if (shouldBeElement)
        {
            list = list.FindAll(x => x.costElement.Equals(element));
        }
        var card = list[Random.Range(0, list.Count)];
        return card.Clone();
    }

    public List<string> GetRandomDeck()
    {
        if (FullCardList == null) { SetupNewCardBase(); }
        List<string> deckToReturn = new();
        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomCard(CardType.Pillar, false, true).iD);
        }

        for (var i = 0; i < 20; i++)
        {
            deckToReturn.Add(GetRandomCard(CardType.Creature, false, true).iD);
        }

        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomCard(CardType.Spell, false, true).iD);
        }

        return deckToReturn;
    }

    public Card GetRandomCardOfTypeWithElement(CardType type, Element element, bool shouldBeUpgraded)
    {
        var reducedList = FullCardList.FindAll(x => x.costElement.Equals(element)
                                                    && x.cardType.Equals(type)
                                                    && !x.cardName.Contains("Shard of")
                                                    && !x.cardName.Contains(" Nymph")
                                                    && x.iD.IsUpgraded() == shouldBeUpgraded);
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
        card.atk += Random.Range(0, 4);
        card.def += Random.Range(0, 4);
        card.passiveSkills.Mutant = true;
        card.skillCost = Random.Range(1, 3);
        card.skillElement = card.costElement;
        var index = Random.Range(0, _mutantActiveAList.Count);
        var abilityName = _mutantActiveAList[index];

        switch (abilityName)
        {
            case "immaterial":
                card.skillCost = 0;
                card.innateSkills.Immaterial = true;
                break;
            case "momentum":
            case "scavenger":
                card.skillCost = 0;
                card.passiveSkills.Scavenger = true;
                break;
            default:
                card.skill = abilityName;
                break;
        }
        var skillCost = card.skillCost == 0 ? "" : card.skillCost == 1 ? $"<sprite={(int)card.costElement}>" : $"<sprite={(int)card.costElement}><sprite={(int)card.costElement}>";
        card.desc = $"{AddSpacesToSentence(abilityName)} {skillCost} : \n {_mutantActiveADescList[index]}";

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

    public Card GetGolemAbility(List<IDCardPair> shardList)
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
            elementCount[item.card.costElement]++;
            switch (item.card.costElement)
            {
                case Element.Earth:
                    golem.atk += item.card.iD.IsUpgraded() ? 2 : 1;
                    golem.def += item.card.iD.IsUpgraded() ? 5 : 4;
                    break;
                case Element.Fire:
                    golem.atk += item.card.iD.IsUpgraded() ? 4 : 3;
                    golem.def += item.card.iD.IsUpgraded() ? 1 : 0;
                    break;
                case Element.Gravity:
                    golem.atk += item.card.iD.IsUpgraded() ? 1 : 0;
                    golem.def += item.card.iD.IsUpgraded() ? 7 : 6;
                    break;
                default:
                    golem.atk += item.card.iD.IsUpgraded() ? 3 : 2;
                    golem.def += item.card.iD.IsUpgraded() ? 3 : 2;
                    break;
            }
            EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(item.id));
        }

        if (elementCount[Element.Air] > 0)
        {
            golem.innateSkills.Airborne = true;
        }

        if (elementCount[Element.Darkness] > 0)
        {
            golem.innateSkills.Devourer = true;
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

        golem.skillElement = maxValueKey;
        switch (maxValueKey)
        {
            case Element.Aether:
                switch (elementCount[maxValueKey])
                {
                    case 3:
                    case 4:
                    case 5:
                        golem.skill = "lobotomize";
                        golem.skillCost = 2;
                        golem.desc = "<sprite=0><sprite=0> : Remove any skill from the target creature.";
                        break;
                    case 6:
                    case 7:
                        golem.innateSkills.Immaterial = true;
                        golem.desc = "Immaterial: \n Golem can not be targeted.";
                        break;
                }
                break;
            case Element.Air:
                switch (elementCount[maxValueKey])
                {
                    case 2:
                        golem.skill = "queen";
                        golem.skillCost = 2;
                        golem.desc = "<sprite=1><sprite=1> : Firefly\nGenerate a firefly.";
                        break;
                    case 3:
                        golem.skill = "sniper";
                        golem.desc = "<sprite=1><sprite=1> : Sniper\nDeal 3 damage to the target creature.";
                        golem.skillCost = 2;
                        break;
                    case 4:
                    case 5:
                        golem.skill = "dive";
                        golem.desc = "<sprite=1><sprite=1> : Dive\nThe damage dealt is doubled for 1 turn.";
                        golem.skillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "unstablegas";
                        golem.desc = "<sprite=1><sprite=1> : Unstable gas\n Generate unstable gas";
                        golem.skillCost = 2;
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
                        golem.skill = "liquidshadow";
                        golem.desc = "<sprite=2><sprite=2> : : Liquid Shadow\nThe target creature is poisoned and its skill switched to \"vampire\".";
                        golem.skillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "steal";
                        golem.desc = "<sprite=2><sprite=2><sprite=2> : : Steal\nSteal a permanent";
                        golem.skillCost = 3;
                        break;
                }
                break;
            case Element.Light:
                switch (elementCount[maxValueKey])
                {
                    case 1:
                    case 2:
                        golem.skill = "heal";
                        golem.desc = "<sprite=3> : Heal\nHeal the target creature up to 5 HP's";
                        golem.skillCost = 1;
                        break;
                    case 3:
                    case 4:
                    case 5:
                        golem.skill = "endow";
                        golem.desc = "<sprite=3><sprite=3> : Endow\nGain the target weapon's ability and +X|+2. X is the weapon's attack.";
                        golem.skillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "luciferin";
                        golem.desc = "<sprite=3><sprite=3><sprite=3><sprite=3> : Luciferin\nAll your creatures without a skill gain \"bioluminescence\". Heal yourself for up to 10 HP";
                        golem.skillCost = 4;
                        break;
                }
                break;
            case Element.Death:
                switch (elementCount[maxValueKey])
                {
                    case 1:
                        golem.skill = "infection";
                        golem.desc = "<sprite=4> : Infection\nInflict 1 damage per turn to a target creature.";
                        golem.skillCost = 1;
                        break;
                    case 2:
                    case 3:
                        golem.passiveSkills.Scavenger = true;
                        golem.desc = "Scavenger:\nEvery time a creature dies, Shard Golem gains +1/+1";
                        break;
                    case 4:
                        golem.passiveSkills.Venom = true;
                        golem.desc = "Deal 1 poison damage at the end of every turn.\nPoison damage is cumulative.";
                        break;
                    case 5:
                        golem.skill = "aflatoxin";
                        golem.desc = "<sprite=4><sprite=4> : Poison the target creature. If the target creature dies, it turns into a malignant cell.";
                        golem.skillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.passiveSkills.DeadlyVenom = true;
                        golem.desc = "Deadly Venom: \nAdd 2 poison damage to each successful attack. Cause poisoning if ingested.";
                        break;
                }
                break;
            case Element.Earth:
                switch (elementCount[maxValueKey])
                {
                    case 1:
                        golem.skill = "burrow";
                        golem.desc = "<sprite=5> : Burrow\nThe Shard Golem can not be targeted, but its damage is halved.";
                        golem.skillCost = 1;
                        break;
                    case 2:
                    case 3:
                        golem.skill = "stoneform";
                        golem.desc = "<sprite=5> : Stone form\nShard Golem gains +0 / +20";
                        golem.skillCost = 1;
                        break;
                    case 4:
                    case 5:
                        golem.skill = "guard";
                        golem.desc = "<sprite=5> : Guard\n(Do not attack) Delay the target creature for 1 turn (cumulative) and attack it unless it is airborne.";
                        golem.skillCost = 1;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "petrify";
                        golem.desc = "<sprite=5><sprite=5> : Petrify\nThe target creature gains +0/+20 but can not attack or use skills for 6 turns.";
                        golem.skillCost = 2;
                        break;
                }
                break;
            case Element.Entropy:
                switch (elementCount[maxValueKey])
                {
                    case 1:
                        golem.skill = "deadalive";
                        golem.desc = "<sprite=6> : Dead and Alive\nKill this creature; death effects are triggered. This creature is still alive.";
                        golem.skillCost = 1;
                        break;
                    case 2:
                        golem.skill = "mutation";
                        golem.desc = "<sprite=6><sprite=6> : Mutation\nThe target creature might turn into an abomination, a mutant, or die.";
                        golem.skillCost = 2;
                        break;
                    case 3:
                        golem.skill = "paradox";
                        golem.desc = "<sprite=6><sprite=6> : Paradox\nKill the target creature if its attack is higher than its defence";
                        golem.skillCost = 2;
                        break;
                    case 4:
                        golem.skill = "improve";
                        golem.desc = "<sprite=6><sprite=6> : Improved Mutation\nThe target creature might turn into an abomination, a mutant, or die";
                        golem.skillCost = 2;
                        break;
                    case 5:
                        golem.innateSkills.Scramble = true;
                        golem.desc = "Randomly convert some of the opponent's quantums into other elements.";
                        break;
                    case 6:
                    case 7:
                        golem.skill = "anitmatter";
                        golem.desc = "<sprite=6><sprite=6><sprite=6><sprite=6> : Antimatter\nInvert the attack power of the target creature (the creature inflict heals instead of damage)";
                        golem.skillCost = 4;
                        break;
                }
                break;
            case Element.Time:
                switch (elementCount[maxValueKey])
                {
                    case 2:
                        golem.skill = "scarab";
                        golem.desc = "<sprite=7><sprite=7> : Scarab\nGenerate a Scarab.";
                        golem.skillCost = 2;
                        break;
                    case 3:
                        golem.skill = "dejavu";
                        golem.desc = "<sprite=7><sprite=7><sprite=7><sprite=7> : Deja Vu\nShard Golem creates a copy of itself";
                        golem.skillCost = 4;
                        break;
                    case 4:
                    case 5:
                        golem.passiveSkills.Neurotoxin = true;
                        golem.desc = "Neurotoxin: Add 1 poison damage to each successful attack, 1 extra poison for each card played by afflicted player.";
                        break;
                    case 6:
                    case 7:
                        golem.skill = "precognition";
                        golem.desc = "<sprite=7><sprite=7> : Precognition\nYou can see your opponent's hand. Draw a card.";
                        golem.skillCost = 2;
                        break;
                }
                break;
            case Element.Fire:
                switch (elementCount[maxValueKey])
                {
                    case 2:
                        golem.skill = "ablaze";
                        golem.desc = "<sprite=8> : Ablaze\nShard Golem gains +2/+0";
                        golem.skillCost = 1;
                        break;
                    case 3:
                    case 4:
                        golem.innateSkills.Fiery = true;
                        golem.desc = "Deal X damages at the end of every turn. X is the number of <sprite=8> you own, divided by 5.";
                        break;
                    case 5:
                        golem.skill = "destroy";
                        golem.desc = "<sprite=8><sprite=8><sprite=8>: Destroy\nShatter the target permanent.";
                        golem.skillCost = 3;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "rage";
                        golem.desc = "<sprite=8><sprite=8> : Rage\nThe target creature gains +5/-5";
                        golem.skillCost = 2;
                        break;
                }
                break;
            case Element.Gravity:
                switch (elementCount[maxValueKey])
                {
                    case 5:
                        golem.skill = "devour";
                        golem.desc = "<sprite=9><sprite=9><sprite=9> : Devour\nSwallow a smaller (less HP's) creature and gain +1/+1";
                        golem.skillCost = 3;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "blackhole";
                        golem.desc = "<sprite=9><sprite=9><sprite=9><sprite=9> : Black Hole\nAbsorb 3 quanta per element from the opponent. Gain 1 HP per absorbed quantum.";
                        golem.skillCost = 4;
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
                        golem.skill = "growth";
                        golem.desc = "<sprite=10><sprite=10> : Growth\nThe Shard Golem gains +2/+2";
                        golem.skillCost = 2;
                        break;
                    case 5:
                        golem.skill = "adrenaline";
                        golem.desc = "<sprite=10><sprite=10> : Adrenaline\nThe target creature attacks multiple times per turn.";
                        golem.skillCost = 2;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "mitosiss";
                        golem.desc = "<sprite=10><sprite=10><sprite=10><sprite=10> Mitosis: \n Generate a daughter creature";
                        golem.skillCost = 4;
                        break;
                }
                break;
            case Element.Water:
                switch (elementCount[maxValueKey])
                {
                    case 2:
                    case 3:
                        golem.skill = "steam";
                        golem.desc = "<sprite=11><sprite=11> : Steam\nGain 5 charges (+5|+0). Remove 1 charge per turn.";
                        golem.skillCost = 2;
                        break;
                    case 4:
                    case 5:
                        golem.skill = "freeze";
                        golem.desc = "<sprite=11><sprite=11><sprite=11> : Freeze\nFreeze the target creature";
                        golem.skillCost = 3;
                        break;
                    case 6:
                    case 7:
                        golem.skill = "nymph";
                        golem.desc = "<sprite=10><sprite=10><sprite=10><sprite=10> : Nymph's tears\nTurn one of your pillars into a Nymph";
                        golem.skillCost = 4;
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