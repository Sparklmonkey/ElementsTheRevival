using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPlayedDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image cardImage, headerBackground;
    [SerializeField] private GameObject container;

    private EventBinding<DisplayCardPlayedEvent> _displayCardPlayedBinding;
    private void OnDisable()
    {
        EventBus<DisplayCardPlayedEvent>.Unregister(_displayCardPlayedBinding);
    }

    private void Awake()
    {
        _displayCardPlayedBinding = new EventBinding<DisplayCardPlayedEvent>(ShowCardPlayed);
        EventBus<DisplayCardPlayedEvent>.Register(_displayCardPlayedBinding);
    }

    public void ShowCardPlayed(DisplayCardPlayedEvent card)
    {
        container.SetActive(true);
        cardName.text = card.CardName;
        cardImage.sprite = card.Sprite;
        headerBackground.sprite = ImageHelper.GetCardHeadBackground(card.Element);
        StartCoroutine(HideCardPlayed());
    }
    
    private IEnumerator HideCardPlayed()
    {
        yield return new WaitForSeconds(0.5f);
        container.SetActive(false);
    }
}

public struct DisplayCardPlayedEvent : IEvent
{
    public Sprite Sprite;
    public string Element;
    public string CardName;
    public DisplayCardPlayedEvent(Sprite sprite, string element, string cardName)
    {
        Sprite = sprite;
        Element = element;
        CardName = cardName;
    }
}