using System;
using Core.Helpers;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform backgroundRectTransform;

    [SerializeField] private CardDisplayDetail cardDisplayDetail;
    private RectTransform _bottomFieldRectTransform;
    private float _midPointX, _midPointY;
    private EventBinding<DisplayCardToolTipEvent> _setupCardDisplayBinding;
    private void Awake()
    {
        _midPointX = Screen.width / 2;
        _midPointY = Screen.height / 2;
        _setupCardDisplayBinding = new EventBinding<DisplayCardToolTipEvent>(ShowToolTip);
        EventBus<DisplayCardToolTipEvent>.Register(_setupCardDisplayBinding);
        
        cardDisplayDetail.gameObject.SetActive(false);
    }

    private void Update()
    {
        var mousePosition = Input.mousePosition;
        
        var offsetX = mousePosition.x < _midPointX ? backgroundRectTransform.rect.width / 2 : -backgroundRectTransform.rect.width / 2;
        var offsetY = mousePosition.y < _midPointY ? backgroundRectTransform.rect.height / 2 : -backgroundRectTransform.rect.height / 2;
        transform.position = Input.mousePosition + new Vector3(offsetX, offsetY, 0);

        var anchoredPosition = backgroundRectTransform.anchoredPosition;
        if (anchoredPosition.x + backgroundRectTransform.rect.width > Screen.width)
        {
            anchoredPosition.x = Screen.width - backgroundRectTransform.rect.width;
        }

        backgroundRectTransform.anchoredPosition = anchoredPosition;
    }

    private void ShowToolTip(DisplayCardToolTipEvent displayCardToolTipEvent)
    {
        if (displayCardToolTipEvent.IsHide)
        {
            cardDisplayDetail.gameObject.SetActive(false);    
            return;
        }
        cardDisplayDetail.gameObject.SetActive(true);    
        cardDisplayDetail.SetupCardView(displayCardToolTipEvent.Card, displayCardToolTipEvent.Id.IsOwnedBy(OwnerEnum.Player), true);
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }
        
}