using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarterDeckElementButton : MonoBehaviour, IPointerEnterHandler
{
    public Element element;
    
    [SerializeField]
    private TextMeshProUGUI elementTitle;
    [SerializeField]
    private Image elementImage, buttonImage;
    [SerializeField]
    private DeckSelector manager;

    private void Awake()
    {
        elementImage.sprite = ImageHelper.GetElementImage(element.FastElementString());
        buttonImage.color = GetButtonTint();
        elementTitle.text = element.ToString();
        elementTitle.color = GetTextColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.ElementSelection(element);
    }

    private Color32 GetButtonTint()
    {
        return element switch
        {
            Element.Earth => ImageHelper.HexToColor32("FFCCB3"),
            Element.Aether => ImageHelper.HexToColor32("6ECBBB"),
            Element.Air => ImageHelper.HexToColor32("9EB1FF"),
            Element.Darkness => ImageHelper.HexToColor32("6D6F74"),
            Element.Light => ImageHelper.HexToColor32("DEDEDC"),
            Element.Death => ImageHelper.HexToColor32("927BBC"),
            Element.Entropy => ImageHelper.HexToColor32("DE92DE"),
            Element.Time => ImageHelper.HexToColor32("E1CC68"),
            Element.Fire => ImageHelper.HexToColor32("DE6958"),
            Element.Gravity => ImageHelper.HexToColor32("FFA367"),
            Element.Life => ImageHelper.HexToColor32("73C773"),
            Element.Water => ImageHelper.HexToColor32("6784D1"),
            _ => ImageHelper.HexToColor32("FFCCB3")
        };
    }

    private Color32 GetTextColor()
    {
        return element switch
        {
            Element.Earth => ImageHelper.HexToColor32("663300"),
            Element.Aether => ImageHelper.HexToColor32("488A7D"),
            Element.Air => ImageHelper.HexToColor32("0033FF"),
            Element.Darkness => ImageHelper.HexToColor32("101016"),
            Element.Light => ImageHelper.HexToColor32("DEDFDD"),
            Element.Death => ImageHelper.HexToColor32("522E98"),
            Element.Entropy => ImageHelper.HexToColor32("CE67CE"),
            Element.Time => ImageHelper.HexToColor32("9A6600"),
            Element.Fire => ImageHelper.HexToColor32("D42F00"),
            Element.Gravity => ImageHelper.HexToColor32("FF7B10"),
            Element.Life => ImageHelper.HexToColor32("18B621"),
            Element.Water => ImageHelper.HexToColor32("001D8E"),
            _ => ImageHelper.HexToColor32("FFCCB3")
        };
    }
}

