using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Battlefield.Abilities;
using Battlefield.Abilities.Weapon;
using Core.Classes;
using UnityEngine;
using UnityEngine.Serialization;


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

    public List<string> markIds = new() { "8pu", "8pr", "8pt", "8pq", "8pk", "8pm", "8pj", "8ps", "8po", "8pl", "8pn", "8pp" };
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

    public Card GetRandomCardOfTypeWithElement(CardType type, Element element, bool shouldBeUpgraded)
    {
        var reducedList = FullCardList.FindAll(x => x.CostElement.Equals(element)
                                                    && x.Type.Equals(type)
                                                    && !x.CardName.Contains("Shard of")
                                                    && !x.CardName.Contains(" Nymph")
                                                    && x.Id.IsUpgraded() == shouldBeUpgraded
                                                    && !_illegalHatchCards.Contains(x.Id));
        if (reducedList.Count <= 0)
            return GetRandomCardOfTypeWithElement((CardType)Random.Range(0, 6), element, shouldBeUpgraded);
        var card = reducedList[Random.Range(0, reducedList.Count)];
        var cardToReturn = card.Clone();
        return cardToReturn;
    }

    public List<Card> GetHalfBloodDeck(Element primary, Element secondary) => _deckBuilder.GetHalfBloodDeck(primary, secondary);

    public List<string> GetRandomDeck() => _deckBuilder.GetRandomDeck();

    public Card GetMutant(bool isUpgraded, Card fromCard = null) => MutantHelper.GetMutant(isUpgraded,
        fromCard == null ? GetRandomCard(CardType.Creature, isUpgraded, true) : fromCard);

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

    public Card GetGolem(List<(ID id, Card card)> shardList) =>
        GolemHelper.GetGolemAbility(GetCardFromId("597"), shardList);

    internal Card GetPlaceholderCard(int index)
    {
        if (index == 1)
        {
            return GetCardFromId("4t2");
        }
        return GetCardFromId("4t1");

    }

    public Card GetUnuppedAlt(string id)
    {
        var idToGet = id.GetUppedRegular();
        return GetCardFromId(idToGet);
    }
}