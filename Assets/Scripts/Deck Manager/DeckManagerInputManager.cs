using TMPro;
using UnityEngine;

public class DeckCodeManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField legacyDeckCodeField, oetgDeckCodeField;

    public void SetupFields(string deckCode, Element markElement)
    {
        if (deckCode.Contains(" "))
        {
            legacyDeckCodeField.text = deckCode + " " + CardDatabase.Instance.MarkIds[(int)markElement];
            oetgDeckCodeField.text = legacyDeckCodeField.text.ConvertLegacyToOetg();
        }
        else
        {
            oetgDeckCodeField.text = deckCode;
            legacyDeckCodeField.text = deckCode.ConvertOetgToLegacy();
        }
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
