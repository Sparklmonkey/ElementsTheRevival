using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_SoundManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Game_SoundManager.shared.PlayAudioClip("TapButton");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Game_SoundManager.shared.PlayAudioClip("HoverOverButton");
    }
}
