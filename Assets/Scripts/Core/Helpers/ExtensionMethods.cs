using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class ExtensionMethods
{


    public static List<string> SerializeCard(this List<Card> cardList)
    {
        List<string> listToReturn = new List<string>();

        foreach (Card card in cardList)
        {
            listToReturn.Add(card.iD);
        }

        return listToReturn;
    }

    public static List<Card> DeserializeCard(this List<string> cardObjectList)
    {
        List<Card> listToReturn = new List<Card>();

        if (cardObjectList.Count > 0)
        {
            foreach (string cardID in cardObjectList)
            {
                if (cardID != "" && cardID != " ")
                {
                    listToReturn.Add(CardDatabase.GetCardFromId(cardID));
                }
            }
        }

        return listToReturn;
    }

    public static List<Card> SortDeck(this List<Card> listToSort)
    {
        List<Card> sortedList = new List<Card>();

        for (int elementCheck = 12; elementCheck >= 0; elementCheck--)
        {
            Element element = (Element)elementCheck;

            for (int cardTypeCheck = 0; cardTypeCheck < 6; cardTypeCheck++)
            {
                CardType cardType = (CardType)cardTypeCheck;

                foreach (Card card in listToSort)
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

    public static int GetFullQuantaCount(this List<QuantaObject> quantaObjects)
    {
        int count = 0;

        foreach (QuantaObject item in quantaObjects)
        {
            count += item.count;
        }
        return count;
    }

    public static int GetIntTotal(this List<int> list)
    {
        int count = 0;

        foreach (int item in list)
        {
            count += item;
        }

        return count;
    }

    [ThreadStatic] private static System.Random Local;

    public static System.Random ThisThreadsRandom
    {
        get { return Local ??= new System.Random(unchecked((Environment.TickCount * 31) + Thread.CurrentThread.ManagedThreadId)); }
    }

    public static void OrderSprites(this List<Sprite> listToOrder)
    {
        List<Sprite> orderedList = new List<Sprite>(listToOrder);
        listToOrder = new List<Sprite>();
        for (int i = 0; i < orderedList.Count; i++)
        {
            for (int x = 0; x < orderedList.Count; x++)
            {
                if(orderedList[x].name == i.ToString("00"))
                {
                    listToOrder.Add(orderedList[x]);
                }
            }
        }
    }
    public static T GetScriptFromName<T>(this string AbilityName)
    {
        Type type = Type.GetType(AbilityName);
        if (type == null)
        {
            //Debug.Log("Active Ability Doesnt exist");
            return default(T);
        }
        T obj = (T)Activator.CreateInstance(type);
        //Debug.Log(obj);
        return obj;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
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

    public static string RemovePassivePrefix(this string input)
    {
        input = input.Replace("is", "");
        input = input.Replace("will", "");
        input = input.Replace("has", "");
        return input;
    }
    public static int? ContainsElement(this List<QuantaObject> listToCheck, Element element)
    {
        if(listToCheck.Count == 0)
        {
            return null;
        }
        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i].element.Equals(element))
            {
                return i;
            }
        }
        return null;
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
        List<int> weights = new List<int> { 40, 30, 10, 10, 5, 5 }; // Creature > Spell > Pillar > Artifact > Weapon > Shield
        List<CardType> cardTypes = new List<CardType> { CardType.Creature, CardType.Spell, CardType.Pillar, CardType.Artifact, CardType.Weapon, CardType.Shield };
        int rndValue = UnityEngine.Random.Range(0, 100);

        for (int i = 0; i < weights.Count; i++)
        {
            rndValue -= weights[i];
            if(rndValue <= 0) { return cardTypes[i]; }
        }
        return CardType.Pillar;
    }

    public static string GetUppedRegular(this string cardID)
    {
        int cardValue = cardID.Base32ToInt();
        if (cardValue > 7000)
        {
            return (cardValue - 2000).IntToBase32();
        }
        return (cardValue + 2000).IntToBase32();
    }

    public static bool IsUpgraded(this string cardID)
    {
        int cardValue = cardID.Base32ToInt();
        return cardValue > 7000;
    }

    public static bool IsRare(this Card card)
    {
        return (card.rarity == 6 || card.rarity == 8 || card.rarity == 15 || card.rarity == 18 || card.rarity == 20);
    }

    public const string AcceptedCharacters = "ybndrfg8ejkmcpqxot1uwisza345h769";

    public static string Encode(this int input)
    {
        string result = "";

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
        char[] alphabet = "0123456789abcdefghijklmnopqrstuv".ToCharArray();
        string result = "";

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
        char[] charArray = result.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static int Base32ToInt(this string base32number)
    {
        char[] base32 = new char[] {
      '0','1','2','3','4','5','6','7',
      '8','9','a','b','c','d','e','f',
      'g','h','i','j','k','l','m','n',
      'o','p','q','r','s','t','u','v'};

        long n = 0;

        foreach (char d in base32number.ToLowerInvariant())
        {
            n = n << 5;
            int idx = Array.IndexOf(base32, d);

            if (idx == -1)
                throw new Exception("Provided number contains invalid characters");

            n += idx;
        }

        return (int)n;
    }
    
}

