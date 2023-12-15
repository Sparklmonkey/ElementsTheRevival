using UnityEngine;

public static class ElementColours
{
    private static Color EarthColour => GetColourFromHex("#94430E");

    private static Color EntropyColour => GetColourFromHex("#CA58CA");

    private static Color DarknessColour => GetColourFromHex("#292836");

    private static Color DeathColour => GetColourFromHex("#56329D");

    private static Color LifeColour => GetColourFromHex("#2BB134");

    private static Color LightColour => GetColourFromHex("#D6D6D2");

    private static Color AetherColour => GetColourFromHex("#55C6B2");

    private static Color WaterColour => GetColourFromHex("#064CCB");

    private static Color TimeColour => GetColourFromHex("#DAC13A");

    private static Color AirColour => GetColourFromHex("#79CBFF");

    private static Color GravityColour => GetColourFromHex("#F67A1B");

    private static Color FireColour => GetColourFromHex("#CB360B");

    private static Color GetColourFromHex(string hexCode)
    {
        if (ColorUtility.TryParseHtmlString(hexCode, out var colour))
        {
            return colour;
        }
        return Color.white;
    }

    public static Color GetRandomColour()
    {
        var element = (Element)Random.Range(0, 12);

        return element switch
        {
            Element.Aether => AetherColour,
            Element.Air => AirColour,
            Element.Darkness => DarknessColour,
            Element.Light => LightColour,
            Element.Death => DeathColour,
            Element.Earth => EarthColour,
            Element.Entropy => EntropyColour,
            Element.Time => TimeColour,
            Element.Fire => FireColour,
            Element.Gravity => GravityColour,
            Element.Life => LifeColour,
            Element.Water => WaterColour,
            Element.Other => AetherColour,
            _ => AetherColour,
        };
    }
    public static Color GetElementColour(Element element)
    {
        return element switch
        {
            Element.Aether => AetherColour,
            Element.Air => AirColour,
            Element.Darkness => DarknessColour,
            Element.Light => LightColour,
            Element.Death => DeathColour,
            Element.Earth => EarthColour,
            Element.Entropy => EntropyColour,
            Element.Time => TimeColour,
            Element.Fire => FireColour,
            Element.Gravity => GravityColour,
            Element.Life => LifeColour,
            Element.Water => WaterColour,
            Element.Other => GetRandomColour(),
            _ => AetherColour,
        };
    }

    public static Color32 GetWhiteColor() => new(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    public static Color32 GetBlackColor() => new(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
    public static Color32 GetInvisibleColor() => new(byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue);
}
