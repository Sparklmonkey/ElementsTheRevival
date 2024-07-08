using System;
using System.Collections.Generic;
using System.Linq;
using Networking;
using Newtonsoft.Json;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        ViewedNewsTest();
    }

    private void ViewedNewsTest()
    {
        for (int i = 0; i < 12; i++)
        {
            var regularNymph = CardDatabase.Instance.GetRandomRegularNymph((Element)i);
            var uppedNymph = CardDatabase.Instance.GetRandomEliteNymph((Element)i);
            Debug.Log($"Regular ID: {regularNymph.Id}");
            Debug.Log($"Upped ID: {uppedNymph.Id}");
            Debug.Log($"Element: {(Element)i}");
        }
    } 
}

[Serializable]
public class CardDBLegacy
{
    public List<CardDefinition> cardDb;
} 