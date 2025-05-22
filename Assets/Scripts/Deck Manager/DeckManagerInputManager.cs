using System;
using System.Collections.Generic;
using System.Linq;
using Deck_Manager.Events;
using Networking;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class DeckCodeManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField legacyDeckCodeField, oetgDeckCodeField;
    [SerializeField]
    private GameObject deckAObject, deckBObject, deckCObject, deckCodePopUpObject;
    private DeckPresets _deckPresets;
    private List<string> _currentDeck;
    private string _markId;
    private IEventBinding<UpdateCurrentDeckEvent> _updateCurrentDeckEvent;
    private void Awake()
    {
        LoadDeckPresets();
        _updateCurrentDeckEvent = new EventBinding<UpdateCurrentDeckEvent>(UpdateCurrentDeck);
        EventBus<UpdateCurrentDeckEvent>.Register(_updateCurrentDeckEvent);
    }

    private void OnDestroy()
    {
        EventBus<UpdateCurrentDeckEvent>.Unregister(_updateCurrentDeckEvent);
    }

    public void OpenDeckCodePopUp()
    {
        deckCodePopUpObject.SetActive(true);
        SetupFields();
    }
    
    private void UpdateCurrentDeck(UpdateCurrentDeckEvent updateCurrentDeckEvent)
    {
        _currentDeck = updateCurrentDeckEvent.DeckString.IsNullOrEmpty() ? _currentDeck : updateCurrentDeckEvent.DeckString;
        _markId = CardDatabase.Instance.markIds[updateCurrentDeckEvent.Mark];
    }
    private async void LoadDeckPresets()
    {
        _deckPresets = await ApiManager.Instance.GetDeckPresets();
        var isEnabled = RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.DeckPresets);
        deckAObject.SetActive(isEnabled);
        deckBObject.SetActive(isEnabled);
        deckCObject.SetActive(isEnabled);
    }

    private void SetupFields()
    {
        var deck = string.Join(" ", _currentDeck.ToArray());
        if (!deck.Contains(_markId))
        {
            deck = deck + " " + _markId;
        }
        legacyDeckCodeField.text = deck;
        oetgDeckCodeField.text = deck.ConvertLegacyToOetg();
    }

    public async void UpdateDeckPreset(int deckPreset)
    {
        var deck = _currentDeck;
        if (!deck.Contains(_markId))
        {
            deck.Add(_markId);
        }
        var deckCode = deck.ConvertListToCardCode();
        var response = await ApiManager.Instance.SaveDeckPresets(deckPreset == 0 ? deckCode : "",
                                                                    deckPreset == 1 ? deckCode : "", 
                                                                    deckPreset == 2 ? deckCode : "");
        _deckPresets = response;
    }

    public void LoadDeckFromPreset(int deckPreset)
    {
        var deckCode = new List<string>();
        switch (deckPreset)
        {
            case 0:
                deckCode = _deckPresets.deckA.ConvertCardCodeToList();
                break;
            case 1:
                deckCode = _deckPresets.deckB.ConvertCardCodeToList();
                break;
            case 2:
                deckCode = _deckPresets.deckC.ConvertCardCodeToList();
                break;
        }
        EventBus<UpdateDeckDisplayEvent>.Raise(new UpdateDeckDisplayEvent(deckCode));
    }
    public string GetDeckCode() => legacyDeckCodeField.text;

    public void UpdateDeckFields(TMP_InputField deckCode)
    {
        if (deckCode.text.Contains(" "))
        {
            legacyDeckCodeField.text = deckCode.text;
            oetgDeckCodeField.text = deckCode.text.ConvertLegacyToOetg();
        }
        else
        {
            oetgDeckCodeField.text = deckCode.text;
            legacyDeckCodeField.text = deckCode.text.ConvertOetgToLegacy();
        }
    }
}
