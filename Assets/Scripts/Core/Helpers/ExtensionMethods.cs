using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class ExtensionMethods
{
    public static List<CardObject> SerializeCard(this List<Card> cardList)
    {
        List<CardObject> listToReturn = new List<CardObject>();

        foreach (Card card in cardList)
        {
            listToReturn.Add(new CardObject(card.name, card.type.ToString(), !card.isUpgradable));
        }

        return listToReturn;
    }

    public static List<Card> DeserializeCard(this List<CardObject> cardObjectList)
    {
        List<Card> listToReturn = new List<Card>();

        foreach (CardObject cardObject in cardObjectList)
        {
            listToReturn.Add(CardDatabase.GetCardFromResources(cardObject));
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
                    if (card.element.Equals(element) && card.type.Equals(cardType))
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

}

