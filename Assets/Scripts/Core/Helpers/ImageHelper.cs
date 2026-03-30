using UnityEngine;

public static class ImageHelper
{
    private static string IsAltOrOriginal()
    {
        if (PlayerPrefs.GetInt("IsAltArt") == 1)
        {
            return "Alt";
        }
        return "Original";
    }

    public static Sprite GetCardHeadBackground(string element)
    {
        return Resources.Load<Sprite>("Sprites/CardHeadBackground/" + element);
    }
    public static Sprite GetCardImage(string imageID)
    {
        var path = "Sprites/CardImages/" + imageID;
        return Resources.Load<Sprite>(path);
    }

    public static Sprite GetPendulumImage(string mainElement, string mElement)
    {
        var path = "Sprites/CardImages/Pendulum/" + mainElement + "/" + mElement;
        return Resources.Load<Sprite>(path);
    }

    public static Sprite GetElementImage(string element)
    {
        return Resources.Load<Sprite>($"Sprites/Elements/{IsAltOrOriginal()}/" + element);
    }

    public static Sprite GetCardBackGroundImage(string element)
    {
        return Resources.Load<Sprite>("Sprites/CardBackground/" + element);
    }

    public static Sprite GetCardTypeImage(string type)
    {
        return Resources.Load<Sprite>("Sprites/CardTypes/" + type);
    }
    public static Sprite GetCardBackImage()
    {
        return Resources.Load<Sprite>("Sprites/CardBack");
    }

    public static Sprite GetCreatureEffectIndicator(string effect)
    {
        return Resources.Load<Sprite>("Sprites/Effects/" + effect);
    }

    public static Sprite GetPoisonSprite(bool isPoison)
    {
        return Resources.Load<Sprite>($"Sprites/PoisonTypes/{IsAltOrOriginal()}/" + (isPoison ? "Death" : "Water"));
    }

    public static Sprite GetAchievementFrame(int achievementDataRarity)
    {
        var rarity = achievementDataRarity == 2 ? "Hard" : achievementDataRarity == 1 ? "Medium" : "Normal";
        return Resources.Load<Sprite>($"Sprites/AchievementFrame/" + rarity);
    }
    
    public static Color32 HexToColor32(string hex)
    {
        // Remove # if present
        hex = hex.Replace("#", "");
        
        // Parse the hex values
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = hex.Length >= 8 ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;
    
        return new Color32(r, g, b, a);
    }
}
