using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckPresetHead : MonoBehaviour, IPointerClickHandler
{
    private string deckCode;
    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private Image cardElement;

    [SerializeField]
    private TMP_FontAsset underlayBlack;
    private DeckDisplayManager manager;
    public void SetupCardHead(string deckName, string markElement, string deckCode, DeckDisplayManager manager)
    {
        this.manager = manager;
        this.deckCode = deckCode;
        cardName.text = deckName;
        cardName.font = underlayBlack;
        cardName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        cardElement.sprite = ImageHelper.GetCardBackGroundImage(markElement);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(deckCode);
        manager.SetupDeckPresetView(deckCode);
    }

}
