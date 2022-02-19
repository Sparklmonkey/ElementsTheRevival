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
        Element element = (Element)Random.Range(0,12);

        switch (element)
        {
            case Element.Aether:
                return aetherColour;
            case Element.Air:
                return airColour;
            case Element.Darkness:
                return darknessColour;
            case Element.Light:
                return lightColour;
            case Element.Death:
                return deathColour;
            case Element.Earth:
                return earthColour;
            case Element.Entropy:
                return entropyColour;
            case Element.Time:
                return timeColour;
            case Element.Fire:
                return fireColour;
            case Element.Gravity:
                return gravityColour;
            case Element.Life:
                return lifeColour;
            case Element.Water:
                return waterColour;
            case Element.Other:
                return aetherColour;
            default:
                return aetherColour;
        }

        
    }
    public static Color GetElementColour(Element element)
    {
        switch (element)
        {
            case Element.Aether:
                return aetherColour;
            case Element.Air:
                return airColour;
            case Element.Darkness:
                return darknessColour;
            case Element.Light:
                return lightColour;
            case Element.Death:
                return deathColour;
            case Element.Earth:
                return earthColour;
            case Element.Entropy:
                return entropyColour;
            case Element.Time:
                return timeColour;
            case Element.Fire:
                return fireColour;
            case Element.Gravity:
                return gravityColour;
            case Element.Life:
                return lifeColour;
            case Element.Water:
                return waterColour;
            case Element.Other:
                return GetRandomColour();
            default:
                return aetherColour;
        }
    }
}
