//// TestScriptCreator
//using UnityEngine;

//public class TestScriptCreator : MonoBehaviour
//{
//	private void Start()
//	{
//	}

//	private void Update()
//	{
//	}
//}

//// CardHidden
//using Elements.Core;
//using UnityEngine;

//public class CardHidden : MonoBehaviour
//{
//	private CardBase displayCard;

//	private void Start()
//	{
//	}

//	public void SetupCardView(CardBase cardToDisplay)
//	{
//		displayCard = cardToDisplay;
//	}
//}

//// EndTurnManager
//public class EndTurnManager
//{
//}

//// IconPreview
//using UnityEngine;
//using UnityEngine.UI;

//public class IconPreview : MonoBehaviour
//{
//	public Sprite[] icons;

//	private GameObject icon;

//	private void Awake()
//	{
//		for (int i = 0; i < icons.Length; i++)
//		{
//			icon = new GameObject("icon" + i);
//			icon.transform.SetParent(base.gameObject.transform);
//			icon.AddComponent<RectTransform>();
//			icon.AddComponent<Image>();
//			icon.GetComponent<Image>().sprite = icons[i];
//		}
//	}

//	private void Update()
//	{
//	}
//}
