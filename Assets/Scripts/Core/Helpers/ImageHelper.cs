using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageHelper
{
	private static string IsAltOROriginal()
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
		string path = "Sprites/CardImages/" + imageID;
		return Resources.Load<Sprite>(path);
	}

	public static Sprite GetPendulumImage(string mainElement, string mElement)
	{
		string path = "Sprites/CardImages/Pendulum/" + mainElement + "/" + mElement;
		return Resources.Load<Sprite>(path);
	}

	public static Sprite GetElementImage(string element)
	{
		return Resources.Load<Sprite>($"Sprites/Elements/{IsAltOROriginal()}/" + element);
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
		return Resources.Load<Sprite>("Sprites/PoisonTypes/" + (isPoison ? "Death" : "Water"));
	}
}
