using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

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
