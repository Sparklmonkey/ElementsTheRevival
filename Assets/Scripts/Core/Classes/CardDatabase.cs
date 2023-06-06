using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;


public class CardDatabase : MonoBehaviour
{

    private static readonly CardDatabase instance = new();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static CardDatabase()
    {
    }

    private CardDatabase()
    {
        SetupNewCardBase();
    }

    public static CardDatabase Instance
    {
        get
        {
            return instance;
        }
    }


    public List<string> endTurnPassives = new List<string> { "fire", "sanctuary","gratitude", "infest", "light", "air", "earth", "overdrive", 
        "acceleration", "void", "empathy", "flood", "devourer", "swarm", "patience", "singularity"};

    internal List<int> GetFullCardCount()
    {
        var elementCount = new List<int>();
        for (int i = 0; i < 13; i++)
        {
            elementCount.Add(fullCardList.FindAll(x => !x.iD.IsUpgraded() && x.costElement.Equals((Element)i)).Count);
        }
        return elementCount;
    }

    public List<string> skillsNoTarget = new List<string> {"duality", "hasten", "ignite", "scarab","ablaze", "black hole", "luciferin", "dead / alive", "lycanthropy", "deja vu", "hatch", "evolve", "unstable gas", "dive",
        "gravity pullc", "growth", "plague", "poison", "stone form", "precognition", "photosynthesis", "steam", "queen", "divineshield", "burrow", "rebirth", "silence", "sacrifice", "patience",
        "serendipity", "bravery", "nova", "pandemonium", "miracle", "thunderstorm", "flying", "stoneskin", "healp", "mitosis", "rain of fire", "blitz", "supernova", "deadly poison",
        "shard"};

    public List<string> skillsWithTarget = new List<string> { "sniper", "mutation", "mitosiss", "improve", "antimatter", "web", "endow", "liquid shadow", "accretion", "devour", "immortality","infect", "congeal",
        "berserk", "lobotomize", "rage", "paradox", "freeze", "infection", "nymph", "heal", "petrify", "aflatoxin", "guard", "fractal", "chaos power", "overdrive", "readiness", "wisdom", "chaos",
        "butterfly", "momentum", "acceleration", "armor", "reverse time", "enchant", "earthquake", "adrenaline", "fire bolt", "destroy", "immolate", "icebolt", "purify", "holy light", "blessing",
        "shockwave", "steal", "drain life", "nightmare", "gravity pull", "lightning", "parallel universe","heavy armor", "cremation"};

    public List<string> rareWeaponRewards = new (){ "5ic", "5ol", "5ur", "5f7", "5lh", "4vl", "52q", "55s", "58v", "5c5", "5ro","61u"};

    private List<Card> fullCardList;

    private List<string> illegalPets = new() { "4vr", "4t8", "4vf", "52h", "55o", "58r", "5bt", "5f2", "5id", "5la", "5of", "5rm", "5ul", "61v" };
    public Card GetRandomPet()
    {
        return fullCardList.Find(x => !x.iD.IsUpgraded() && !illegalPets.Contains(x.iD) && x.cardType.Equals(CardType.Creature));
    }
    public Card GetOracleCreature(Element element)
    {
        return fullCardList.Find(x => !x.iD.IsUpgraded() 
                        && x.costElement.Equals(element) 
                        && !illegalPets.Contains(x.iD) 
                        && x.cardType.Equals(CardType.Creature)
                        && !x.cardName.Contains("Shard of"));
    }

    public List<string> trainerCardList = new List<string> { "562", "5c7", "52s", "4vn", "595", "55v", "5lf", "4vo", "4vi", "5f6", "5us", "593", "592", "5f4", "5oi", "622", "5i7", "55t", 
        "5c2", "5lc", "5i8", "5f9", "61q", "5uu", "5lj", "5li", "5c9", "55q", "4vk", "5v1", "4vj", "5ig", "4vp", "61r", "52p", "58t", "52o", "5rr", "5ia", "621", "5fb", "5f8", "5rk", "5on", "624", 
        "5op", "5up", "594", "5oh", "7n2", "6u2", "7gn", "7dp", "718", "71c", "77l", "74f", "6u8", "80i", "7te", "7ap", "7th", "7h0", "6u9", "7qb", "7gq", "80h", "7n7", "80k", "7n9", "7an", "7dm", 
        "7dk", "7do", "77k", "74d", "77d", "7js", "7go", "6u7", "7jv", "7ai", "7k2", "6u4", "719", "7t9", "7n1", "7k3", "74i", "77i", "77j", "7dr", "7q4", "7tc", "6u3", "80a", "80b", "74a", "52n", 
        "561", "5v2", "5c6", "5ih", "5rl", "623", "5uq", "5lm", "52v", "5rp", "5om", "7ta", "7q5", "7ti", "80j", "7k6", "71f", "7q9", "7n6", "7am", "717", "7h1", "74h", "4ve","58p","5rn","5ib","55m",
        "5fd","5f1","5of","5ul","5i6","52h","560","5i5","5c0","55o","5f2","5ll","52u","5rh","5rm","5um","5od","5rt","5bt","4vm","5ri","5f3","5ok","5oj","52j","5c8","5c1","5uv","5ru","58u","5la","590",
        "55u","55n","5le","58q","5bu","5id","61s","596","5fa","4vh","4vd","5if","5ut","52t","55r","5un","5lb","5rs","61v","620","5fc","5l9","625","4vf","5bv","55l","5rq","4vq","5fe","591","4vr","52m",
        "61p","5ii","58r","5ie","52i","5v0","52k","5oe","7gm","6tv","7ju","7gt","7gr","809","77b","7t7","7dh","714","7mt","779","7q7","746","7dt","745","74g","7ag","7k5","71e","7q1","7qd","7q2","7n4",
        "7tf","7qe","77g","80c","6tt","71d","74b","7jr","7qc","80f","80l","7n3","7qa","6ua","7du","77h","6ub","716","7h2","7tg","7mu","6u6","7dj","713","7ah","7ae","77e","74e","747","711","7ad","7dq",
        "7af","7jq","0","748","6tu","7ds","7t5","7t6","80g","7gl","7gu","7jp","712","7di","7ao","7q6","7mv","77a","7gv","7td","77m","6u1","52q","4t3","4vl","5c5","5ro","5f7","4t5","61u","5lh","5ol",
        "58v","4tb","4t4","55s","5ic","5ur","0","6rj","7n5","80e","71a","6u5","7q8","7dn","77f","74c","6rl","7al","6rr","6rk","7k1","7gs","7tb","52r","61t","4vg","5uo","5c4","5f5","5og","55p","5lk",
        "5i9","5rj","5lg","4tc","0","52l","5ld","5c3","58s","5oo","77c","6u0","71b","749","7k4","7n8","7dl","7t8","7n0","7ak","7k0","7gp","80d","715","7jt","7aj","6rs","7q3","63a","61o","5pu","4vc",
        "52g","5f0","606","542","5aa","5bs","50u","5gi","576","55k","5de","5mq","5l8","5uk","4sa","58o","5rg","5t2","5jm","5i4","5oc","808","6ts","710","7dg","81q","7oe","7um","72i","78q","6ve","7f2",
        "75m","7bu","7la","7ri","7i6","7ac","744","7jo","7t4","6qq","778","7q0","7gk","7ms","63a","61o","5pu","4vc","52g","5f0","606","542","5aa","5bs","50u","5gi","576","55k","5de","5mq","5l8","5uk",
        "4sa","58o","5rg","5t2","5jm","5i4","5oc","808","6ts","710","7dg","81q","7oe","7um","72i","78q","6ve","7f2","75m","7bu","7la","7ri","7i6","7ac","744","7jo","7t4","6qq","778","7q0","7gk","7ms"};

    public List<string> weaponIdList = new List<string> { "52q", "4t3", "4vl", "5c5", "5ro", "5f7", "4t5", "61u", "5lh", "5ol", "58v", "4tb", "4t4", "55s", "5ic", "5ur", "0", "6rj", "7n5", "80e",
        "71a", "6u5", "7q8", "7dn", "77f", "74c", "6rl", "7al", "6rr", "6rk", "7k1", "7gs", "7tb" };
    public Card GetShardOfElement(Element element)
    {
        return GetAllShards().Find(x => x.costElement.Equals(element) && !x.iD.IsUpgraded());
    }

    private readonly List<string> mutantActiveAList = new List<string>
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

    public void SetupNewCardBase()
    {
        TextAsset jsonString = Resources.Load<TextAsset>("Cards/CardDatabase");
        CardDB cardDBNew = JsonUtility.FromJson<CardDB>(jsonString.text);
        fullCardList = cardDBNew.cardDb;
    }
    public Card GetCardFromId(string id)
    {
        Card baseCard = fullCardList.Find(x => x.iD == id);

        return new Card(baseCard);
    }

    internal List<Card> GetAllBazaarCards()
    {
        return new List<Card>(fullCardList.FindAll(x => !x.IsRare() && !x.iD.IsUpgraded() && x.iD.IsBazaarLegal()));
    }

    internal List<Card> GetCardListWithID(List<string> cardRewards)
    {
        List<Card> cardObjects = new List<Card>();
        foreach (var cardId in cardRewards)
        {
            cardObjects.Add(GetCardFromId(cardId));
        }
        return cardObjects;
    }

    public List<Card> GetAllShards()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => x.cardName.Contains("Shard of")));
        return list;
    }

    public List<string> markIds = new List<string> { "4su", "4sr", "4st", "4sq", "4sk", "4sm", "4sj", "4ss", "4so", "4sl", "4sn", "4sp" };
    
    public string weaponsIds = "52q 4t3 4vl 5c5 5ro 5f7 4t5 61u 5lh 5ol 58v 4tb 4t4 55s 5ic 5ur 6rj 7n5 80e 71a 6u5 7q8 7dn 77f 74c 6rl 7al 6rr 6rk 7k1 7gs 7tb";

    private string baseSOpath = @"Cards";
    internal Card GetRandomSpell()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => x.cardType == CardType.Spell && !x.iD.IsUpgraded()));
        Card card = list[Random.Range(0, list.Count)];
        return new Card(card);
    }
    public Card GetRandomEliteSpell()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => x.cardType == CardType.Spell && x.iD.IsUpgraded()));
        Card card = list[Random.Range(0, list.Count)];
        return new Card(card);
    }

    public Card GetRandomEliteCreature()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => !illegalHatchCards.Contains(x.iD) && x.cardType == CardType.Creature && x.iD.IsUpgraded()));
        Card card = list[Random.Range(0, list.Count)];
        return new Card(card);
    }

    public Card GetRandomCreature()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => !illegalHatchCards.Contains(x.iD) && x.cardType == CardType.Creature && !x.iD.IsUpgraded()));
        Card card = list[Random.Range(0, list.Count)];
        return new Card(card);
    }

    public Card GetRandomHatchCreature()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => !illegalHatchCards.Contains(x.iD) && x.cardType.Equals(CardType.Creature) && !x.iD.IsUpgraded()));
        System.Random rnd = new System.Random();
        Card card = list.OrderBy(x => rnd.Next())
                          .First();
        Card cardToReturn = new Card(card);
        cardToReturn.cardName = cardToReturn.cardName.Replace("(Clone)", "");
        return cardToReturn;
    }

    List<string> illegalHatchCards = new List<string> { "7qa", "7q2", "5ri", "61s", "80c", "6ro", "4t8", "6ub", "4vr", "74g", "560", "7dt", "5fd", "5rm", "7q6" };
    public Card GetRandomEliteHatchCreature()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => !illegalHatchCards.Contains(x.iD) && x.cardType.Equals(CardType.Creature) && x.iD.IsUpgraded()));
        System.Random rnd = new System.Random();
        Card card = list.OrderBy(x => rnd.Next())
                          .First();
        Card cardToReturn = new Card(card);
        cardToReturn.cardName = cardToReturn.cardName.Replace("(Clone)", "");
        return cardToReturn;
    }

    public Card GetRandomPillar()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => x.cardType == CardType.Pillar && !x.iD.IsUpgraded()));
        Card card = list[Random.Range(0, list.Count)];
        return new Card(card);
    }

    public Card GetRandomTower()
    {
        List<Card> list = new List<Card>(fullCardList.FindAll(x => x.cardType == CardType.Pillar && x.iD.IsUpgraded()));
        Card card = list[Random.Range(0, list.Count)];
        return new Card(card);
    }

    public List<string> GetRandomDeck()
    {
        if(fullCardList == null) { SetupNewCardBase(); }
        List<string> deckToReturn = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomPillar().iD);
        }

        for (int i = 0; i < 20; i++)
        {
            deckToReturn.Add(GetRandomCreature().iD);
        }

        for (int i = 0; i < 10; i++)
        {
            deckToReturn.Add(GetRandomSpell().iD);
        }

        return deckToReturn;
    }

    public Card GetRandomCardOfTypeWithElement(CardType type, Element element, bool shouldBeUpgraded)
    {
        string upgradedFolder = shouldBeUpgraded ? "Upgraded" : "Regular";

        
        List<Card> reducedList = fullCardList.FindAll(x => x.costElement.Equals(element) && x.cardType.Equals(type) && !x.cardName.Contains("Shard of") && !x.cardName.Contains(" Nymph"));
        if(reducedList.Count > 0)
        {
            Card card = reducedList[Random.Range(0, reducedList.Count)];
            Card cardToReturn = new(card);
            return cardToReturn;
        }
        return GetRandomCardOfTypeWithElement((CardType)Random.Range(0, 6), element, shouldBeUpgraded);
    }

    public List<Card> GetHalfBloodDeck(Element primary, Element secondary)
    {
        List<Card> deckToReturn = new List<Card>();

        //Get Quantum Pillars
        for (int i = 0; i < 4; i++)
        {
            deckToReturn.Add(GetCardFromId("4sa"));
        }

        bool shouldBeUpgraded = Random.Range(0, 100) < 30;
        //Get Major Element Pillars
        for (int i = 0; i < 20; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Pillar, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Get Major Element Cards
        int cardTotal = 30;
        int cardCount = Random.Range(0, cardTotal);
        // Creatures
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Creature, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        cardTotal -= cardCount;

        //Spells
        cardCount = Random.Range(0, cardTotal);
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Spell, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        cardTotal -= cardCount;

        cardCount = 0;
        //Artifacts
        if (!primary.Equals(Element.Earth))
        {
            cardCount = Random.Range(0, cardTotal);
            for (int i = 0; i < cardCount; i++)
            {
                deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Artifact, primary, shouldBeUpgraded));
                shouldBeUpgraded = Random.Range(0, 100) < 30;
            }
        }
        cardTotal -= cardCount;

        //Weapon
        cardCount = Random.Range(0, cardTotal);
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Weapon, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        cardTotal -= cardCount;

        //Shield
        cardCount = cardTotal;
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Shield, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        //Get Major Element Cards
        cardTotal = 10;
        cardCount = Random.Range(0, cardTotal);
        // Creatures
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Creature, secondary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        cardTotal -= cardCount;

        //Spells
        cardCount = Random.Range(0, cardTotal);
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Spell, secondary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        cardTotal -= cardCount;

        cardCount = 0;
        //Artifacts
        if (!primary.Equals(Element.Earth))
        {
            cardCount = Random.Range(0, cardTotal);
            for (int i = 0; i < cardCount; i++)
            {
                deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Artifact, secondary, shouldBeUpgraded));
                shouldBeUpgraded = Random.Range(0, 100) < 30;
            }
        }
        cardTotal -= cardCount;

        //Weapon
        cardCount = Random.Range(0, cardTotal);
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Weapon, secondary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        cardTotal -= cardCount;

        //Shield
        cardCount = cardTotal;
        for (int i = 0; i < cardCount; i++)
        {
            deckToReturn.Add(GetRandomCardOfTypeWithElement(CardType.Shield, secondary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        return deckToReturn;
    }

    public Card GetMutant(bool isUpgraded, Card fromCard = null)
    {

        Card card = fromCard == null ? isUpgraded ? GetRandomEliteHatchCreature() : GetRandomHatchCreature() : fromCard;
        card.atk += Random.Range(0, 4);
        card.def += Random.Range(0, 4);
        card.passive.Add("mutant");
        card.skillCost = Random.Range(1, 3);
        card.skillElement = card.costElement;
        int index = Random.Range(0, mutantActiveAList.Count);
        string abilityName = mutantActiveAList[index];

        switch (abilityName)
        {
            case "immaterial":
                card.skillCost = 0;
                card.innate.Add("immaterial");
                break;
            case "momentum":
            case "scavenger":
                card.skillCost = 0;
                card.passive.Add(abilityName);
                break;
            default:
                card.skill = abilityName;
                break;
        }
        string skillCost = card.skillCost == 0 ? "" : card.skillCost == 1 ? $"<sprite={(int)card.costElement}>" : $"<sprite={(int)card.costElement}><sprite={(int)card.costElement}>";
        card.desc = $"{AddSpacesToSentence(abilityName)} {skillCost} : \n {mutantActiveADescList[index]}";

        return card;
    }

    private List<string> mutantActiveADescList = new List<string>
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
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        return textInfo.ToTitleCase(text);
    }

    private Dictionary<Element, string> regularNymphNames = new Dictionary<Element, string> {
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


    private Dictionary<Element, string> eliteNymphNames = new Dictionary<Element, string> {
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
            return GetCardFromId(regularNymphNames[(Element)Random.Range(0, 12)]);
        }
        return GetCardFromId(regularNymphNames[element]);
    }
    public Card GetRandomEliteNymph(Element element)
    {
        if (element.Equals(Element.Other))
        {
            return GetCardFromId(eliteNymphNames[(Element)Random.Range(0, 12)]);
        }
        return GetCardFromId(eliteNymphNames[element]);
    }

    public Card GetGolemAbility(QuantaObject shardElementLast, Card golem)
    {
        //switch (shardElementLast.element)
        //{
        //    case Element.Aether:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //            case 2:
        //                golem.passive.Add("immaterial");
        //                golem.desc = "Immaterial: \n Golem can not be targeted.";
        //                break;
        //            default:
        //                golem.skill = "lobotomizer";
        //                golem.skillCost = 2;
        //                golem.skillElement = Element.Aether;
        //                golem.desc = "<sprite=0><sprite=0> : Remove any skill from the target creature.";
        //                break;
        //        }
        //        break;
        //    case Element.Air:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.innate.Add("airborne");
        //                break;
        //            case 2:
        //                golem.skill = "queen";
        //                golem.desc = "<sprite=10><sprite=10> : Firefly\nGenerate a firefly.";
        //                golem.skillCost = 2;
        //                golem.skillElement = Element.Life;
        //                break;
        //            case 3:
        //                golem.skill = "sniper";
        //                golem.desc = "<sprite=1><sprite=1> : Sniper\nDeal 3 damage to the target creature.";
        //                golem.skillCost = 2;
        //                golem.skillElement = Element.Air;
        //                break;
        //            case 4:
        //                golem.skill = "dive";
        //                golem.desc = "<sprite=1> : Dive\nThe damage dealt is doubled for 1 turn.";
        //                golem.skillCost = 1;
        //                golem.skillElement = Element.Air;
        //                break;
        //            default:
        //                golem.skill = "unstable gas";
        //                break;
        //        }
        //        break;
        //    case Element.Darkness:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.passive.Add("devourer");
        //                break;
        //            case 2:
        //                golem.innate.Add("voodoo");
        //                golem.desc = "Voodoo:\nIf this creature survives an attack, damage and status is inflicted to your opponent as well.";
        //                break;
        //            case 3:
        //            case 4:
        //                golem.passive.Add("vampire");
        //                break;
        //            case 5:
        //                golem.skill = "liquid shadow";
        //                break;
        //            default:
        //                golem.skill = "steal";
        //                break;
        //        }
        //        break;
        //    case Element.Light:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = "heal";
        //                break;
        //            case 2:
        //            case 3:
        //                golem.skill = "endow";
        //                break;
        //            default:
        //                golem.skill = "luciferin";
        //                break;
        //        }
        //        break;
        //    case Element.Death:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = "infection";
        //                break;
        //            case 2:
        //                golem.passive.Add("savenger");
        //                break;
        //            case 3:
        //            case 4:
        //                golem.passive.Add("venemous");
        //                break;
        //            case 5:
        //                golem.skill = "alfatoxin";
        //                break;
        //            default:
        //                golem.passive.Add("deadly venom");
        //                break;
        //        }
        //        break;
        //    case Element.Earth:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = new ActiveABurrow();
        //                break;
        //            case 2:
        //                golem.skill = new ActiveAStoneForm();
        //                break;
        //            case 3:
        //                golem.skill = new ActiveAGuard();
        //                break;
        //            case 4:
        //                golem.skill = new ActiveAGuard();
        //                break;
        //            default:
        //                golem.skill = new ActiveAPetrify();
        //                break;
        //        }
        //        break;
        //    case Element.Entropy:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = new ActiveADeadAlive();
        //                break;
        //            case 2:
        //                golem.skill = new ActiveAMutation();
        //                break;
        //            case 3:
        //                golem.skill = new ActiveAParadox();
        //                break;
        //            case 4:
        //                golem.skill = new ActiveAImprovedMutation();
        //                break;
        //            //case 5:
        //            //    return "ActiveAScramble";
        //            default:
        //                golem.skill = new ActiveAAntimatter();
        //                break;
        //        }
        //        break;
        //    case Element.Time:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = new ActiveASpawnScarab();
        //                break;
        //            case 2:
        //                golem.skill = new ActiveASpawnScarab();
        //                break;
        //            case 3:
        //                golem.skill = new ActiveADejaVu();
        //                break;
        //            case 4:
        //                golem.cardPassives.hasNuerotoxin = true;
        //                break;
        //            case 5:
        //                golem.cardPassives.hasNuerotoxin = true;
        //                break;
        //            default:
        //                golem.skill = new ActiveAPrecognition();
        //                break;
        //        }
        //        break;
        //    case Element.Fire:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = new ActiveAAblaze();
        //                break;
        //            case 2:
        //                golem.skill = new ActiveAAblaze();
        //                break;
        //            case 3:
        //                golem.cardPassives.isFiery = true;
        //                break;
        //            case 4:
        //                golem.cardPassives.isFiery = true;
        //                break;
        //            case 5:
        //                golem.cardPassives.isFiery = true;
        //                break;
        //            default:
        //                golem.skill = new ActiveARageThree();
        //                break;
        //        }
        //        break;
        //    case Element.Gravity:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.cardPassives.hasMomentum = true;
        //                break;
        //            case 2:
        //                golem.cardPassives.hasMomentum = true;
        //                break;
        //            case 3:
        //                golem.skill = new ActiveADevour();
        //                break;
        //            case 4:
        //                golem.skill = new ActiveADevour();
        //                break;
        //            case 5:
        //                golem.skill = new ActiveADevour();
        //                break;
        //            default:
        //                golem.skill = new ActiveABlackHole();
        //                break;
        //        }
        //        break;
        //    case Element.Life:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = new ActiveAGrowthWater();
        //                break;
        //            case 2:
        //                golem.cardPassives.hasAdrenaline = true;
        //                break;
        //            case 3:
        //                golem.cardPassives.hasAdrenaline = true;
        //                break;
        //            case 4:
        //                golem.cardPassives.hasAdrenaline = true;
        //                break;
        //            case 5:
        //                golem.skill = new ActiveAAdrenaline();
        //                break;
        //            default:
        //                golem.skill = new ActiveAMitosis();
        //                golem.desc = "Mitosis: \n Generate a daughter creature";
        //                break;
        //        }
        //        break;
        //    case Element.Water:
        //        switch (shardElementLast.count)
        //        {
        //            case 1:
        //                golem.skill = new ActiveASteam();
        //                break;
        //            case 2:
        //                golem.skill = new ActiveASteam();
        //                break;
        //            case 3:
        //                golem.skill = new ActiveASteam();
        //                break;
        //            case 4:
        //                golem.skill = new ActiveAFreeze();
        //                break;
        //            case 5:
        //                golem.skill = new ActiveAFreeze();
        //                break;
        //            default:
        //                golem.skill = new ActiveANymphTear();
        //                break;
        //        }
        //        break;
        //    case Element.Other:
        //        break;
        //    default:
        //        break;
        //}
        return golem;
    }

    internal Card GetPlaceholderCard(int index)
    {
        if(index == 1)
        {
            return new Card(fullCardList.Find(x => x.iD == "4t2"));
        }
        return new Card(fullCardList.Find(x => x.iD == "4t1"));

    }
}