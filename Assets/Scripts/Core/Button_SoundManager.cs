using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private bool isTransitionButton;
    [SerializeField] private string nextScene;
    public void OnPointerClick(PointerEventData eventData)
    {
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("TapButton"));
        if (isTransitionButton)
        {
            SceneTransitionManager.Instance.LoadScene(nextScene);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventBus<PlaySoundEffectEvent>.Raise(new PlaySoundEffectEvent("HoverOverButton"));
    }
}
