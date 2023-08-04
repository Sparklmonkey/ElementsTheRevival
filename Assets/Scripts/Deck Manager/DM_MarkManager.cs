using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DM_MarkManager : MonoBehaviour
{
    [SerializeField]
    private Image markImage, headBackground;
    [SerializeField]
    private TextMeshProUGUI markName;
    private Element markElement;

    public Element GetMarkSelected()
    {
        return markElement;
    }

    public void SetupMarkCard(int element)
    {
        markElement = (Element)element;
        //Card mark = CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.markIds[(int)markElement]);
        markImage.sprite = ImageHelper.GetElementImage(((Element)element).FastElementString());
        headBackground.sprite = ImageHelper.GetCardHeadBackground(((Element)element).FastElementString());
        markName.text = $"Mark of {markElement}"; 
    }

}
