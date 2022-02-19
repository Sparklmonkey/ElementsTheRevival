//// Elements.Duel.Visuals.ActionVisual
//using Elements.Core;
//using Elements.Duel.Visuals;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class ActionVisual : MonoBehaviour
//{
//	[SerializeField]
//	private TextMeshProUGUI actionTaken;

//	[SerializeField]
//	private Image cardPlayed;

//	[SerializeField]
//	private Image targetImage;

//	[SerializeField]
//	private TextMeshProUGUI targetText;

//	[SerializeField]
//	private Image arrowImage;

//	public void SetupActionObject(string cardPlayedName, string playedOwner, string targetName = null, string targetOwner = null)
//	{
//		if (playedOwner == "You")
//		{
//			if (targetName == null)
//			{
//				targetImage.gameObject.SetActive(value: false);
//				targetText.gameObject.SetActive(value: false);
//				arrowImage.gameObject.SetActive(value: false);
//			}
//			else
//			{
//				targetText.text = targetName + "\n(" + targetOwner + ")";
//				if (targetName.Equals("Player") || targetName.Contains("All "))
//				{
//					targetImage.gameObject.SetActive(value: false);
//				}
//				else
//				{
//					targetImage.sprite = ImageHelper.GetCardImage(targetName.RemoveWhiteSpaces());
//				}
//			}
//			actionTaken.text = cardPlayedName + "\n(" + playedOwner + ")";
//			if (cardPlayedName == "Turn Ended")
//			{
//				cardPlayed.gameObject.SetActive(value: false);
//			}
//			else
//			{
//				cardPlayed.sprite = ImageHelper.GetCardImage(cardPlayedName.RemoveWhiteSpaces());
//			}
//			return;
//		}
//		if (targetName == null)
//		{
//			actionTaken.gameObject.SetActive(value: false);
//			cardPlayed.gameObject.SetActive(value: false);
//			arrowImage.gameObject.SetActive(value: false);
//		}
//		else
//		{
//			actionTaken.text = targetName + "\n(" + targetOwner + ")";
//			arrowImage.transform.Rotate(new Vector3(0f, 180f, 0f));
//			if (targetName.Equals("Player") || targetName.Contains("All "))
//			{
//				cardPlayed.gameObject.SetActive(value: false);
//			}
//			else
//			{
//				cardPlayed.sprite = ImageHelper.GetCardImage(targetName.RemoveWhiteSpaces());
//			}
//		}
//		targetText.text = cardPlayedName + "\n(" + playedOwner + ")";
//		if (cardPlayedName == "Turn Ended")
//		{
//			targetImage.gameObject.SetActive(value: false);
//		}
//		else
//		{
//			targetImage.sprite = ImageHelper.GetCardImage(cardPlayedName.RemoveWhiteSpaces());
//		}
//	}

//	public void SetupActivateAbilityActionObject(string cardPlayedName, string playedOwner, string abilityName, string targetName = null, string targetOwner = null)
//	{
//		if (playedOwner == "You")
//		{
//			if (targetName == null)
//			{
//				targetImage.gameObject.SetActive(value: false);
//				targetText.gameObject.SetActive(value: false);
//				arrowImage.gameObject.SetActive(value: false);
//			}
//			else
//			{
//				targetText.text = targetName + "\n(" + targetOwner + ")";
//				if (targetName.Equals("Player") || targetName.Contains("All "))
//				{
//					targetImage.gameObject.SetActive(value: false);
//				}
//				else
//				{
//					targetImage.sprite = ImageHelper.GetCardImage(targetName.RemoveWhiteSpaces());
//				}
//			}
//			actionTaken.text = abilityName + "\n(" + playedOwner + ")";
//			if (cardPlayedName == "Turn Ended")
//			{
//				cardPlayed.gameObject.SetActive(value: false);
//			}
//			else
//			{
//				cardPlayed.sprite = ImageHelper.GetCardImage(cardPlayedName.RemoveWhiteSpaces());
//			}
//			return;
//		}
//		if (targetName == null)
//		{
//			actionTaken.gameObject.SetActive(value: false);
//			cardPlayed.gameObject.SetActive(value: false);
//			arrowImage.gameObject.SetActive(value: false);
//		}
//		else
//		{
//			actionTaken.text = abilityName + "\n(" + targetOwner + ")";
//			arrowImage.transform.Rotate(new Vector3(0f, 180f, 0f));
//			if (targetName.Equals("Player") || targetName.Contains("All "))
//			{
//				cardPlayed.gameObject.SetActive(value: false);
//			}
//			else
//			{
//				cardPlayed.sprite = ImageHelper.GetCardImage(targetName.RemoveWhiteSpaces());
//			}
//		}
//		targetText.text = cardPlayedName + "\n(" + playedOwner + ")";
//		if (cardPlayedName == "Turn Ended")
//		{
//			targetImage.gameObject.SetActive(value: false);
//		}
//		else
//		{
//			targetImage.sprite = ImageHelper.GetCardImage(cardPlayedName.RemoveWhiteSpaces());
//		}
//	}
//}


//// Elements.Duel.Visuals.DuelManagerVisuals
//using System.Collections.Generic;
//using Elements.Duel.Visuals;
//using TMPro;
//using UnityEngine;

//public class DuelManagerVisuals : MonoBehaviour
//{
//	[SerializeField]
//	private TextMeshProUGUI latestAction;

//	private List<GameObject> actionList = new List<GameObject>();

//	[SerializeField]
//	private GameObject fullActionListDisplay;

//	[SerializeField]
//	private GameObject contentView;

//	[SerializeField]
//	private GameObject actionPrefab;

//	public void UpdateLatestAction(string cardName, string cardOwner, string targetName = null, string targetOwner = null)
//	{
//		GameObject gameObject = Object.Instantiate(actionPrefab);
//		gameObject.GetComponentInChildren<ActionVisual>().SetupActionObject(cardName, cardOwner, targetName, targetOwner);
//		actionList.Add(gameObject);
//		string text = cardName + " (" + cardOwner + ")";
//		if (targetName != null)
//		{
//			text = text + " -> " + targetName + " (" + targetOwner + ")";
//		}
//		latestAction.text = text;
//	}

//	public void UpdateLatestActivateAbilityAction(string cardName, string cardOwner, string abilityName, string targetName = null, string targetOwner = null)
//	{
//		GameObject gameObject = Object.Instantiate(actionPrefab);
//		gameObject.GetComponentInChildren<ActionVisual>().SetupActivateAbilityActionObject(cardName, cardOwner, abilityName, targetName, targetOwner);
//		actionList.Add(gameObject);
//		string text = abilityName + " (" + cardOwner + ")";
//		if (targetName != null)
//		{
//			text = text + " -> " + targetName + " (" + targetOwner + ")";
//		}
//		latestAction.text = text;
//	}

//	public void DisplayListOnclick()
//	{
//		if (fullActionListDisplay.activeSelf)
//		{
//			HideFullActionList();
//		}
//		else
//		{
//			DisplayFullActionList();
//		}
//	}

//	private void DisplayFullActionList()
//	{
//		fullActionListDisplay.SetActive(value: true);
//		foreach (Transform item in contentView.transform)
//		{
//			Object.Destroy(item.gameObject);
//		}
//		int index = actionList.Count - 1;
//		while (index-- > 0)
//		{
//			Object.Instantiate(actionList[index], contentView.transform);
//		}
//	}

//	private void HideFullActionList()
//	{
//		fullActionListDisplay.SetActive(value: false);
//		foreach (Transform item in contentView.transform)
//		{
//			Object.Destroy(item.gameObject);
//		}
//	}
//}

//// Elements.Duel.Visuals.ExtensionMethods
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public static class ExtensionMethods
//{
//	public static System.Random rnd = new System.Random();

//	public static Transform[] FindChildren(this Transform transform, string name)
//	{
//		return (from t in transform.GetComponentsInChildren<Transform>()
//			where t.name == name
//			select t).ToArray();
//	}

//	public static int SumIntList(this List<int> intList)
//	{
//		int num = 0;
//		foreach (int @int in intList)
//		{
//			num += @int;
//		}
//		return num;
//	}

//	public static void Shuffle<T>(this IList<T> list)
//	{
//		int num = list.Count;
//		while (num > 1)
//		{
//			int index = rnd.Next(0, num) % num;
//			num--;
//			T value = list[index];
//			list[index] = list[num];
//			list[num] = value;
//		}
//	}

//	public static string RemoveWhiteSpaces(this string text)
//	{
//		return text.Replace(" ", "");
//	}

//	public static int GetIntFromStack(this string text)
//	{
//		if (!(text != ""))
//		{
//			return 1;
//		}
//		return int.Parse(text.Replace("x", ""));
//	}
//}
