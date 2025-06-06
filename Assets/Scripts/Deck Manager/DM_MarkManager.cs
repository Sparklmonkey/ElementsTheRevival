using Deck_Manager.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DmMarkManager : MonoBehaviour
{
    [SerializeField]
    private Image markImage, headBackground;
    [SerializeField]
    private TextMeshProUGUI markName;
    private Element _markElement;

    public Element GetMarkSelected()
    {
        return _markElement;
    }

    public void SetupMarkCard(int element)
    {
        _markElement = (Element)element;
        //Card mark = CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.markIds[(int)markElement]);
        markImage.sprite = ImageHelper.GetElementImage(((Element)element).FastElementString());
        headBackground.sprite = ImageHelper.GetCardHeadBackground(((Element)element).FastElementString());
        markName.text = $"Mark of {_markElement}";
        EventBus<UpdateCurrentDeckEvent>.Raise(new UpdateCurrentDeckEvent(new(), (int)_markElement));
    }

}
