using UnityEngine;
using UnityEngine.EventSystems;

public class StarterDeckElementButton : MonoBehaviour, IPointerEnterHandler
{
    public Element element;
    [SerializeField]
    private DeckSelector manager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.ElementSelection(element);
    }
}
