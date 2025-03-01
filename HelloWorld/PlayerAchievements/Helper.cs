using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerAchievements;

public static class Helper
{
    private static List<string> _validIdList = new()
    {
        "71a", "6rj", "6u5", "7n5", "80e", "7q8", "7dn", "6rl", "7al", "6rk", "6rr", "7k1", "7gs",
        "77f", "74c", "7tb", "52q", "4t3", "4vl", "5c5", "5ro", "5f7", "4t5", "61u", "5lh", "5ol", "58v", "4tb", "4t4",
        "55s", "5ic", "5ur", "4t2", "562", "5c7", "52s",
        "4vn", "595", "55v", "5lf", "4vo", "4vi", "5f6", "5us", "593", "592", "5f4", "5oi", "622", "5i7", "77k", "55t",
        "5c2", "5lc", "5i8", "5f9", "61q", "5uu", "5lj",
        "5li", "5c9", "55q", "4vk", "5v1", "4vj", "5ig", "4vp", "61r", "52p", "58t", "52o", "5rr", "5ia", "621", "5fb",
        "5f8", "5rk", "5fu", "5m6", "5se", "53e", "50a",
        "62m", "5on", "624", "5op", "5up", "594", "5oh", "71c", "7n2", "77l", "74f", "6u8", "6u2", "7gn", "7dp", "718",
        "7an", "7dm", "7dk", "7do", "80i", "74d", "77d",
        "7js", "7go", "6u7", "7jv", "7ai", "7k2", "6u4", "719", "7t9", "7n1", "7te", "7k3", "7ap", "7th", "7h0", "74i",
        "6u9", "7qb", "77i", "7gq", "77j", "80h", "7dr",
        "7q4", "7ee", "7km", "7qu", "71u", "6uq", "816", "7n7", "80k", "7tc", "7n9", "6u3", "80a", "80b", "74a", "52r",
        "61t", "4vg", "5uo", "5c4", "5f5", "5og", "55p",
        "5lk", "5i9", "5rj", "5lg", "4t1", "4tc", "52l", "5ld", "5c3", "58s", "5oo", "71b", "77c", "6u0", "7dl", "749",
        "7k4", "7t8", "7n0", "7ak", "7k0", "7gp", "80d",
        "715", "7jt", "7aj", "6rs", "7q3", "7n8", "61o", "4vc", "52g", "5f0", "5bs", "55k", "5l8", "5uk", "4sa", "58o",
        "5rg", "5i4", "5oc", "808", "6ts", "710", "7dg",
        "7ac", "744", "7jo", "7t4", "6qq", "778", "7q0", "7gk", "7ms", "63a", "5pu", "606", "542", "5aa", "50u", "5gi",
        "576", "5de", "5mq", "5t2", "5jm", "81q", "7oe",
        "7um", "72i", "78q", "6ve", "7f2", "75m", "7bu", "7la", "7ri", "7i6", "8pu", "8pr", "8pt", "8pk", "8pm", "8pj",
        "8po", "8pl", "8pn", "8pq", "8ps", "8pp", "4ve",
        "568", "58p", "5rn", "5ib", "55m", "5f1", "5fd", "59c", "5of", "5ul", "5v8", "5i6", "5p0", "52h", "560", "5i5",
        "5c0", "55o", "5f2", "5ll", "52u", "5rh", "5rm",
        "5um", "5od", "5rt", "5bt", "4vm", "5ri", "5f3", "5oj", "5ok", "52j", "5c8", "5c1", "5uv", "5ru", "58u", "5la",
        "5s4", "590", "55u", "55n", "534", "5le", "58q",
        "5lt", "5bu", "5id", "61s", "596", "5fa", "4vh", "4vd", "5if", "5ut", "52t", "5io", "55r", "5un", "5lb", "5rs",
        "61v", "620", "5fc", "5l9", "625", "4vf",
        "500", "5fk", "5bv", "55l", "5rq", "4vq", "5fe", "597", "56i", "59m", "591", "52m", "61p", "5ii", "58r", "5ie",
        "62c", "52i", "5v0", "52k", "5ls", "5oe",
        "7gm", "80s", "7ng", "6tv", "7ju", "7gt", "7gr", "7dt", "809", "77b", "7t7", "7dh", "74g", "714", "7k5", "7mt",
        "7to", "71k", "71e", "7qd", "77s", "779", "7q7",
        "746", "745", "7ag", "7q1", "7n4", "77g", "80c", "71d", "74b", "7jr", "80f", "7n3", "7qa", "77h", "716", "7mu",
        "6u6", "7q2", "7e4", "7dj", "713", "7ah", "7tf",
        "7qe", "7ae", "77e", "7qk", "74e", "747", "74o", "7kd", "711", "7ad", "7dq", "7af", "7b0", "7jq", "7kc", "748",
        "6tt", "6tu", "7ds", "7t5", "7t6", "7qc",
        "80g", "7gl", "80l", "7gu", "6ug", "7jp", "712", "7di", "6ua", "7ao", "7du", "752", "786", "7q6", "7mv", "7h2",
        "77a", "7gv", "7td", "77m", "7tg", "7h8",
        "6u1", "52n", "561", "5v2", "5c6", "5ih", "5rl", "623", "5uq", "5lm", "5pa", "5cq", "5j2", "5vi", "52v", "5rp",
        "5om", "7ti", "7ta", "7q5", "7am", "717", "7h1",
        "80j", "7k6", "7nq", "7ba", "7hi", "7u2", "71f", "7q9", "74h", "7n6"
    };

    public static T GetScript<T>(this string scriptName, string category)
    {
        string objectToInstantiate = $"PlayerAchievements.AchievementScripts.Achievements.{category}.{scriptName}, PlayerAchievements";
        var type = Type.GetType(objectToInstantiate);
        if (type == null)
        {
            return default;
        }
        var obj = (T)Activator.CreateInstance(type);
        return obj;
    }
    private static bool IsCardIdValid(this string cardId)
    {
        return _validIdList.Contains(cardId);
    }

    public static List<string> GetAllCardsSingleton(this List<string> fullCardList)
    {
        var singletonCards = fullCardList.Distinct().ToList();
        return singletonCards;
    }

    public static List<string> GetAllCardsTriple(this List<string> fullCardList)
    {
        var singletonCards = fullCardList.Distinct().ToList();

        return singletonCards.Where(cardId => fullCardList.Count(x => x == cardId) >= 3).Distinct().ToList();
    }

    public static List<string> GetAllCardsFull(this List<string> fullCardList)
    {
        var singletonCards = fullCardList.Distinct().ToList();

        return singletonCards.Where(cardId => fullCardList.Count(x => x == cardId) >= 6).Distinct().ToList();
    }

    public static List<string> CompareWith(this List<string> cardList, List<string> elementList)
    {
        return cardList.Intersect(elementList).ToList();
    }

    public static List<string> DeserializeDeck(this string arenaDeck)
    {
        var returnStr = new List<string>();
        var deckSeparated = arenaDeck.Split("X");

        foreach (var oCard in deckSeparated)
        {
            if (oCard is "") continue;
            var cardCode = oCard[^3..];
            var cardCount = oCard[..^3].Base32ToInt();
            var legacyId = (cardCode.Base32ToInt() + 4000).IntToBase32();
            if (!legacyId.IsCardIdValid()) continue;
            for (var i = 0; i < cardCount; i++)
            {
                returnStr.Add(legacyId);
            }
        }

        return returnStr;
    }

    public static string SerializeDeck(this List<string> deckList)
    {
        var returnString = "X";
        var cardDict = new Dictionary<string, int>();

        foreach (var item in deckList)
        {
            if (item is " " or "" or "Card" or "ID" or "Card ID") continue;
            if (item is "4t8" or "6ro" or "6ub" or "4vr") continue;
            if (!item.IsCardIdValid()) continue;
            if (cardDict.ContainsKey(item))
            {
                cardDict[item]++;
            }
            else
            {
                cardDict.Add(item, 1);
            }
        }

        foreach (var item in cardDict)
        {
            var countString = item.Value.IntToBase32();
            var cardId = (item.Key.Base32ToInt() - 4000).IntToBase32();
            var stringId = cardId;
            returnString += $"{countString}{stringId}X";
        }

        return returnString;
    }

    public static string IntToBase32(this int value)
    {
        var alphabet = "0123456789abcdefghijklmnopqrstuv".ToCharArray();
        var result = "";

        if (value < 32)
        {
            return alphabet[value].ToString();
        }

        long index;
        while (value != 0)
        {
            index = value % 32;
            value /= 32;
            result += alphabet[index].ToString();
        }

        var charArray = result.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static int Base32ToInt(this string base32Number)
    {
        var base32 = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', 'a', 'b', 'c', 'd', 'e', 'f',
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v'
        };

        long n = 0;

        foreach (var d in base32Number.ToLowerInvariant())
        {
            n = n << 5;
            var idx = Array.IndexOf(base32, d);

            if (idx == -1)
                throw new Exception("Provided number contains invalid characters");

            n += idx;
        }

        return (int)n;
    }
}