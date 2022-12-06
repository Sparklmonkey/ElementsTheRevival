//// Elements.Duel.Visuals.CardObjectOnField
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class CardObjectOnField : MonoBehaviour
//{
//	public int index;

//	private string cardType;

//	[SerializeField]
//	private Image cardImage;

//	[SerializeField]
//	private Image mask;

//	[SerializeField]
//	private TextMeshProUGUI textDisplay;

//	[SerializeField]
//	private Button cardButton;

//	[SerializeField]
//	private Transform counterParent;

//	[SerializeField]
//	private Material dissolveMat;

//	private void Start()
//	{
//		textDisplay.text = "";
//	}

//	public void SetupObject(int index, Action action)
//	{
//		this.index = index;
//		cardButton.onClick.AddListener(delegate
//		{
//			action();
//		});
//		base.gameObject.SetActive(value: false);
//	}

//	public void FirstSetupCardOnField(string cardName, int power, int toughness, string cardType)
//	{
//		this.cardType = cardType;
//		base.gameObject.SetActive(value: true);
//		cardImage.sprite = ImageHelper.GetCardImage(cardName.RemoveWhiteSpaces());
//		StartCoroutine(DissolveOnSummon());
//		if (power != 0 || toughness != 0)
//		{
//			textDisplay.text = power + "/" + toughness;
//		}
//	}

//	public void SetupCardOnField(string cardName, int power, int toughness)
//	{
//		base.gameObject.SetActive(value: true);
//		if (power != 0 || toughness != 0)
//		{
//			textDisplay.text = power + "/" + toughness;
//		}
//	}

//	private IEnumerator DissolveOnSummon()
//	{
//		mask.enabled = false;
//		dissolveMat.SetTexture("_mainTexture", cardImage.sprite.texture);
//		cardImage.material = new Material(dissolveMat);
//		Material shader = cardImage.material;
//		shader.SetFloat("_DissolveAmount", 1f);
//		shader.SetFloat("_NoiseScale", 25f);
//		float currentTime = 1f;
//		while (currentTime > 0f)
//		{
//			float value = currentTime / 1f;
//			currentTime -= Time.deltaTime;
//			shader.SetFloat("_DissolveAmount", value);
//			yield return null;
//		}
//		shader.SetFloat("_DissolveAmount", 0f);
//		cardImage.material = null;
//		mask.enabled = true;
//	}

//	private IEnumerator DissolveOnDestroy()
//	{
//		mask.enabled = false;
//		dissolveMat.SetTexture("_mainTexture", cardImage.sprite.texture);
//		cardImage.material = new Material(dissolveMat);
//		Material shader = cardImage.material;
//		shader.SetFloat("_DissolveAmount", 0f);
//		shader.SetFloat("_NoiseScale", 25f);
//		float currentTime = 0f;
//		while (currentTime < 1f)
//		{
//			float value = currentTime / 1f;
//			currentTime += Time.deltaTime;
//			shader.SetFloat("_DissolveAmount", value);
//			yield return null;
//		}
//		shader.SetFloat("_DissolveAmount", 1f);
//		cardImage.material = null;
//		mask.enabled = true;
//		base.gameObject.SetActive(value: false);
//	}


//	public void UpdateCounters(List<int> counterList)
//	{
//		foreach (Transform item in counterParent)
//		{
//			UnityEngine.Object.Destroy(item.gameObject);
//		}
//		for (int i = 0; i < counterList.Count; i++)
//		{
//			if (counterList[i] != 0)
//			{
//				for (int j = 0; j < counterList[i]; j++)
//				{
//					UnityEngine.Object.Instantiate(GetCounterObject(i), counterParent);
//				}
//			}
//		}
//	}

//	public void StartQuantaGenerateAnimation(Element element)
//	{
//		StartCoroutine(GenerateQuantaDissolve(element));
//	}

//	private GameObject GetCounterObject(int i)
//	{
//		return Resources.Load<GameObject>("Prefabs/Counters/" + i switch
//		{
//			0 => "Purity",
//			1 => "Freeze",
//			2 => "Poison",
//			3 => "Invisible",
//			4 => "Bubble",
//			_ => "Poison",
//		});
//	}

//	public void StackPillar()
//	{
//		if (textDisplay.text == "")
//		{
//			textDisplay.text = "x2";
//		}
//		else
//		{
//			textDisplay.text = "x" + (textDisplay.text.GetIntFromStack() + 1);
//		}
//	}

//	public void SetStack(int amount)
//	{
//		if (amount == 0)
//		{
//			ClearCardObject();
//		}
//		else
//		{
//			textDisplay.text = "x" + amount;
//		}
//	}

//	public void ClearCardObject()
//	{
//		textDisplay.text = "";
//		StartCoroutine(DissolveOnDestroy());
//	}

//	public int GetStackCount()
//	{
//		return textDisplay.text.GetIntFromStack();
//	}

//	public bool DestroyPillarStack(int amount)
//	{
//		int num = textDisplay.text.GetIntFromStack() - amount;
//		if (num < 1)
//		{
//			ClearCardObject();
//			return true;
//		}
//		textDisplay.text = "x" + num;
//		return false;
//	}
//}

////// Elements.Duel.Visuals.CardObjectOnField
////using System;
////using System.Collections;
////using System.Collections.Generic;
////using Elements.Core;
////using Elements.Duel.Visuals;
////using TMPro;
////using UnityEngine;
////using UnityEngine.UI;

////public class CardObjectOnField : MonoBehaviour
////{
////	public int index;

////	private string cardType;

////	[SerializeField]
////	private Image cardImage;

////	[SerializeField]
////	private Image mask;

////	[SerializeField]
////	private TextMeshProUGUI textDisplay;

////	[SerializeField]
////	private Button cardButton;

////	[SerializeField]
////	private Transform counterParent;

////	[SerializeField]
////	private Material dissolveMat;

////	private void Start()
////	{
////		textDisplay.text = "";
////	}

////	public void SetupObject(int index, Action action)
////	{
////		this.index = index;
////		cardButton.onClick.AddListener(delegate
////		{
////			action();
////		});
////		base.gameObject.SetActive(value: false);
////	}

////	public void FirstSetupCardOnField(string cardName, int power, int toughness, string cardType)
////	{
////		this.cardType = cardType;
////		base.gameObject.SetActive(value: true);
////		cardImage.sprite = ImageHelper.GetCardImage(cardName.RemoveWhiteSpaces());
////		StartCoroutine(DissolveOnSummon());
////		if (power != 0 || toughness != 0)
////		{
////			textDisplay.text = power + "/" + toughness;
////		}
////	}

////	public void SetupCardOnField(string cardName, int power, int toughness)
////	{
////		base.gameObject.SetActive(value: true);
////		if (power != 0 || toughness != 0)
////		{
////			textDisplay.text = power + "/" + toughness;
////		}
////	}

////	private IEnumerator DissolveOnSummon()
////	{
////		mask.enabled = false;
////		dissolveMat.SetTexture("_mainTexture", cardImage.sprite.texture);
////		cardImage.material = new Material(dissolveMat);
////		Material shader = cardImage.material;
////		shader.SetFloat("_DissolveAmount", 1f);
////		shader.SetFloat("_NoiseScale", 25f);
////		float currentTime = 1f;
////		while (currentTime > 0f)
////		{
////			float value = currentTime / 1f;
////			currentTime -= Time.deltaTime;
////			shader.SetFloat("_DissolveAmount", value);
////			yield return null;
////		}
////		shader.SetFloat("_DissolveAmount", 0f);
////		cardImage.material = null;
////		mask.enabled = true;
////	}

////	private IEnumerator DissolveOnDestroy()
////	{
////		mask.enabled = false;
////		dissolveMat.SetTexture("_mainTexture", cardImage.sprite.texture);
////		cardImage.material = new Material(dissolveMat);
////		Material shader = cardImage.material;
////		shader.SetFloat("_DissolveAmount", 0f);
////		shader.SetFloat("_NoiseScale", 25f);
////		float currentTime = 0f;
////		while (currentTime < 1f)
////		{
////			float value = currentTime / 1f;
////			currentTime += Time.deltaTime;
////			shader.SetFloat("_DissolveAmount", value);
////			yield return null;
////		}
////		shader.SetFloat("_DissolveAmount", 1f);
////		cardImage.material = null;
////		mask.enabled = true;
////		base.gameObject.SetActive(value: false);
////	}

////	private IEnumerator GenerateQuantaDissolve(Element element)
////	{
////		mask.enabled = false;
////		dissolveMat.SetTexture("_mainTexture", cardImage.sprite.texture);
////		cardImage.material = new Material(dissolveMat);
////		Material shader = cardImage.material;
////		shader.SetFloat("_DissolveAmount", 0f);
////		shader.SetFloat("_NoiseScale", 25f);
////		shader.SetFloat("_EdgeWidth", 0.2f);
////		switch (element)
////		{
////			case Element.Darkness:
////				shader.SetColor("_DissolveColor", ElementColors.darknessColor);
////				break;
////			case Element.Death:
////				shader.SetColor("_DissolveColor", ElementColors.deathColor);
////				break;
////			case Element.Earth:
////				shader.SetColor("_DissolveColor", ElementColors.earthColor);
////				break;
////			case Element.Fire:
////				shader.SetColor("_DissolveColor", ElementColors.fireColor);
////				break;
////			case Element.Gravity:
////				shader.SetColor("_DissolveColor", ElementColors.gravityColor);
////				break;
////			case Element.Time:
////				shader.SetColor("_DissolveColor", ElementColors.timeColor);
////				break;
////			case Element.Water:
////				shader.SetColor("_DissolveColor", ElementColors.waterColor);
////				break;
////			case Element.Life:
////				shader.SetColor("_DissolveColor", ElementColors.lifeColor);
////				break;
////			case Element.Aether:
////				shader.SetColor("_DissolveColor", ElementColors.aetherColor);
////				break;
////			case Element.Air:
////				shader.SetColor("_DissolveColor", ElementColors.airColor);
////				break;
////			case Element.Entropy:
////				shader.SetColor("_DissolveColor", ElementColors.entropyColor);
////				break;
////			case Element.Light:
////				shader.SetColor("_DissolveColor", ElementColors.lightColor);
////				break;
////			case Element.Other:
////				shader.SetColor("_DissolveColor", ElementColors.earthColor);
////				break;
////			default:
////				shader.SetColor("_DissolveColor", ElementColors.earthColor);
////				break;
////		}
////		float currentTime = 0f;
////		while (currentTime < 0.5f)
////		{
////			float value = currentTime / 0.5f;
////			currentTime += Time.deltaTime;
////			shader.SetFloat("_DissolveAmount", value);
////			yield return null;
////		}
////		while (currentTime > 0f)
////		{
////			float value2 = currentTime / 0.5f;
////			currentTime -= Time.deltaTime;
////			shader.SetFloat("_DissolveAmount", value2);
////			yield return null;
////		}
////		shader.SetFloat("_DissolveAmount", 0f);
////		cardImage.material = null;
////		mask.enabled = true;
////	}

////	public void UpdateCounters(List<int> counterList)
////	{
////		foreach (Transform item in counterParent)
////		{
////			UnityEngine.Object.Destroy(item.gameObject);
////		}
////		for (int i = 0; i < counterList.Count; i++)
////		{
////			if (counterList[i] != 0)
////			{
////				for (int j = 0; j < counterList[i]; j++)
////				{
////					UnityEngine.Object.Instantiate(GetCounterObject(i), counterParent);
////				}
////			}
////		}
////	}

////	public void StartQuantaGenerateAnimation(Element element)
////	{
////		StartCoroutine(GenerateQuantaDissolve(element));
////	}

////	private GameObject GetCounterObject(int i)
////	{
////		return Resources.Load<GameObject>("Prefabs/Counters/" + i switch
////		{
////			0 => "Purity",
////			1 => "Freeze",
////			2 => "Poison",
////			3 => "Invisible",
////			4 => "Bubble",
////			_ => "Poison",
////		});
////	}

////	public void StackPillar()
////	{
////		if (textDisplay.text == "")
////		{
////			textDisplay.text = "x2";
////		}
////		else
////		{
////			textDisplay.text = "x" + (textDisplay.text.GetIntFromStack() + 1);
////		}
////	}

////	public void SetStack(int amount)
////	{
////		if (amount == 0)
////		{
////			ClearCardObject();
////		}
////		else
////		{
////			textDisplay.text = "x" + amount;
////		}
////	}

////	public void ClearCardObject()
////	{
////		textDisplay.text = "";
////		StartCoroutine(DissolveOnDestroy());
////	}

////	public int GetStackCount()
////	{
////		return textDisplay.text.GetIntFromStack();
////	}

////	public bool DestroyPillarStack(int amount)
////	{
////		int num = textDisplay.text.GetIntFromStack() - amount;
////		if (num < 1)
////		{
////			ClearCardObject();
////			return true;
////		}
////		textDisplay.text = "x" + num;
////		return false;
////	}
////}
