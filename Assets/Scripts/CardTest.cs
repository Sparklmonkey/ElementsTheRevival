using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        AuthenticationService.Instance.SignInAnonymouslyAsync();
        
    }

    private void ViewedNewsTest()
    {
        var idList = "";
        foreach (var card in CardDatabase.Instance.FullCardList)
        {
            idList += $" \"{card.Id}\",";
        }
        
        Debug.Log(idList);
        // for (int i = 0; i < 12; i++)
        // {
        //     var regularNymph = CardDatabase.Instance.GetRandomRegularNymph((Element)i);
        //     var uppedNymph = CardDatabase.Instance.GetRandomEliteNymph((Element)i);
        //     Debug.Log($"Regular ID: {regularNymph.Id}");
        //     Debug.Log($"Upped ID: {uppedNymph.Id}");
        //     Debug.Log($"Element: {(Element)i}");
        // }
    } 
}

[Serializable]
public class CardDBLegacy
{
    public List<CardDefinition> cardDb;
} 