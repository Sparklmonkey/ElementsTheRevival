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

    private async void ViewedNewsTest()
    {
        
        var response = await ApiManager.Instance.LoginController(new LoginRequest
        {
            username = "TheGreat",
            password = "TheGreat"
        }, Endpointbuilder.UserCredentialLogin);

        var testOne = await ApiManager.Instance.HasSeenLatestNews();
        Debug.Log(testOne.booleanValue);
        
        var testTwo = await ApiManager.Instance.UpdateSeenNews(new ViewedNewsRequest
        {
            newsId = 1
        });
        Debug.Log(testTwo.booleanValue);
    } 
}

[Serializable]
public class CardDBLegacy
{
    public List<CardDefinition> cardDb;
} 