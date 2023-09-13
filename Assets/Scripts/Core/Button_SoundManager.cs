using UnityEngine;
using UnityEngine.EventSystems;

public class Button_SoundManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlayAudioClip("TapButton");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayAudioClip("HoverOverButton");
    }
}
