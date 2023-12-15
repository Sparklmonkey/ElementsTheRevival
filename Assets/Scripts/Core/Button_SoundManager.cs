using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("TapButton"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("HoverOverButton"));
    }
}
