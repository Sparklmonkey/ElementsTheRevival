using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public static class ExtensionMethods
{
    private static readonly string usernameCriteria = @"^[a-zA-Z0-9.,@_-]{3,20}$";
    private static readonly string passwordCriteria = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@!#$%^&*()\-_=+{};:,<.>/?\\[\]|]).{8,30}$";
    public static bool UsernameCheck(this string username) => Regex.IsMatch(username, usernameCriteria, RegexOptions.None);
    public static bool PasswordCheck(this string password) => Regex.IsMatch(password, passwordCriteria, RegexOptions.None);

    public static bool Toggle(this bool boolean) => !boolean;
    public static OwnerEnum Not(this OwnerEnum owner)
    {
        return owner.Equals(OwnerEnum.Player) ? OwnerEnum.Opponent : OwnerEnum.Player;
    }

    public static string ToLongDescription(this ErrorCases error)
    {
        return error switch
        {
            ErrorCases.UserNameInUse =>
                "Username is already being used by someone else. Please use a different username.",
            ErrorCases.UserDoesNotExist => "Something went wrong. Please try again later.",
            ErrorCases.IncorrectPassword =>
                "Either your password or username is incorrect. Please double check and try again.",
            ErrorCases.AllGood => "Success",
            ErrorCases.UserMismatch => "Something went wrong. Please try again later.",
            ErrorCases.UnknownError => "Something went wrong. Please try again later.",
            ErrorCases.IncorrectEmail => "The email provided is incorrect. Please double check and try again.",
            ErrorCases.OtpIncorrect => "Something went wrong. Please try again later.",
            ErrorCases.OtpExpired => "Something went wrong. Please try again later.",
            ErrorCases.AccountNotVerified => "Something went wrong. Please try again later.",
            ErrorCases.AppUpdateRequired => "There is a new update available.",
            ErrorCases.ServerMaintainance => "The servers are currently under maintenance, please check in later.",
            ErrorCases.PasswordInvalid => "The entered Password does not meet the criteria.",
            ErrorCases.UsernameInvalid => "The entered Username does not meet the criteria.",
            _ => "Something went wrong. Please try again later.",
        };
    }

    public static List<string> SerializeCard(this List<Card> cardList)
    {
        var listToReturn = new List<string>();

        foreach (var card in cardList)
        {
            listToReturn.Add(card.iD);
        }

        return listToReturn;
    }

    public static int DeckCount(this string deckCode)
    {
        return deckCode.DecompressDeckCode().Count;
    }

    public static string AddDeckCard(this string deckCode, string codeToAdd)
    {
        var newDeck = deckCode.DecompressDeckCode();
        newDeck.Add(codeToAdd);
        return newDeck.CompressDeckCode();
    }

    public static List<string> GetListFromString(this string longString)
    {
        return longString.Split(" ").ToList();
    }
    public static List<Card> DeserializeCard(this List<string> cardObjectList)
    {
        var listToReturn = new List<Card>();

        foreach (var cardID in cardObjectList)
        {
            if (cardID != "" && cardID != " ")
            {
                listToReturn.Add(CardDatabase.Instance.GetCardFromId(cardID));
            }
        }
        return listToReturn;
    }

    public static List<Card> SortDeck(this List<Card> listToSort)
    {
        var sortedList = new List<Card>();

        for (var elementCheck = 12; elementCheck >= 0; elementCheck--)
        {
            var element = (Element)elementCheck;

            for (var cardTypeCheck = 0; cardTypeCheck < 6; cardTypeCheck++)
            {
                var cardType = (CardType)cardTypeCheck;

                foreach (var card in listToSort)
                {
                    if (card.costElement.Equals(element) && card.cardType.Equals(cardType))
                    {
                        sortedList.Add(card);
                    }
                }
            }
        }
        return sortedList;
    }
    
    public static bool IsFromHand(this ID id)
    {
        return id.field.Equals(FieldEnum.Hand);
    }

    [ThreadStatic] private static System.Random _local;

    private static System.Random ThisThreadsRandom => _local ??= new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId));

    public static void OrderSprites(this List<Sprite> listToOrder)
    {
        var orderedList = new List<Sprite>(listToOrder);
        listToOrder = new List<Sprite>();
        for (var i = 0; i < orderedList.Count; i++)
        {
            for (var x = 0; x < orderedList.Count; x++)
            {
                if (orderedList[x].name == i.ToString("00"))
                {
                    listToOrder.Add(orderedList[x]);
                }
            }
        }
    }
    public static T GetScriptFromName<T>(this string scriptName)
    {
        var type = Type.GetType(scriptName);
        if (type == null)
        {
            return default;
        }
        var obj = (T)Activator.CreateInstance(type);
        return obj;
    }

    public static T GetSkillScript<T>(this string abilityName)
    {
        var nameToCheck = abilityName[0].ToString().ToUpper() + abilityName[1..];
        var type = Type.GetType(nameToCheck);
        if (type == null)
        {
            return default;
        }
        var obj = (T)Activator.CreateInstance(type);
        return obj;
    }

    public static T GetShieldScript<T>(this string abilityName)
    {
        var nameToCheck = $"Shield{abilityName}";
        var type = Type.GetType(nameToCheck);
        if (type == null)
        {
            return default;
        }
        var obj = (T)Activator.CreateInstance(type);
        return obj;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = ThisThreadsRandom.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static string FastElementString(this Element element)
    {
        return element switch
        {
            Element.Aether => nameof(Element.Aether),
            Element.Air => nameof(Element.Air),
            Element.Darkness => nameof(Element.Darkness),
            Element.Light => nameof(Element.Light),
            Element.Death => nameof(Element.Death),
            Element.Earth => nameof(Element.Earth),
            Element.Entropy => nameof(Element.Entropy),
            Element.Time => nameof(Element.Time),
            Element.Fire => nameof(Element.Fire),
            Element.Gravity => nameof(Element.Gravity),
            Element.Life => nameof(Element.Life),
            Element.Water => nameof(Element.Water),
            Element.Other => nameof(Element.Other),
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null),
        };
    }

    public static string FastCardTypeString(this CardType cardType)
    {
        return cardType switch
        {
            CardType.Pillar => nameof(CardType.Pillar),
            CardType.Creature => nameof(CardType.Creature),
            CardType.Spell => nameof(CardType.Spell),
            CardType.Artifact => nameof(CardType.Artifact),
            CardType.Weapon => nameof(CardType.Weapon),
            CardType.Shield => nameof(CardType.Shield),
            CardType.Mark => nameof(CardType.Mark),
            _ => throw new ArgumentOutOfRangeException(nameof(cardType), cardType, null),
        };
    }

    public static CardType GetSerendipityWeighted()
    {
        var weights = new List<int> { 40, 30, 10, 10, 5, 5 }; // Creature > Spell > Pillar > Artifact > Weapon > Shield
        var cardTypes = new List<CardType> { CardType.Creature, CardType.Spell, CardType.Pillar, CardType.Artifact, CardType.Weapon, CardType.Shield };
        var rndValue = UnityEngine.Random.Range(0, 100);

        for (var i = 0; i < weights.Count; i++)
        {
            rndValue -= weights[i];
            if (rndValue <= 0) { return cardTypes[i]; }
        }
        return CardType.Pillar;
    }

    public static string GetUppedRegular(this string cardID)
    {
        var cardValue = cardID.Base32ToInt();
        if (cardValue > 7000)
        {
            return (cardValue - 2000).IntToBase32();
        }
        return (cardValue + 2000).IntToBase32();
    }

    public static int GetRegularBuyPrice(this string cardID)
    {
        var regCard = CardDatabase.Instance.GetCardFromId(cardID.GetUppedRegular());
        return regCard.rarity * regCard.rarity * 6 + regCard.cost;
    }

    public static bool IsBazaarLegal(this string cardID)
    {
        if (CardDatabase.Instance.MarkIds.Contains(cardID))
        {
            return false;
        }
        if ((PlayerData.Shared.currentQuestIndex < 7 || PlayerPrefs.GetFloat("ShouldShowRareCard") == 1) && cardID.IsUpgraded())
        {
            return false;
        }
        var uppedRegId = cardID.GetUppedRegular();
        return !bazaarIllegalIds.Contains(cardID) && !bazaarIllegalIds.Contains(uppedRegId);
    }

    public static bool IsDeckLegal(this string cardId)
    {
        if (CardDatabase.Instance.MarkIds.Contains(cardId))
        {
            return false;
        }
        return !bazaarIllegalIds.Contains(cardId);
    }

    private static readonly List<string> bazaarIllegalIds = new() { "4sj", "4sk", "4sl", "4sm", "4sn", "4so", "4sp", "4sq", "4sr", "4st", "8pu", "4t8", "4vr", "4t1", "4t2", "8pu", "8pr", "8pt", "8pq", "8pk", "8pm", "8pj", "8ps", "8po", "8pl", "8pn", "8pp" };


    public static string ConvertLegacyToOetg(this string legacyCode)
    {
        var returnString = "0";
        var legacyList = legacyCode.Split(" ");
        var cardDict = new Dictionary<string, int>();
        var markId = "";
        foreach (var item in legacyList)
        {
            if (item == " ") { continue; }
            if (item == "") { continue; }
            if (CardDatabase.Instance.MarkIds.Contains(item)) { markId = item; continue; }
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
            var cardId = (item.Key.Base32ToInt() - 4000).IntToBase32().ToString();
            var stringId = cardId.Length == 2 ? $"0{cardId}" : cardId;
            returnString += $"{countString}{stringId}0";
        }
        return returnString + $"1{markId}";
    }

    public static string ConvertOetgToLegacy(this string oetgCode)
    {
        var returnStr = new List<string>();
        var deckSeparated = new List<string>(ChunksUpto(oetgCode, 5));
        var mark = deckSeparated[^1][1..];
        deckSeparated.RemoveAt(deckSeparated.Count - 1);

        foreach (var oCard in deckSeparated)
        {
            var cardCount = oCard[0].ToString().Base32ToInt();
            var legacyId = (oCard[1..].Base32ToInt() + 4000).IntToBase32();
            for (var i = 0; i < cardCount; i++)
            {
                returnStr.Add(legacyId);
            }
        }

        returnStr.Add(mark);
        return string.Join(" ", returnStr);
    }

    private static List<string> ChunksUpto(string str, int maxChunkSize)
    {
        var returnStr = new List<string>();
        for (var i = 0; i < str.Length; i += maxChunkSize)
        {
            returnStr.Add(str.Substring(i, Math.Min(maxChunkSize, str.Length - i))[1..]);
        }

        return returnStr;
    }

    public static bool IsUpgraded(this string cardID)
    {
        var cardValue = cardID.Base32ToInt();
        return cardValue > 7000;
    }

    public static bool IsRare(this Card card)
    {
        return card.rarity is 6 or 8 or 15 or 18 or 20;
    }

    public const string AcceptedCharacters = "ybndrfg8ejkmcpqxot1uwisza345h769";

    public static string Encode(this int input)
    {
        var result = "";

        if (input == 0)
        {
            result += AcceptedCharacters[0];
        }
        else
        {
            while (input > 0)
            {
                //Must make sure result is in the correct order
                result = AcceptedCharacters[input % AcceptedCharacters.Length] + result;
                input /= AcceptedCharacters.Length;
            }
        }

        return result;
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
            value = Mathf.FloorToInt(value / 32);
            result += alphabet[index].ToString();
        }
        var charArray = result.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static int Base32ToInt(this string base32Number)
    {
        var base32 = new char[] {
      '0','1','2','3','4','5','6','7',
      '8','9','a','b','c','d','e','f',
      'g','h','i','j','k','l','m','n',
      'o','p','q','r','s','t','u','v'};

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



public static class DeckCodeExtension
{
    public static string CompressDeckCode(this string deckList)
    {
        Dictionary<string, int> cardDict = new();
        var intList = "";
        List<string> cardList = new(deckList.Split(" "));

        foreach (var item in cardList)
        {
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
            string cardCount = item.Value.IntToBase32();
            if (cardCount.Length == 1)
            {
                cardCount = $"0{cardCount}";
            }
            intList += $"{cardCount}{item.Key}";
        }
        return intList;
    }
    public static string CompressDeckCode(this List<string> deckList)
    {
        Dictionary<string, int> cardDict = new();
        var intList = "";

        foreach (var item in deckList)
        {
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
            string cardCount = item.Value.IntToBase32();
            if (cardCount.Length == 1)
            {
                cardCount = $"0{cardCount}";
            }
            intList += $"{cardCount}{item.Key}";
        }
        return intList.Remove(0);
    }

    public static List<string> DecompressDeckCode(this string compressedDeck)
    {
        List<string> returnList = new();
        var list = new List<string>();
        var i = 0;
        while (i < compressedDeck.Length)
        {
            var subLength = Math.Min(compressedDeck.Length - i, 5);
            var item = compressedDeck.Substring(i, subLength);
            list.Add(item);
            i += 5;
        }
        foreach (var item in list)
        {

            if (item == "") { continue; }
            var cardCode = item[^3..];
            int cardCount = item.Replace(cardCode, "").Base32ToInt();
            for (var c = 0; c < cardCount; c++)
            {
                returnList.Add(cardCode);
            }
        }

        return returnList;
    }
}