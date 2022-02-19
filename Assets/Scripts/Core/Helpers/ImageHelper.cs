using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageHelper
{
	public static Sprite GetCardHeadBackground(string element)
	{
		return Resources.Load<Sprite>("Sprites/CardHeadBackground/" + element);
	}
	public static Sprite GetCardImage(string imageID)
    {
		string path = "Sprites/CardImages/" + imageID;
		return Resources.Load<Sprite>(path);
	}

	public static Sprite GetPendulumImage(string imageID, string mElement)
	{
		string path = "Sprites/CardImages/Pendulum/" + imageID + "/" + mElement;
		return Resources.Load<Sprite>(path);
	}

	public static Sprite GetElementImage(string element)
	{
		return Resources.Load<Sprite>("Sprites/Elements/" + element);
	}

	public static Sprite GetCardBackGroundImage(string element)
	{
		return Resources.Load<Sprite>("Sprites/CardBackground/" + element);
	}

	public static Sprite GetCardTypeImage(string type)
    {
		return Resources.Load<Sprite>("Sprites/CardTypes/" + type);
	}
}
