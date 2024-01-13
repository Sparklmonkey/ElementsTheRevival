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
        cardImage.sprite = ImageHelper.GetCardImage(card.ImageId);
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
    public string ImageId;
    public string Element;
    public string CardName;
    public DisplayCardPlayedEvent(string imageId, string element, string cardName)
    {
        ImageId = imageId;
        Element = element;
        CardName = cardName;
    }
}