using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDetailToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CardDisplayer displayer = GetComponent<CardDisplayer>();
        Card cardToDisplay = displayer.GetCardOnDisplay();

        if(cardToDisplay == null && gameObject.name.Contains("Weapon"))
        {
            cardToDisplay = CardDatabase.GetCardFromResources("Weapon", "Weapon", false);
        }
        else if(cardToDisplay == null && gameObject.name.Contains("Shield"))
        {
            cardToDisplay = CardDatabase.GetCardFromResources("Shield", "Shield", false);
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 objectSize = new Vector2(rectTransform.rect.height, rectTransform.rect.width);
        ToolTipCanvas.Instance.SetupToolTip(new Vector2(transform.position.x, transform.position.y), objectSize, cardToDisplay, displayer.GetObjectID().Index + 1, gameObject.name.Contains("Creature"));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipCanvas.Instance.HideToolTip();
    }
}
