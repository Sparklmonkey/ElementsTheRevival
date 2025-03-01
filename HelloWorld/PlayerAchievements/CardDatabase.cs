using System;
using System.Collections.Generic;

namespace PlayerAchievements;

public class CardDatabase
{
    private readonly List<string> _darknessCards = new()
    {
        "5uk", "5ul", "5um", "5un", "5uo", "5up", "5uq", "5ur", "5us", "5ut", "5uu", "5uv", "5v0", "5v1", "5v2",
        "5v8", "5vi", "606", "7t4", "7t5", "7t6", "7t7", "7t8", "7t9", "7ta", "7tb", "7tc", "7td", "7te", "7tf",
        "7tg", "7th", "7ti", "7to", "7u2", "7um"
    };
    
    private readonly List<string> _aetherCards = new()
    {
        "61o", "61p", "61q", "61r", "61s", "61t", "61u", "61v", "620", "621", "622", "623", "624", "625", "626",
        "62c", "62m", "63a", "808", "809", "80a", "80b", "80c", "80d", "80e", "80f", "80g", "80h", "80i", "80j",
        "80k", "80l", "80m", "80s", "816", "81q"
    };
    
    private readonly List<string> _timeCards = new()
    { 
        "5rg", "5rh", "5ri", "5rj", "5rk", "5rl", "5rm", "5rn", "5ro", "5rp", "5rq", "5rr", "5rs", "5rt", "5ru",
        "5s4", "5se", "5t2", "7q0", "7q1", "7q2", "7q3", "7q4", "7q5", "7q6", "7q7", "7q8", "7q9", "7qa", "7qb",
        "7qc", "7qd", "7qe", "7qk", "7qu", "7ri"
    };
    
    private readonly List<string> _airCards = new()
    { 
        "5oc", "5od", "5oe", "5of", "5og", "5oh", "5oi", "5oj", "5ok", "5ol", "5om", "5on", "5oo", "5op", "5p0",
        "5pa", "5pu", "7ms", "7mt", "7mu", "7mv", "7n0", "7n1", "7n2", "7n3", "7n4", "7n5", "7n6", "7n7", "7n8",
        "7n9", "7ng", "7nq", "7oe"
    };
    
    private readonly List<string> _lightCards = new()
    { 
        "5l8", "5l9", "5la", "5lb", "5lc", "5ld", "5le", "5lf", "5lg", "5lh", "5li", "5lj", "5lk", "5ll", "5lm",
        "5ls", "5m6", "5mq", "7jo", "7jp", "7jq", "7jr", "7js", "7jt", "7ju", "7jv", "7k0", "7k1", "7k2", "7k3",
        "7k4", "7k5", "7k6", "7kc", "7km", "7la"
    };
    
    private readonly List<string> _waterCards = new()
    { 
        "5i4", "5i5", "5i6", "5i7", "5i8", "5i9", "5ia", "5ib", "5ic", "5id", "5ie", "5if", "5ig", "5ih", "5ii",
        "5ij", "5io", "5j2", "5jm", "7gk", "7gl", "7gm", "7gn", "7go", "7gp", "7gq", "7gr", "7gs", "7gt", "7gu",
        "7gv", "7h0", "7h1", "7h2", "7h3", "7h8", "7hi", "7i6"
    };
    
    private readonly List<string> _fireCards = new()
    { 
        "5f0", "5f1", "5f2", "5f3", "5f4", "5f5", "5f6", "5f7", "5f8", "5f9", "5fa", "5fb", "5fc", "5fe", "5fk", 
        "5fu", "5gi", "7dg", "7dh", "7di", "7dj", "7dk", "7dl", "7dm", "7dn", "7do", "7dp", "7dq", "7dr", "7ds", 
        "7du", "7e4", "7ee", "7f2"
    };
    
    private readonly List<string> _lifeCards = new()
    { 
        "5bs", "5bt", "5bu", "5bv", "5c0", "5c1", "5c2", "5c3", "5c4", "5c5", "5c6", "5c7", "5c8", "5c9", "5cg", 
        "5cq", "5de", "7ac", "7ad", "7ae", "7af", "7ag", "7ah", "7ai", "7aj", "7ak", "7al", "7am", "7an", "7ao", 
        "7ap", "7b0", "7ba", "7bu"
    };
    
    private readonly List<string> _earthCards = new()
    { 
        "58o", "58p", "58q", "58r", "58s", "58t", "58u", "58v", "590", "591", "592", "593", "594", "595", "596", 
        "59c", "59m", "5aa", "778", "779", "77a", "77b", "77c", "77d", "77e", "77f", "77g", "77h", "77i", "77j",
        "77k", "77l", "77m", "77s", "786", "78q"
    };
    
    private readonly List<string> _gravityCards = new()
    { 
        "55k", "55l", "55m", "55n", "55o", "55p", "55q", "55r", "55s", "55t", "55u", "55v", "560", "561", "562", 
        "563", "568", "56i", "576", "744", "745", "746", "747", "748", "749", "74a", "74b", "74c", "74d", "74e", 
        "74f", "74g", "74h", "74i", "74j", "74o", "752", "75m"
    };
    
    private readonly List<string> _deathCards = new()
    { 
        "52g", "52h", "52i", "52j", "52k", "52l", "52m", "52n", "52o", "52p", "52q", "52r", "52s", "52t", "52u", 
        "52v", "534", "53e", "542", "710", "711", "712", "713", "714", "715", "716", "717", "718", "719", "71a", 
        "71b", "71c", "71d", "71e", "71f", "71k", "71u", "72i"
    };
    
    private readonly List<string> _entropyCards = new()
    { 
        "4vc", "4vd", "4ve", "4vf", "4vg", "4vh", "4vi", "4vj", "4vk", "4vl", "4vm", "4vn", "4vo", "4vp", "4vq", 
        "500", "50a", "50u", "6ts", "6tt", "6tu", "6tv", "6u0", "6u1", "6u2", "6u3", "6u4", "6u5", "6u6", "6u7", 
        "6u8", "6u9", "6ua", "6ug", "6uq", "6ve"
    };
    
    private readonly List<string> _otherCards = new()
    { 
        "4sa", "4t3", "4t4", "4t5", "4tb", "4tc", "6qq", "6rj", "6rk", "6rl", "6rr", "6rs"
    };

    public List<string> GetCardList(Element element)
    {
        return element switch
        {
            Element.Aether => _aetherCards,
            Element.Air => _airCards,
            Element.Darkness => _darknessCards,
            Element.Light => _lightCards,
            Element.Death => _deathCards,
            Element.Earth => _earthCards,
            Element.Entropy => _entropyCards,
            Element.Time => _timeCards,
            Element.Fire => _fireCards,
            Element.Gravity => _gravityCards,
            Element.Life => _lifeCards,
            Element.Water => _waterCards,
            Element.Other => _otherCards,
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
        };
    }
    
    public int HasCardSingleCollection(List<string> fullCardList, Element elementToCheck)
    {
        var singletonList = fullCardList.GetAllCardsSingleton();
        var cardList = GetCardList(elementToCheck);
        var cardsThatMatter = singletonList.CompareWith(cardList);
        var completionPercent = Convert.ToInt32(Convert.ToSingle(cardsThatMatter.Count) / Convert.ToSingle(cardList.Count) * 100);
        
        return completionPercent;
    }
    
    public int HasCardHalfCollection(List<string> fullCardList, Element elementToCheck)
    {
        var tripleList = fullCardList.GetAllCardsTriple();
        var cardList = GetCardList(elementToCheck);
        var cardsThatMatter = tripleList.CompareWith(cardList);
        var completionPercent = Convert.ToInt32(Convert.ToSingle(cardsThatMatter.Count) / Convert.ToSingle(cardList.Count) * 100);
        
        return completionPercent;
    }
    
    public int HasCardCompleteCollection(List<string> fullCardList, Element elementToCheck)
    {
        var sixList = fullCardList.GetAllCardsFull();
        var cardList = GetCardList(elementToCheck);
        var cardsThatMatter = sixList.CompareWith(cardList);
        var completionPercent = Convert.ToInt32(Convert.ToSingle(cardsThatMatter.Count) / Convert.ToSingle(cardList.Count) * 100);
        
        return completionPercent;
    }
}

public enum Element
{
    Aether,
    Air,
    Darkness,
    Light,
    Death,
    Earth,
    Entropy,
    Time,
    Fire,
    Gravity,
    Life,
    Water,
    Other
}