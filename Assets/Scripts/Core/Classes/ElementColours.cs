using UnityEngine;

public static class ElementColours
{
    private static Color earthColour => GetColourFromHex("#94430E");

    private static Color entropyColour => GetColourFromHex("#CA58CA");

    private static Color darknessColour => GetColourFromHex("#292836");

    private static Color deathColour => GetColourFromHex("#56329D");

    private static Color lifeColour => GetColourFromHex("#2BB134");

    private static Color lightColour => GetColourFromHex("#D6D6D2");

    private static Color aetherColour => GetColourFromHex("#55C6B2");

    private static Color waterColour => GetColourFromHex("#064CCB");

    private static Color timeColour => GetColourFromHex("#DAC13A");

    private static Color airColour => GetColourFromHex("#79CBFF");

    private static Color gravityColour => GetColourFromHex("#F67A1B");

    private static Color fireColour => GetColourFromHex("#CB360B");

    private static Color GetColourFromHex(string hexCode)
    {
        if (ColorUtility.TryParseHtmlString(hexCode, out var Colour))
        {
            return Colour;
        }
        return Color.white;
    }

    public static Color GetRandomColour()
    {
        Element element = (Element)Random.Range(0, 12);

        return element switch
        {
            Element.Aether => aetherColour,
            Element.Air => airColour,
            Element.Darkness => darknessColour,
            Element.Light => lightColour,
            Element.Death => deathColour,
            Element.Earth => earthColour,
            Element.Entropy => entropyColour,
            Element.Time => timeColour,
            Element.Fire => fireColour,
            Element.Gravity => gravityColour,
            Element.Life => lifeColour,
            Element.Water => waterColour,
            Element.Other => aetherColour,
            _ => aetherColour,
        };
    }
    public static Color GetElementColour(Element element)
    {
        return element switch
        {
            Element.Aether => aetherColour,
            Element.Air => airColour,
            Element.Darkness => darknessColour,
            Element.Light => lightColour,
            Element.Death => deathColour,
            Element.Earth => earthColour,
            Element.Entropy => entropyColour,
            Element.Time => timeColour,
            Element.Fire => fireColour,
            Element.Gravity => gravityColour,
            Element.Life => lifeColour,
            Element.Water => waterColour,
            Element.Other => GetRandomColour(),
            _ => aetherColour,
        };
    }

    public static Color32 GetWhiteColor() => new(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    public static Color32 GetBlackColor() => new(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
    public static Color32 GetInvisibleColor() => new(byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue);
}
